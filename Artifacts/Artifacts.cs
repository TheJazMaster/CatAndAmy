using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.CatAndAmy.Actions;
using TheJazMaster.CatAndAmy.Cards;
using TheJazMaster.CatAndAmy.Features;
namespace TheJazMaster.CatAndAmy.Artifacts;

internal sealed class OverwhelmingAttraction : Artifact, IRegisterableArtifact, IOnSwitchArtifact
{
	public int counter = 0;

	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [ArtifactPool.Common], helper, package, out _);
	}


	public override List<Tooltip>? GetExtraTooltips() => [
		.. PairManager.PairTrait.Configuration.Tooltips!(DB.fakeState, null), new TTCard {
			card = new Kiss()
		}
	];

	public void OnSwitch(State s, Combat c)
	{
		counter++;
		if (counter == 4) {
			counter = 0;
			c.Queue(new AAddCard {
				card = new Kiss {
					exhaustOverride = true
				},
				destination = CardDestination.Hand,
				artifactPulse = Key()
			});
		}
	}

	public override int? GetDisplayNumber(State s)
	{
		return counter;
	}
}

internal sealed class HeartShapedCookies : Artifact, IRegisterableArtifact
{
	public int counter = 0;

	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [ArtifactPool.Common], helper, package, out _);
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		new TTCard {
			card = new Kiss()
		}, .. StatusMeta.GetTooltips(Status.overdrive, 1)
	];

    public override void OnReceiveArtifact(State state)
    {
        
		(state.GetDialogue()?.actionQueue ?? state.GetCurrentQueue()).Queue(new AAddCard {
			card = new Kiss {
				temporaryOverride = false
			},
			amount = 2,
			destination = CardDestination.Deck,
			callItTheDeckNotTheDrawPile = true
		});
    }

	public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
	{
		if (card is Kiss) {
			counter++;
			if (counter == 2) {
				combat.Queue(new AStatus {
					status = Status.overdrive,
					statusAmount = 1,
					targetPlayer = true,
					artifactPulse = Key()
				});
			}
		}
	}

	public override void OnTurnEnd(State state, Combat combat)
	{
		counter = 0;
	}

	public override void OnCombatStart(State state, Combat combat)
	{
		counter = 0;
	}

	public override int? GetDisplayNumber(State s)
	{
		if (s.route is Combat)
			return counter;
		return null;
	}
}


internal sealed class WorkoutRoutine : Artifact, IRegisterableArtifact, IOnSwitchArtifact
{
	public int counter = 0;

	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [ArtifactPool.Common], helper, package, out _);
	}


	public override List<Tooltip>? GetExtraTooltips() => [
		.. PairManager.PairTrait.Configuration.Tooltips!(DB.fakeState, null),
		.. StatusMeta.GetTooltips(Status.evade, 1)
	];
		
	public void OnSwitch(State s, Combat c)
	{
		counter++;
		if (counter == 3) {
			c.Queue(new AStatus {
				status = Status.evade,
				statusAmount = 1,
				targetPlayer = true,
				artifactPulse = Key()
			});
		}
	}

	public override void OnTurnStart(State state, Combat combat)
	{
		counter = 0;
	}

	public override int? GetDisplayNumber(State s)
	{
		if (s.route is Combat && counter < 3)
			return counter;
		return null;
	}
}


internal sealed class SentimentalLocket : Artifact, IRegisterableArtifact
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [ArtifactPool.Common], helper, package, out _);
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		new TTCard {
			card = new StarCrossedLovers()
		}
	];

	public override void OnTurnStart(State state, Combat combat)
	{
		if (combat.turn == 1) {
			combat.Queue(new AAddCard {
				card = new StarCrossedLovers(),
				destination = CardDestination.Hand,
				artifactPulse = Key()
			});
		}
	}
}


internal sealed class LoveLetters : Artifact, IRegisterableArtifact, IOnSwitchArtifact
{
	public int counter = 0;

	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [ArtifactPool.Common], helper, package, out _);
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		.. PairManager.PairTrait.Configuration.Tooltips!(DB.fakeState, null)
	];

	public void OnSwitch(State s, Combat c)
	{
		counter++;
		if (counter == 4) {
			counter = 0;
			c.Queue(new AEnergy {
				changeAmount = 1,
				artifactPulse = Key()
			});
		}
	}

	public override int? GetDisplayNumber(State s)
	{
		return counter;
	}
}


[HarmonyPatch]
internal sealed class PowerCouple : Artifact, IRegisterableArtifact
{

	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [ArtifactPool.Boss], helper, package, out _);
	}

	public override int ModifyBaseDamage(int baseDamage, Card? card, State state, Combat? combat, bool fromPlayer)
	{
		if (!fromPlayer) return 0;

        int total = 0;
		if (card == null || card.GetMeta().deck != ModEntry.Instance.CatAndAmyDeck.Deck) total--;

		if (PairManager.GetGirlGlobal(state) != PairManager.Girl.CAT) total++;

		return total;
	}

	public override List<Tooltip>? GetExtraTooltips()
	{
		return new AAttack {
			damage = 1,
			moveEnemy = -1 
		}.GetTooltips(DB.fakeState);
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(Card), nameof(Card.GetActionsOverridden))]
	private static void Card_GetActionsOverriden_Postfix(State s, Combat c, Card __instance, List<CardAction> __result) {
		if (PairManager.GetGirlGlobal(s) != PairManager.Girl.AMY && s.EnumerateAllArtifacts().OfType<PowerCouple>().FirstOrDefault() != null) {
			foreach (CardAction action in __result) {
				if (action is AAttack attack) {
					if (attack.moveEnemy <= 0) attack.moveEnemy--;
					else attack.moveEnemy++;
				}
			}
		}
	}
}