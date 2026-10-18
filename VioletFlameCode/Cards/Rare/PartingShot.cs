using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class PartingShot() : VioletFlameCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new ExtraDamageVar(1M),
        new CalculatedDamageVar(ValueProp.Unblockable).WithMultiplier((card, _) => card.Owner.Creature.MaxHp - card.Owner.Creature.CurrentHp)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var creature = Owner.Creature;
        await DamageCmd.Attack(DynamicVars.CalculatedDamage.Calculate(null))
            .FromCard(this, play)
            .Targeting(play.Target)
            .Execute(choiceContext);
        if (!IsUpgraded)
            await CreatureCmd.Kill(Owner.Creature);
        
        await CreatureCmd.Damage(choiceContext, Owner.Creature, creature.CurrentHp - 1M, ValueProp.Unpowered, this, play);

    }

    protected override void OnUpgrade()
    {

    }
}