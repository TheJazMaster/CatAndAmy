using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TheJazMaster.CatAndAmy.Features;

namespace TheJazMaster.CatAndAmy.Actions;

public class AVariableHintPair : AVariableHint
{   
    public int displayAdjustment = 0;

    public AVariableHintPair() : base() {
        hand = true;
    }

    public override Icon? GetIcon(State s) {
        return new Icon(ModEntry.Instance.PairHandIcon, null, Colors.textMain);
    }

	public override List<Tooltip> GetTooltips(State s)
	{
		List<Tooltip> list = [];
        string parentheses = "";
        if (s.route is Combat c)
        {
            var amt = PairManager.CountPairsInHand(s, c);
            DefaultInterpolatedStringHandler stringHandler = new(22, 1);
            stringHandler.AppendLiteral(" </c>(<c=keyword>");
            stringHandler.AppendFormatted(amt + displayAdjustment);
            stringHandler.AppendLiteral("</c>)");
            
            parentheses = stringHandler.ToStringAndClear();
        }
        list.Add(new TTText(ModEntry.Instance.Localizations.Localize(["action", "vairableHintPair", "description"], new { Amount = parentheses })));
        return list;
	}
}