using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.CustomEnums;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class LastResort() : VioletFlameCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    public override bool GainsBlock => true;
    
    protected override HashSet<CardTag> CanonicalTags => [VioletFlameTag.Fatal];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.Static(StaticHoverTip.Fatal)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4M, ValueProp.Move),
        new RepeatVar(3),
        new HealVar(5M)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        AttackCommand atkCmd = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .FromCard(this, play)
            .Targeting(play.Target);

        var result = await atkCmd.ExecuteAndCheckFatal(choiceContext);
        bool killed = result.Count > 0;

        if (killed)
        {
            if (Owner.Creature.CurrentHp == 1)
                await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue * 2);
            else
                await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        }
        
        await PowerCmd.Apply<NoBlockPower>(choiceContext, Owner.Creature, 9M, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1M);
        DynamicVars.Heal.UpgradeValueBy(2M);
    }
}