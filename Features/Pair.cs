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

    private static readonly string GlobalGirlKey = "GlobalGirl";
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
					TitleColor = Colors.action,
					Title = ModEntry.Instance.Localizations.Localize(["trait", "pair", "name"]),
					Description = ModEntry.Instance.Localizations.Localize(["trait", "pair", "description", 
                        s.route is Combat c && c != DB.fakeCombat && !BothActive(s) ? GetGirlGlobal(s) switch {
                            Girl.CAT => "top",
                            Girl.AMY => "bottom",
                            _ => throw new NotImplementedException(),
                        } : "both"]),
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
        return IsSwitchesOn(s) && IsGirlSwapped(card) ? GetOppositeGirl(girl) : girl;
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

    private static void FlopToWhereNeeded(G g, Card card, bool to) {
        card.flipped = to;
        card.flipAnim = 0;
        card.flopAnim = card.flipped ? 1.0 : (-1.0);
        card.OnFlip(g);
    }
}