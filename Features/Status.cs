using System;
using System.Collections.Generic;
using System.Linq;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using System.Text.RegularExpressions;
using Nickel;
using HarmonyLib;
using System.Reflection.Emit;
using System.Reflection;
using TheJazMaster.CatAndAmy.Cards;
using System.Runtime.CompilerServices;
using TheJazMaster.CatAndAmy.Artifacts;
using System.ComponentModel.DataAnnotations;
using Shockah.Kokoro;
using static Shockah.Kokoro.IKokoroApi.IV2.IStatusRenderingApi.IHook;
using static Shockah.Kokoro.IKokoroApi.IV2.IStatusLogicApi.IHook;
using static Shockah.Kokoro.IKokoroApi.IV2.IStatusLogicApi;
using Microsoft.Extensions.Logging;
namespace TheJazMaster.CatAndAmy.Features;

[HarmonyPatch]
public class StatusManager : IHook, IKokoroApi.IV2.IStatusRenderingApi.IHook
{
    private static ModEntry Instance => ModEntry.Instance;

    public StatusManager()
    {
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(this, 0);
        ModEntry.Instance.KokoroApi.StatusRendering.RegisterHook(this, 0);
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(AMove), nameof(AMove.Begin))]
    private static IEnumerable<CodeInstruction> AMove_Begin_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il, MethodBase originalMethod)
    {
        return new SequenceBlockMatcher<CodeInstruction>(instructions)
            .Find(
                ILMatches.Ldloc<Ship>(originalMethod).CreateLdlocInstruction(out var ldLoc),
                ILMatches.LdcI4(Status.strafe),
                ILMatches.Call("Get").Anchor(out var anchor),
                ILMatches.LdcI4(0),
                ILMatches.Instruction(OpCodes.Ldnull),
                ILMatches.Call("GetActualDamage"),
                ILMatches.Stfld("damage")
            )
            .Anchors()
            .PointerMatcher(anchor)
			.Insert(SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion, [
                ldLoc,
                new(OpCodes.Call, AccessTools.DeclaredMethod(typeof(StatusManager), nameof(GetTempStrafe))),
                new(OpCodes.Add)
            ])
            .AllElements();
    }

    private static int GetTempStrafe(Ship ship) => ship.Get(ModEntry.Instance.TempStrafeStatus);

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AMove), nameof(AMove.Begin))]
    private static void AMove_Begin_Postfix(AMove __instance, State s, Combat c) {
        Ship movingShip = __instance.targetPlayer ? s.ship : c.otherShip;
        Ship oppositeShip = __instance.targetPlayer ? c.otherShip : s.ship;

        int mStrafe = movingShip.Get(Status.strafe);
        int mTempStrafe = movingShip.Get(ModEntry.Instance.TempStrafeStatus);
        int mReflex = movingShip.Get(ModEntry.Instance.ReflexStatus);
        int mTempReflex = movingShip.Get(ModEntry.Instance.TempReflexStatus);
        
        int oStrafe = oppositeShip.Get(Status.strafe);
        int oTempStrafe = oppositeShip.Get(ModEntry.Instance.TempStrafeStatus);
        int oReflex = oppositeShip.Get(ModEntry.Instance.ReflexStatus);
        int oTempReflex = oppositeShip.Get(ModEntry.Instance.TempReflexStatus);

        int oLeadOn = oppositeShip.Get(ModEntry.Instance.LeadOnStatus);

        if (oLeadOn > 0) {
            c.QueueImmediate(new AMove {
                dir = __instance.dir + Math.Sign(__instance.dir)*oLeadOn,
                targetPlayer = !movingShip.isPlayerShip
            });
        }
        
        // Handled by vanilla
        // FireStrafe(__instance, s, c, mTempStrafe, movingShip, movingShip);
        
        if (c.isPlayerTurn)
            FireStrafe(__instance, s, c, oReflex + oTempReflex, oppositeShip, movingShip);

        if (s.EnumerateAllArtifacts().OfType<BeatEmUp>().FirstOrDefault() is { } artifact) {
            FireStrafe(__instance, s, c, mReflex + mTempReflex, movingShip, movingShip, artifact.Key());

            if (c.isPlayerTurn)
                FireStrafe(__instance, s, c, oStrafe + oTempStrafe, oppositeShip, movingShip, artifact.Key());
        }
    }

    private static void FireStrafe(AMove move, State s, Combat c, int damage, Ship shootingShip, Ship movingShip, string? artifactPulse = null) {
        if (damage == 0) return;

        if (move.dir != 0 || (movingShip.Get(Status.hermes) > 0 && !move.ignoreHermes))
            c.QueueImmediate(new AAttack() {
                damage = Card.GetActualDamage(s, damage),
                targetPlayer = !shootingShip.isPlayerShip,
                fast = true,
                storyFromStrafe = true,
                artifactPulse = artifactPulse
            });
    }

    private static bool HandleKisses(IHandleStatusTurnAutoStepArgs args) {
		if (args.Timing != StatusTurnTriggerTiming.TurnStart)
			return false;

		if (args.Status == Instance.KissesStatus && args.Amount > 0) {
            args.Combat.Queue(new AAddCard {
                card = new Kiss(),
                statusPulse = args.Status,
                destination = CardDestination.Hand
            });
        } else if (args.Status == Instance.KissesAStatus && args.Amount > 0) {
            args.Combat.Queue(new AAddCard {
                card = new Kiss {
                    upgrade = Upgrade.A
                },
                statusPulse = args.Status,
                destination = CardDestination.Hand
            });
        }
		return false;
    }

    private static bool HandleLovers(IHandleStatusTurnAutoStepArgs args) {
        if (args.Status != Instance.LoversStatus || args.Amount == 0)
			return false;
		if (args.Timing != StatusTurnTriggerTiming.TurnEnd)
			return false;
        if (args.Ship.Get(Status.timeStop) > 0)
            return false;

		args.Amount = 0;
		return false;
    }

    private static bool HandleDevotion(IHandleStatusTurnAutoStepArgs args) {
        if (args.Status != Instance.DevotionStatus || args.Amount == 0)
            return false;
        if (args.Timing != StatusTurnTriggerTiming.TurnStart)
			return false;

        args.Combat.Queue(new AStatus {
            status = ModEntry.Instance.LoversStatus,
            statusAmount = args.Amount,
            targetPlayer = true,
            statusPulse = args.Status
        });
		return false;
    }

    private static bool HandleSwitches(IHandleStatusTurnAutoStepArgs args) {
        if (args.Status != Instance.SwitchesStatus || args.Amount == 0)
			return false;
		if (args.Timing != StatusTurnTriggerTiming.TurnEnd)
			return false;
        if (args.Ship.Get(Status.timeStop) > 0)
            return false;

		if (args.Amount > 0)
			args.Amount = Math.Max(args.Amount - 1, 0);
		return false;
    }

    private static bool HandleTempReflex(IHandleStatusTurnAutoStepArgs args) {
        if (args.Status != Instance.TempReflexStatus || args.Amount == 0)
			return false;
		if (args.Timing != StatusTurnTriggerTiming.TurnEnd)
			return false;
        if (args.Ship.Get(Status.timeStop) > 0)
            return false;

		args.Amount = 0;
		return false;
    }

    private static bool HandleTempStrafe(IHandleStatusTurnAutoStepArgs args) {
        if (args.Status != Instance.TempStrafeStatus || args.Amount == 0)
			return false;
		if (args.Timing != StatusTurnTriggerTiming.TurnStart)
			return false;
        if (args.Ship.Get(Status.timeStop) > 0)
            return false;

		args.Amount = 0;
		return false;
    }

    private static bool HandleLeadOn(IHandleStatusTurnAutoStepArgs args) {
        if (args.Status != Instance.LeadOnStatus || args.Amount == 0)
			return false;
		if (args.Timing != StatusTurnTriggerTiming.TurnEnd)
			return false;
        if (args.Ship.Get(Status.timeStop) > 0)
            return false;

		args.Amount = 0;
		return false;
    }

    private static bool HandleOverdrive(IHandleStatusTurnAutoStepArgs args) {
        if (args.Status == Status.overdrive && args.Amount > 0 && !ShouldReduceOverdrive(args.State, args.Combat)) {
            return true;
        }
        return false;
    }

    private static bool ShouldReduceOverdrive(State s, Combat c) {
        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is IOverdriveReductionPreventerArtifact artifact && !artifact.ShouldReduceOverdrive(s, c))  {
                item.Pulse();
                return false;
            }
                
        }
        return true;
    }

    public bool HandleStatusTurnAutoStep(IHandleStatusTurnAutoStepArgs args)
	{
        return HandleKisses(args)
            || HandleLovers(args)
            || HandleDevotion(args)
            || HandleSwitches(args)
            || HandleTempReflex(args)
            || HandleTempStrafe(args)
            || HandleLeadOn(args)
            || HandleOverdrive(args);
	}

    public int ModifyStatusChange(IModifyStatusChangeArgs args) {
        if (args.OldAmount == 0 && args.NewAmount > 0 && args.Status == ModEntry.Instance.SwitchesStatus) {
            State s = args.State;
            Combat c = args.Combat;
            IModData modData = ModEntry.Instance.Helper.ModData;
            foreach (Card card in s.deck.Concat(c.hand).Concat(c.discard).Concat(c.exhausted)) {
                modData.RemoveModData(card, PairManager.CardGirlSwapKey);
            }
        }
        return args.NewAmount;
    }


    public IReadOnlyList<Tooltip> OverrideStatusTooltips(IOverrideStatusTooltipsArgs args) {
        if (args.Status == ModEntry.Instance.LoversStatus) return [
            .. args.Tooltips,
            .. PairManager.PairTrait.Configuration.Tooltips!(DB.fakeState, null)
        ];
        if (args.Status == ModEntry.Instance.SwitchesStatus) return [
            .. args.Tooltips,
            .. PairManager.PairTrait.Configuration.Tooltips!(DB.fakeState, null),
            .. PairManager.FloppableTrait.Configuration.Tooltips!(DB.fakeState, null)
        ];
        if (args.Status == ModEntry.Instance.DevotionStatus) return [
            .. args.Tooltips,
            .. StatusMeta.GetTooltips(ModEntry.Instance.LoversStatus, args.Amount)
        ];
        if (args.Status == ModEntry.Instance.KissesStatus) return [
            .. args.Tooltips,
            new TTCard {
                card = new Kiss()
            }
        ];
        if (args.Status == ModEntry.Instance.KissesAStatus) return [
            .. args.Tooltips,
            new TTCard {
                card = new Kiss {
                    upgrade = Upgrade.A
                }
            }
        ];
        return args.Tooltips;
    }
}