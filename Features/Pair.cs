using System;
using System.Linq;
using Nickel;
using System.Reflection;
using Nanoray.PluginManager;
using HarmonyLib;
using System.Collections.Generic;
using TheJazMaster.CatAndAmy.Artifacts;
using TheJazMaster.CatAndAmy.Actions;
using Microsoft.Extensions.Logging;

namespace TheJazMaster.CatAndAmy.Features;

[HarmonyPatch]
public static class PairManager
{
    private static ModEntry Instance => ModEntry.Instance;
    private static IModCards CardsHelper => Instance.Helper.Content.Cards;
    private static IModData ModData => Instance.Helper.ModData;

    internal static readonly string GlobalGirlKey = "GlobalGirl";
    internal static readonly string CardGirlSwapKey = "GirlSwap";

    internal static ICardTraitEntry PairTrait { get; private set; } = null!;
    internal static ICardTraitEntry FloppableTrait { get; private set; } = null!;


    public enum Girl {
        CAT, AMY, BOTH
    }

    internal static void Initialize(IPluginPackage<IModManifest> package, IModHelper helper) {
        PairTrait = CardsHelper.RegisterTrait("Pair", new() {
            Icon = (s, _) => s.route is Combat c && c != DB.fakeCombat && !BothActive(s) ? GetGirlGlobal(s) switch {
				Girl.AMY => ModEntry.Instance.PairTopIcon,
				Girl.CAT => ModEntry.Instance.PairBottomIcon,
				_ => throw new NotImplementedException(),
			} : ModEntry.Instance.PairIcon,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["trait", "pair"]).Localize,
            Tooltips = (s, _) => [
                new GlossaryTooltip($"trait.{MethodBase.GetCurrentMethod()!.DeclaringType!.Namespace!}::Pair")
				{
					Icon = Instance.PairIcon,
					TitleColor = Colors.cardtrait,
					Title = ModEntry.Instance.Localizations.Localize(["trait", "pair", "name"]),
					Description = ModEntry.Instance.Localizations.Localize(["trait", "pair", "description", 
                        s.route is Combat c && c != DB.fakeCombat ? GetGirlGlobal(s) switch {
                            Girl.CAT => "bottom",
                            Girl.AMY => "top",
                            _ => throw new NotImplementedException(),
                        } : "basic"]),
				}
            ]
        });

        FloppableTrait = CardsHelper.RegisterTrait("Floppable", new() {
            Icon = (s, card) => (card != null && GetGirl(s, card) == Girl.CAT) ? ModEntry.Instance.FloppableFlippedIcon : StableSpr.icons_floppable,
            Name = (_) => Loc.T("cardtrait.floppable.name"),
            Tooltips = (s, _) => [
                new TTGlossary("cardtrait.floppable")
            ]
        });

        ModEntry.Instance.Helper.Events.RegisterAfterArtifactsHook(nameof(Artifact.OnPlayerPlayCard), (State state, Combat combat, Card card) =>
        {
            ModData.SetModData(state.storyVars, GlobalGirlKey, GetGirlGlobal(state));

            if (CardsHelper.IsCardTraitActive(state, card, PairTrait)) {
                combat.Queue(new AFlipGirlies());
                if (state.ship.Get(ModEntry.Instance.LoversStatus) > 0) {
                    state.ship.PulseStatus(ModEntry.Instance.LoversStatus);
                    combat.Queue(new AStatus {
                        status = ModEntry.Instance.LoversStatus,
                        statusAmount = -1,
                        targetPlayer = true
                    });
                }
            }
        });
    }

    public static bool BothActive(State s) => s.ship.Get(ModEntry.Instance.LoversStatus) > 0;

    public static void GlobalFlip(State s, Combat c) {
        if (IsLoversOn(s)) return;

        ModData.SetModData(c, GlobalGirlKey, ModData.GetModDataOrDefault(c, GlobalGirlKey, Girl.AMY) == Girl.AMY ? Girl.CAT : Girl.AMY);

        foreach (Artifact item in s.EnumerateAllArtifacts()) {
            if (item is IOnSwitchArtifact artifact) {
                artifact.OnSwitch(s, c);
            }
        }
    }

    public static bool IsPair(Card card, State s) => CardsHelper.IsCardTraitActive(s, card, PairTrait);

    public static int CountPairsInHand(State s, Combat c) => c.hand.Count(card => IsPair(card, s));

    public static bool IsLoversOn(State s) => s.ship.Get(ModEntry.Instance.LoversStatus) > 0;

    public static bool IsSwitchesOn(State s) => s.ship.Get(ModEntry.Instance.SwitchesStatus) > 0;

    public static bool IsGirlSwapped(Card card) => ModData.GetModDataOrDefault(card, CardGirlSwapKey, false);

    public static Girl GetGirl(State s, Card card) {
        if (MG.inst.g.metaRoute != null) return Girl.AMY;
        return IsLoversOn(s) ? Girl.BOTH : GetGirlNotBoth(s, card);
    }

    public static Girl GetGirlNotBoth(State s, Card card) {
        Girl girl = GetGirlGlobal(s);
        return (IsSwitchesOn(s) || s == DB.fakeState) && IsGirlSwapped(card) ? GetOppositeGirl(girl) : girl;
    }

    public static Girl GetGirlGlobal(State s) {
        return ModData.GetModDataOrDefault(s.route is Combat c ? c : DB.fakeCombat, GlobalGirlKey, Girl.AMY);
    }

    public static Girl GetOppositeGirl(Girl girl) => girl switch
    {
        Girl.AMY => Girl.CAT,
        Girl.CAT => Girl.AMY,
        _ => girl
    };

    public static List<CardAction> MakeSet(State s, Card card, List<CardAction> first, List<CardAction> second) {
        Girl girl = GetGirl(s, card);
        foreach (CardAction c in first) {
            c.disabled = girl == Girl.CAT;
        }
        foreach (CardAction c in second) {
            c.disabled = girl == Girl.AMY;
        }
        if (first.Count + second.Count < 5) {
            return [.. first, new ADummyAction(), .. second];
        } else {
            return [.. first, .. second];
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Combat), nameof(Combat.FlipCardInHand))]
    private static void Combat_FlipCardInHand_Prefix(Card card, ref bool __state) {
        __state = card.flipped;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Combat), nameof(Combat.FlipCardInHand))]
    private static void Combat_FlipCardInHand_Postfix(G g, Card card, Combat __instance, bool __state) {
        if (IsSwitchesOn(g.state) && !IsLoversOn(g.state) && IsPair(card, g.state)) {
            bool hasBeenFlipped = __state != card.flipped;
            if (hasBeenFlipped) {
                if (__state && !IsGirlSwapped(card)) {
                    FlopToWhereNeeded(g, card, true);
                    ModData.SetModData(card, CardGirlSwapKey, true);
                }
                else if (!__state && IsGirlSwapped(card)) {
                    FlopToWhereNeeded(g, card, false);
                    ModData.SetModData(card, CardGirlSwapKey, false);
                }
            }
            else {
                FlopToWhereNeeded(g, card, card.flipped);
                ModData.SetModData(card, CardGirlSwapKey, !IsGirlSwapped(card));
            }
        }
    }

    internal static void FlopToWhereNeeded(G g, Card card, bool to) {
        card.flipped = to;
        card.flipAnim = 0;
        card.flopAnim = to ? 1 : -1;
        card.OnFlip(g);
    }



    internal static readonly string JustReturnedFromMissingKey = "JustReturnedFromMissing";

    [HarmonyPostfix]
    [HarmonyPatch(typeof(StoryNode), nameof(StoryNode.Filter))]
    private static void StoryNode_Filter_Prefix(StoryNode n, State s, StorySearch ctx, ref bool __result) {
        if (!__result) return;
        
        {
            if (ModData.TryGetModData(n, GlobalGirlKey, out Girl data) && ModData.TryGetModData(s.storyVars, GlobalGirlKey, out Girl value) && data != value)
            {
                __result = false;
                return;
            }
        }
        {
            if (ModData.TryGetModData(n, GlobalGirlKey, out bool data) && ModData.TryGetModData(s.storyVars, JustReturnedFromMissingKey, out bool value) && data != value)
            {
                __result = false;
                return;
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(StoryVars), nameof(StoryVars.ResetAfterCombatLine))]
	private static void StoryVars_ResetAfterCombatLine_Postfix(StoryVars __instance) {
		ModData.RemoveModData(__instance, JustReturnedFromMissingKey);
        ModData.RemoveModData(__instance, GlobalGirlKey);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AStatus), nameof(AStatus.Begin))]
    private static void AStatus_Begin_Prefix(AStatus __instance, State s, ref int __state)
		=> __state = __instance.targetPlayer ? s.ship.Get(__instance.status) : 0;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(AStatus), nameof(AStatus.Begin))]
    private static void AStatus_Begin_Postfix(AStatus __instance, State s, Combat c, ref int __state)
    {
        if (!__instance.targetPlayer)
            return;

        if (__instance.status == ModEntry.Instance.CatAndAmyCharacter.MissingStatus.Status && __state > 0 && s.ship.Get(ModEntry.Instance.CatAndAmyCharacter.MissingStatus.Status) <= 0)
            s.storyVars.ApplyModData(JustReturnedFromMissingKey, true);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CardReward), nameof(CardReward.Render))]
    private static void CardReward_Render_Prefix(CardReward __instance, G g)
    {
        
		if (__instance.flipFloppableCardsTimer > 3.0)
		{
			foreach (Card card in __instance.cards)
			{
				if (IsPair(card, DB.fakeState) && !card.isForeground)
				{
					card.flipAnim = 1.0;
                    ModData.SetModData(card, CardGirlSwapKey, !IsGirlSwapped(card));
				}
			}
		}
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CardReward), nameof(CardReward.TakeCard))]
    private static void CardReward_TakeCard_Postfix(CardReward __instance, G g, Card card)
    {
        ModData.RemoveModData(card, CardGirlSwapKey);
    }


}