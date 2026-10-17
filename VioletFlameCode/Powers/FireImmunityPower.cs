using BaseLib.Cards.Variables;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Powers;

public class FireImmunityPower() : VioletFlamePower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Burn>()];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ("BurnBase", 0),
        new ("BurnExtra", 3),
        new CustomCalculatedVar("Burn").WithMultiplier((p, c) => p.Amount),
        new ("BlockBase", 0),
        new ("BlockExtra", 4),
        new CustomCalculatedVar("Block").WithMultiplier((p, c) => p.Amount)
    ];

    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player shuffler)
    {
        if (shuffler != Owner.Player)
            return;
        int burnCount = (int)((CustomCalculatedVar)DynamicVars["Burn"]).CalculateCustom(Owner);
        await CardPileCmd.AddToCombatAndPreview<Burn>(Owner, PileType.Draw, burnCount, null, CardPilePosition.Random);
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner != Owner.Player || card is not Burn)
            return;
        await CardCmd.Exhaust(choiceContext, card);
        await CardPileCmd.Draw(choiceContext, Owner.Player);
        await CreatureCmd.GainBlock(Owner, ((CustomCalculatedVar)DynamicVars["Block"]).CalculateCustom(Owner), ValueProp.Unpowered, null);
    }
}