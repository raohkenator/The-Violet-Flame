using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class Hellfire() : VioletFlameCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(14M),
        new ExtraDamageVar(3M),
        //  new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => (card.Owner.Creature.GetPowerAmount<StrengthPower>().LessThan(0) ? -(card.Owner.Creature.GetPowerAmount<StrengthPower>())  : 0))
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => -Math.Min(0, card.Owner.Creature.GetPowerAmount<StrengthPower>()))
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        await DamageCmd.Attack(DynamicVars.CalculatedDamage.Calculate(Owner.Creature) - Math.Min(0, Owner.Creature.GetPowerAmount<StrengthPower>()))
            .FromCard(this, play)
            .Targeting(play.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(2M);
    }
}