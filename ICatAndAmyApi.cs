using System.Collections.Generic;
using Nickel;

#nullable enable
namespace TheJazMaster.CatAndAmy;

public interface ICatAndAmyApi
{
	int CountPairsInHand(State s, Combat c);

	Deck CatAndAmyDeck { get; }
	Status LoversStatus { get; }
	Status SwitchesStatus { get; }
	Status KissesStatus { get; }
	Status KissesAStatus { get; }
	Status DevotionStatus { get; }
	Status ReflexStatus { get; }
	Status TempReflexStatus { get; }
	Status TempStrafeStatus { get; }
	Status LeadOnStatus { get; }

	ICardTraitEntry PairTrait { get; }
	ICardTraitEntry FloppablePairTrait { get; }
}
