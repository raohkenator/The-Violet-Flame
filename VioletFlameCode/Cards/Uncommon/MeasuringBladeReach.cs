using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.Interface;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class MeasuringBladeReach() : VioletFlameCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy), IStrengthNull
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0M),
        new ExtraDamageVar(6M),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((c, _) => Math.Abs(c.Owner.Creature.GetPowerAmount<StrengthPower>()))
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var calcDmg = Math.Abs(DynamicVars.CalculatedDamage.Calculate(null));
        await PowerCmd.Remove<StrengthPower>(Owner.Creature);
        await DamageCmd.Attack(calcDmg)
            .FromCard(this, play)
            .Targeting(play.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(3M);
    }

   /* public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power is not StrengthPower)
            return Task.CompletedTask;
        DynamicVars.CalculationBase.BaseValue -= amount;
        DynamicVars.CalculationBase.UpdateCardPreview(this, CardPreviewMode.Normal, null, true);
        DynamicVars.CalculatedDamage.UpdateCardPreview(this, CardPreviewMode.Normal, null, true);
        return Task.CompletedTask;
    }
    */
}