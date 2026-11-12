using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.Interface;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class AdjustPosture() : VioletFlameCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self), IDexterityNull
{
    public override bool GainsBlock => true;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<DexterityPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0M),
        new CalculationExtraVar(5M),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((c, _) => Math.Abs(c.Owner.Creature.GetPowerAmount<DexterityPower>()))
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var calcBlock = DynamicVars.CalculatedBlock.Calculate(null);
        await PowerCmd.Remove<DexterityPower>(Owner.Creature);
        await CreatureCmd.GainBlock(Owner.Creature, calcBlock, ValueProp.Move, play);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationExtra.UpgradeValueBy(3);
    }
}