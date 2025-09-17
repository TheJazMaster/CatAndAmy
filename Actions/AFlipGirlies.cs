// AMove
using System;
using System.Collections.Generic;
using System.Linq;
using FSPRO;
using Nickel;
using TheJazMaster.CatAndAmy.Features;

namespace TheJazMaster.CatAndAmy.Actions;

public class AFlipGirlies : CardAction
{
    internal static IModCards CardsHelper => ModEntry.Instance.Helper.Content.Cards;
    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.5;
		PairManager.GlobalFlip(s, c);
        Audio.Play(Event.Status_ShieldUp);

        if (PairManager.IsSwitchesOn(s)) return;

        bool isCat = PairManager.GetGirlGlobal(s) == PairManager.Girl.CAT;
        foreach (Card card in c.hand) {
			if (CardsHelper.IsCardTraitActive(s, card, PairManager.PairTrait)) {
                card.flopAnim = isCat ? 1 : -1;
            }
        }
    }
}