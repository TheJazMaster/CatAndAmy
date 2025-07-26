using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.CatAndAmy.Features;

#nullable enable
namespace TheJazMaster.CatAndAmy.Artifacts;

internal sealed class LovesProtection : Artifact, IRegisterableArtifact, IOnSwitchArtifact
{
	public int counter;

	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi == null) return;

		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [], helper, package, out _, ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck);
		ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact<LovesProtection>([Deck.dizzy, ModEntry.Instance.CatAndAmyDeck.Deck]);
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		.. PairManager.PairTrait.Configuration.Tooltips!(DB.fakeState, null),
		.. StatusMeta.GetTooltips(Status.tempShield, 1)
	];

	public void OnSwitch(State s, Combat c)
	{
		counter++;
		if (counter == 2) {
			counter = 0;
			c.Queue(new AStatus {
				status = Status.tempShield,
				statusAmount = 1,
				targetPlayer = true,
				artifactPulse = Key()
			});
		}
	}
}


internal sealed class BeatEmUp : Artifact, IRegisterableArtifact
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi == null) return;

		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [], helper, package, out _, ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck);
		ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact<BeatEmUp>([Deck.riggs, ModEntry.Instance.CatAndAmyDeck.Deck]);
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		.. StatusMeta.GetTooltips(Status.strafe, 1),
		.. StatusMeta.GetTooltips(ModEntry.Instance.TempStrafeStatus, 1),
		.. StatusMeta.GetTooltips(ModEntry.Instance.ReflexStatus, 1),
		.. StatusMeta.GetTooltips(ModEntry.Instance.TempReflexStatus, 1),
	];
}


internal sealed class PerseveringPartnership : Artifact, IRegisterableArtifact, IOnSwitchArtifact, IOverdriveReductionPreventerArtifact
{
	public int counter;

	private static Spr spriteInactive;
	private static Spr spriteActive;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi == null) return;

		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [], helper, package, out _, out spriteActive, out spriteInactive, ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck);
		ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact<PerseveringPartnership>([Deck.peri, ModEntry.Instance.CatAndAmyDeck.Deck]);
	}

	public bool ShouldReduceOverdrive(State s, Combat c) {
		if (counter >= 3) {
			Pulse();
			return false;
		}
		return true;
	}

	public override int? GetDisplayNumber(State s)
	{
		if (s.route is Combat && counter < 3) return counter;
		return null;
	}

	public override void OnTurnStart(State state, Combat combat)
	{
		counter = 0;
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		.. PairManager.PairTrait.Configuration.Tooltips!(DB.fakeState, null),
		.. StatusMeta.GetTooltips(Status.overdrive, 1)
	];

	public void OnSwitch(State s, Combat c)
	{
		counter++;
	}

	public override Spr GetSprite() => counter >= 3 ? spriteActive : spriteInactive;
}


[HarmonyPatch]
internal sealed class LoveBomb : Artifact, IRegisterableArtifact, IOnRepairKitDestroyedArtifact
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi == null) return;

		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [], helper, package, out _, ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck);
		ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact<LoveBomb>([Deck.goat, ModEntry.Instance.CatAndAmyDeck.Deck]);
	}

	public override void OnReceiveArtifact(State state)
	{
		(state.GetDialogue()?.actionQueue ?? state.GetCurrentQueue()).Queue(new AAddCard {
			card = new RepairKitCard {
				upgrade = Upgrade.B
			},
			destination = CardDestination.Deck,
			callItTheDeckNotTheDrawPile = true
		});
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		new TTCard {
			card = new RepairKitCard {
				upgrade = Upgrade.B
			}
		},
		.. StatusMeta.GetTooltips(ModEntry.Instance.LoversStatus, 1)
	];

	public void OnRepairKitDestroyed(State s, Combat c, bool wasPlayer, int worldX)
	{
		if (wasPlayer)
			c.Queue(new AStatus {
				status = ModEntry.Instance.LoversStatus,
				statusAmount = 1,
				targetPlayer = true,
				artifactPulse = Key()
			});
	}

	[HarmonyPostfix]
	[HarmonyPatch(typeof(RepairKit), nameof(RepairKit.GetActionsOnDestroyed))]
	private static void RepairKit_GetActionsOnDestroyed_Postfix(State s, Combat c, bool wasPlayer, int worldX) {
		foreach (Artifact item in s.EnumerateAllArtifacts()) {
			if (item is IOnRepairKitDestroyedArtifact artifact)
				artifact.OnRepairKitDestroyed(s, c, wasPlayer, worldX);
		}
	}
}


internal sealed class FlamesOfPassion : Artifact, IRegisterableArtifact
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi == null) return;

		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [], helper, package, out _, ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck);
		ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact<FlamesOfPassion>([Deck.eunice, ModEntry.Instance.CatAndAmyDeck.Deck]);
	}

	public override void AfterPlayerOverheat(State state, Combat combat)
	{
		combat.Queue(new AStatus {
			status = ModEntry.Instance.LoversStatus,
			statusAmount = 1,
			targetPlayer = true,
			artifactPulse = Key()
		});
	}


	public override List<Tooltip>? GetExtraTooltips() => [
		.. StatusMeta.GetTooltips(Status.heat, 3),
		.. StatusMeta.GetTooltips(ModEntry.Instance.LoversStatus, 1)
	];
}


internal sealed class RainbowPride : Artifact, IRegisterableArtifact, IOnSwitchArtifact
{
	public int counter;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi == null) return;

		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [], helper, package, out _, ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck);
		ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact<RainbowPride>([Deck.hacker, ModEntry.Instance.CatAndAmyDeck.Deck]);
	}


	public override List<Tooltip>? GetExtraTooltips() => [
		.. PairManager.PairTrait.Configuration.Tooltips!(DB.fakeState, null),
		.. StatusMeta.GetTooltips(Status.boost, 1)
	];

	public void OnSwitch(State s, Combat c)
	{
		counter++;
		if (counter == 5) {
			counter = 0;
			c.Queue(new AStatus {
				status = Status.boost,
				statusAmount = 1,
				targetPlayer = true,
				artifactPulse = Key()
			});
		}
	}

	public override int? GetDisplayNumber(State s)
	{
		if (s.route is Combat)
			return counter;
		return null;
	}
}


internal sealed class CrystalGirlies : Artifact, IRegisterableArtifact
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi == null) return;

		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [], helper, package, out _, ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck);
		ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact<CrystalGirlies>([Deck.shard, ModEntry.Instance.CatAndAmyDeck.Deck]);
	}

	public override void OnTurnEnd(State state, Combat combat)
	{
		if (state.ship.Get(Status.shard) >= 2) {
			combat.Queue(new AStatus {
				status = ModEntry.Instance.LoversStatus,
				statusAmount = 1,
				shardcost = 2,
				targetPlayer = true,
				artifactPulse = Key()
			});
		}
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		.. StatusMeta.GetTooltips(Status.shard, 1),
		.. StatusMeta.GetTooltips(ModEntry.Instance.LoversStatus, 1),
	];
}


internal sealed class AmySandwich : Artifact, IRegisterableArtifact
{
	public int counter;
	public bool active;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package)
	{
		if (ModEntry.Instance.DuoArtifactsApi == null) return;

		IRegisterableArtifact.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, [], helper, package, out _, ModEntry.Instance.DuoArtifactsApi.DuoArtifactVanillaDeck);
		ModEntry.Instance.DuoArtifactsApi.RegisterDuoArtifact<AmySandwich>([Deck.catartifact, ModEntry.Instance.CatAndAmyDeck.Deck]);
	}

	public override void OnCombatStart(State state, Combat combat)
	{
		counter = 0;
		active = false;
	}

	public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
	{
		if (active) return;

		if (deck == Deck.colorless) counter++;
		if (counter == 10) {
			active = true;
			combat.Queue(new AStatus {
				status = ModEntry.Instance.ReflexStatus,
				statusAmount = 1,
				targetPlayer = true,
				artifactPulse = Key()
			});
		}
	}

	public override List<Tooltip>? GetExtraTooltips() => [
		.. StatusMeta.GetTooltips(ModEntry.Instance.ReflexStatus, 1),
	];

	public override int? GetDisplayNumber(State s)
	{
		if (s.route is Combat && !active)
			return counter;
		return null;
	}
}