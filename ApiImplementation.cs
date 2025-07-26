using System.Collections.Generic;
using Nickel;
using TheJazMaster.CatAndAmy.Features;

#nullable enable
namespace TheJazMaster.CatAndAmy;

public sealed class ApiImplementation : ICatAndAmyApi
{
	ModEntry Instance = ModEntry.Instance;

	public int CountPairsInHand(State s, Combat c) => PairManager.CountPairsInHand(s, c);

	public Deck CatAndAmyDeck => ModEntry.Instance.CatAndAmyDeck.Deck;
	public Status LoversStatus => ModEntry.Instance.LoversStatus;
	public Status SwitchesStatus => ModEntry.Instance.SwitchesStatus;
	public Status KissesStatus => ModEntry.Instance.KissesStatus;
	public Status KissesAStatus => ModEntry.Instance.KissesAStatus;
	public Status DevotionStatus => ModEntry.Instance.DevotionStatus;
	public Status ReflexStatus => ModEntry.Instance.ReflexStatus;
	public Status TempReflexStatus => ModEntry.Instance.TempReflexStatus;
	public Status TempStrafeStatus => ModEntry.Instance.TempStrafeStatus;
	public Status LeadOnStatus => ModEntry.Instance.LeadOnStatus;

	public ICardTraitEntry PairTrait => PairManager.PairTrait;
	public ICardTraitEntry FloppablePairTrait => PairManager.FloppableTrait;
}
