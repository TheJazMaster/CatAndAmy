using Nickel;
using TheJazMaster.CatAndAmy.Actions;
using System.Collections.Generic;
using System.Reflection;
using static TheJazMaster.CatAndAmy.Features.PairManager;
using Nanoray.PluginManager;
using Shockah.Kokoro;

namespace TheJazMaster.CatAndAmy.Cards;


internal sealed class Kiss : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = 0,
		exhaust = true,
		temporary = true,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch
	{
		Upgrade.A => MakeSet(s, this, [
			new AEnergy {
				changeAmount = 1
			},
			new AStatus {
				status = Status.energyNextTurn,
				statusAmount = 1,
				targetPlayer = true
			}
		], [
			new ADrawCard {
				count = 1
			},
			new AStatus {
				status = Status.drawNextTurn,
				statusAmount = 1,
				targetPlayer = true
			}
		]),
		_ => MakeSet(s, this, [
			new AEnergy {
				changeAmount = upgrade == Upgrade.B ? 2 : 1
			},
		], [
			new ADrawCard {
				count = upgrade == Upgrade.B ? 2 : 1
			}
		]),
	};
}

internal sealed class StarCrossedLovers : Card, IRegisterableCard
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, null, true);
	}

	public override CardData GetData(State state) => new() {
		cost = 0,
		exhaust = upgrade != Upgrade.A,
		temporary = true,
		retain = upgrade != Upgrade.B,
		artTint = "ffffff"
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AStatus {
			status = ModEntry.Instance.LoversStatus,
			statusAmount = upgrade == Upgrade.B ? 2 : 1,
			targetPlayer = true
		}
	];
}

internal sealed class Grapple : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 2 : 1,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AMove {
			dir = 2
		},
		new AAttack {
			damage = GetDmg(s, upgrade switch {
				Upgrade.A => 3,
				Upgrade.B => 4,
				_ => 2
			})
		},
	], [
		new AStatus {
			status = Status.tempShield,
			statusAmount = upgrade == Upgrade.None ? 1 : 2,
			targetPlayer = true
		},
		new AStatus {
			status = Status.evade,
			statusAmount = upgrade == Upgrade.B ? 2 : 1,
			targetPlayer = true
		}
	]);
}

internal sealed class Toss : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = 1,
		flippable = upgrade == Upgrade.A,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 1),
			moveEnemy = -2
		},
	], [
		new AMove {
			dir = -2,
			targetPlayer = true
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 1)
		},
	]);
}

internal sealed class DoubleStrike : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = 1,
		flippable = upgrade == Upgrade.A,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.A => MakeSet(s, this, [
			new AAttack {
				damage = GetDmg(s, 1),
				moveEnemy = 1
			},
			new AAttack {
				damage = GetDmg(s, 1),
				moveEnemy = -3
			},
		], [
			new AAttack {
				damage = GetDmg(s, 1)
			},
			new AAttack {
				damage = GetDmg(s, 1)
			},
			new AMove {
				dir = 2,
				targetPlayer = true
			},
		]),
		_ => MakeSet(s, this, [
			new AAttack {
				damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 1),
				moveEnemy = 1
			},
			new AAttack {
				damage = GetDmg(s, 1),
				moveEnemy = -1
			},
		], [
			new AAttack {
				damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 1)
			},
			new AMove {
				isRandom = true,
				dir = 1,
				targetPlayer = true
			},
			new AAttack {
				damage = GetDmg(s, 1)
			},
		])
	};
}

internal sealed class Backhand : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 0 : 1,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AAttack {
			damage = GetDmg(s, 1),
			moveEnemy = -1
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 1 : 0),
			stunEnemy = true
		},
	], [
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 1)
		},
		new AStatus {
			status = Status.evade,
			statusAmount = 1,
			targetPlayer = true
		},
	]);
}

internal sealed class LeadOn : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = 0,
		exhaust = upgrade == Upgrade.B,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AAttack {
			damage = GetDmg(s, upgrade switch {
				Upgrade.A => 2,
				Upgrade.B => 3,
				_ => 1
			})
		},
		new AStatus {
			status = ModEntry.Instance.LeadOnStatus,
			statusAmount = 1
		},
	], [
		new AStatus {
			status = ModEntry.Instance.LeadOnStatus,
			statusAmount = upgrade == Upgrade.None ? 1 : 2
		},
		new AStatus {
			status = Status.evade,
			statusAmount = upgrade == Upgrade.B ? 2 : 1,
			targetPlayer = true
		},
	]);
}

internal sealed class Dropkick : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = 2,
		flippable = upgrade == Upgrade.A,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 3 : 2),
			moveEnemy = upgrade == Upgrade.A ? 2 : 1
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 3 : 2)
		},
	], [
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 3 : 2)
		},
		new AMove {
			dir = upgrade == Upgrade.A ? 2 : 1,
			isRandom = upgrade != Upgrade.A,
			targetPlayer = true
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 3 : 2)
		},
	]);
}

internal sealed class Cruise : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = 0,
		flippable = upgrade == Upgrade.B,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.B => MakeSet(s, this, [
			new AMove {
				dir = 2
			},
			new AMove {
				dir = -1
			},
		], [
			new AMove {
				dir = 2,
				targetPlayer = true
			},
			new AMove {
				dir = -1,
				targetPlayer = true
			},
		]),
		_ => MakeSet(s, this, [
			new AMove {
				dir = upgrade == Upgrade.A ? 3 : 2
			},
		], [
			new AMove {
				dir = upgrade == Upgrade.A ? 3 : 2,
				targetPlayer = true
			},
		])
	};
}

internal sealed class BackBreaker : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 1 : 2,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 3),
			stunEnemy = upgrade == Upgrade.A
		},
		new AMove {
			dir = 1
		},
	], [
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.A ? 3 : 2)
		},
		new AMove {
			isRandom = true,
			dir = upgrade == Upgrade.A ? 3 : upgrade == Upgrade.B ? 1 : 2,
			targetPlayer = true
		},
	]);
}

internal sealed class CheckIn : Card, IRegisterableCard
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.common, helper, package, out _);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 0 : 1,
		exhaust = upgrade != Upgrade.B,
		artTint = "ffffff",
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AAddCard {
			card = new Kiss(),
			amount = 2,
			destination = CardDestination.Hand
		}
	];
}



internal sealed class BaitAndSwitch : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.uncommon, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 2 : upgrade == Upgrade.A ? 0 : 2,
		exhaust = upgrade != Upgrade.None,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AStatus {
			status = ModEntry.Instance.TempReflexStatus,
			statusAmount = upgrade == Upgrade.B ? 2 : 1,
			targetPlayer = true
		},
		new AStatus {
			status = ModEntry.Instance.LeadOnStatus,
			statusAmount = 1
		},
	], [
		new AStatus {
			status = upgrade == Upgrade.B ? Status.autododgeLeft : Status.autododgeRight,
			statusAmount = 1,
			targetPlayer = true
		},
		new AStatus {
			status = Status.tempShield,
			statusAmount = 1,
			targetPlayer = true
		},
	]);
}

internal sealed class ButchAndFemme : Card, IRegisterableCard
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.uncommon, helper, package, out _);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 0 : 1,
		exhaust = upgrade != Upgrade.B,
		retain = true,
		artTint = "ffffff"
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AStatus {
			status = ModEntry.Instance.LoversStatus,
			statusAmount = 1,
			targetPlayer = true
		},
	];
}

internal sealed class Commitment : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.uncommon, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 2 : 1,
		exhaust = true,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.A => MakeSet(s, this, [
			new AStatus {
				status = Status.tableFlip,
				statusAmount = 1,
				targetPlayer = true
			}
		], [
			new AStatus {
				status = upgrade == Upgrade.B ? Status.powerdrive : Status.overdrive,
				statusAmount = upgrade == Upgrade.B ? 1 : 2,
				targetPlayer = true
			},
		]),
		_ => MakeSet(s, this, [
			new AStatus {
				status = Status.tableFlip,
				statusAmount = 1,
				targetPlayer = true
			},
			new AStatus {
				status = upgrade == Upgrade.B ? Status.shield : Status.energyLessNextTurn,
				statusAmount = 1,
				targetPlayer = true
			},
		], [
			new AStatus {
				status = upgrade == Upgrade.B ? Status.powerdrive : Status.overdrive,
				statusAmount = upgrade == Upgrade.B ? 1 : 2,
				targetPlayer = true
			},
			new AStatus {
				status = Status.energyLessNextTurn,
				statusAmount = 1,
				targetPlayer = true
			},
		])
	};
}

internal sealed class SmashAndGrab : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.uncommon, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 0 : 1,
		retain = upgrade == Upgrade.A,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AVariableHint {
			status = Status.shield
		},
		new AStatus {
			status = ModEntry.Instance.TempReflexStatus,
			statusAmount = s.ship.Get(Status.shield),
			xHint = 1,
			targetPlayer = true
		},
		new AStatus {
			status = Status.shield,
			statusAmount = upgrade == Upgrade.B ? 0 : 1,
			mode = AStatusMode.Set,
			targetPlayer = true
		}
	], [
		new AVariableHintPair(),
		new AAttack {
			damage = GetDmg(s, CountPairsInHand(s, c)),
			xHint = 1
		}
	]);
}

internal sealed class YouHaul : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.uncommon, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 1 : 2,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AAttack {
			damage = GetDmg(s, 1),
			moveEnemy = 1
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.A ? 2 : upgrade == Upgrade.B ? 0 : 1),
			moveEnemy = 1
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1),
			moveEnemy = 1
		}
	], [
		new AStatus {
			status = Status.autopilot,
			statusAmount = 2,
			targetPlayer = true
		},
		new AStatus {
			status = Status.evade,
			statusAmount = upgrade == Upgrade.A ? 3 : upgrade == Upgrade.B ? 1 : 2,
			targetPlayer = true
		}
	]);
}

internal sealed class Suplex : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.uncommon, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = 1,
		flippable = upgrade == Upgrade.A,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AMove {
			dir = upgrade == Upgrade.B ? 3 : -3,
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 4 : 3),
			stunEnemy = true
		}
	], [
		new AMove {
			dir = 2,
			isRandom = upgrade != Upgrade.A,
			targetPlayer = true
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.B ? 4 : 3),
			stunEnemy = true
		}
	]);
}

internal sealed class Powerlifting : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	private static IKokoroApi.IV2.IActionCostsApi ActionCosts => ModEntry.Instance.KokoroApi.ActionCosts;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.uncommon, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = 1,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.A => MakeSet(s, this, [
			new AStatus {
				status = Status.shield,
				statusAmount = 2,
				targetPlayer = true
			},
			new AStatus {
				status = Status.overdrive,
				statusAmount = 1,
				targetPlayer = true
			}
		], [
			new AStatus {
				status = Status.evade,
				statusAmount = 2,
				targetPlayer = true
			}
		]),
		Upgrade.B => MakeSet(s, this, [
			new AStatus {
				status = Status.tempShield,
				statusAmount = 1,
				targetPlayer = true
			},
			ActionCosts.MakeCostAction(ActionCosts.MakeResourceCost(
				ActionCosts.MakeStatusResource(Status.shield, true),
				3), new AStatus {
					status = Status.powerdrive,
					statusAmount = 1,
					targetPlayer = true
				}
			).AsCardAction
		], [
			ActionCosts.MakeCostAction(ActionCosts.MakeResourceCost(
				ActionCosts.MakeStatusResource(Status.shield, true),
				2), new AStatus {
					status = Status.evade,
					statusAmount = 4,
					targetPlayer = true
				}
			).AsCardAction
		]),
		_ => MakeSet(s, this, [
			new AStatus {
				status = Status.shield,
				statusAmount = 1,
				targetPlayer = true
			},
			new AStatus {
				status = Status.overdrive,
				statusAmount = 1,
				targetPlayer = true
			}
		], [
			ActionCosts.MakeCostAction(ActionCosts.MakeResourceCost(
				ActionCosts.MakeStatusResource(Status.shield, true),
				1), new AStatus {
					status = Status.evade,
					statusAmount = 2,
					targetPlayer = true
				}
			).AsCardAction
		])
	};
}

internal sealed class DoubleSwitch : Card, IRegisterableCard
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.rare, helper, package, out _);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 0 : 1,
		exhaust = upgrade != Upgrade.B,
		retain = true,
		artTint = "ffffff",
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AStatus {
			status = ModEntry.Instance.SwitchesStatus,
			statusAmount = 3,
			targetPlayer = true
		}
	];
}

internal sealed class Kisses : Card, IRegisterableCard
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.rare, helper, package, out _);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.B ? 2 : 3,
		exhaust = true,
		artTint = "ffffff",
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AStatus {
			status = upgrade == Upgrade.A ? ModEntry.Instance.KissesAStatus : ModEntry.Instance.KissesStatus,
			statusAmount = 1,
			targetPlayer = true
		}
	];
}

internal sealed class UndyingLove : Card, IRegisterableCard
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.rare, helper, package, out _);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 3 : 4,
		exhaust = true,
		artTint = "ffffff"
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.B => [
			new AStatus {
				status = ModEntry.Instance.DevotionStatus,
				statusAmount = 1,
				targetPlayer = true
			},
			new AStatus {
				status = Status.drawNextTurn,
				statusAmount = 3,
				targetPlayer = true
			}
		],
		_ => [
			new AStatus {
				status = ModEntry.Instance.DevotionStatus,
				statusAmount = 1,
				targetPlayer = true
			},
		]
	};
}

internal sealed class TrackStarPartyQueen : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.rare, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = 1,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => MakeSet(s, this, [
		new AStatus {
			status = ModEntry.Instance.TempReflexStatus,
			statusAmount = upgrade == Upgrade.B ? 2 : 1,
			targetPlayer = true
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.A ? 1 : 0),
			moveEnemy = -1
		},
		new AAttack {
			damage = GetDmg(s, upgrade == Upgrade.A ? 1 : 0),
			moveEnemy = 1
		}
	], [
		new AStatus {
			status = ModEntry.Instance.TempStrafeStatus,
			statusAmount = upgrade == Upgrade.B ? 2 : 1,
			targetPlayer = true
		},
		upgrade == Upgrade.A ? new AStatus {
			status = Status.evade,
			statusAmount = 1,
			targetPlayer = true
		} : new AMove {
			dir = 1,
			isRandom = true,
			targetPlayer = true
		}
	]);
}

internal sealed class BootBlack : Card, IRegisterableCard, IHasCustomCardTraits
{
	private static Spr Cat;
	private static Spr Amy;
	private static Spr Both;
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.rare, helper, package, out _, out Both, out Amy, out Cat);
	}

	public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state) => new HashSet<ICardTraitEntry> { PairTrait };

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 2 : 3,
		exhaust = true,
		artTint = "ffffff",
		art = GetGirl(state, this) switch {
			Girl.AMY => Amy,
			Girl.CAT => Cat,
			_ => Both
		}
	};

	public override List<CardAction> GetActions(State s, Combat c) => upgrade switch {
		Upgrade.B => MakeSet(s, this, [
			new AStatus {
				status = ModEntry.Instance.ReflexStatus,
				statusAmount = 1,
				targetPlayer = true
			},
			new AStatus {
				status = Status.shield,
				statusAmount = 2,
				targetPlayer = true
			},
		], [
			new AStatus {
				status = Status.ace,
				statusAmount = 1,
				targetPlayer = true
			},
			new AStatus {
				status = Status.evade,
				statusAmount = 2,
				targetPlayer = true
			},
		]),
		_ => MakeSet(s, this, [
			new AStatus {
				status = ModEntry.Instance.ReflexStatus,
				statusAmount = 1,
				targetPlayer = true
			}
		], [
			new AStatus {
				status = Status.ace,
				statusAmount = 1,
				targetPlayer = true
			}
		]),
	};
}

internal sealed class CatAndAmyExe : Card, IRegisterableCard
{
	public static void Register(IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Rarity.uncommon, helper, package, out _, Deck.colorless);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 0 : 1,
		exhaust = true,
		description = ColorlessLoc.GetDesc(state, upgrade == Upgrade.B ? 3 : 2, ModEntry.Instance.CatAndAmyDeck.Deck),
		artTint = "ffffff"
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new ACardOffering {
			amount = upgrade == Upgrade.B ? 3 : 2,
			limitDeck = ModEntry.Instance.CatAndAmyDeck.Deck,
			makeAllCardsTemporary = true,
			overrideUpgradeChances = false,
			canSkip = false,
			inCombat = true,
			discount = -1,
			dialogueSelector = ".summonCatAndAmy"
		}
	];
}