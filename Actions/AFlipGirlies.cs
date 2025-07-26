// AMove
using System;
using System.Collections.Generic;
using System.Linq;
using FSPRO;
using TheJazMaster.CatAndAmy.Features;

namespace TheJazMaster.CatAndAmy.Actions;

public class AFlipGirlies : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        timer = 0.5;
		PairManager.GlobalFlip(g.state, g.state.route as Combat ?? DB.fakeCombat);
        Audio.Play(Event.Status_ShieldUp);

        if (s.ship.Get(ModEntry.Instance.SwitchesStatus) > 0) return;
        bool flipped = PairManager.GetGirlGlobal(s) == PairManager.Girl.CAT;
        foreach (Card card in c.hand) {
			if (card.flipped != flipped)
            	c.FlipCardInHand(g, card);
        }
		foreach (Card card in s.deck.Concat(c.discard).Concat(c.exhausted)) {
            card.flipped = flipped;
        }
    }
}