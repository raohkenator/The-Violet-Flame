using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace VioletFlame.VioletFlameCode.Utils;

public static class FatalUtils
{
    /// <summary>Check whether a card is an attack with a Fatal condition.</summary>
    /// <param name="card">Card meant to be checked.</param>
    public static bool IsFatalCard(this CardModel card)
    {
        return card.HoverTips.Contains(HoverTipFactory.Static(StaticHoverTip.Fatal)) && card.Type == CardType.Attack;
    }
}