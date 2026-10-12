using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using VioletFlame.VioletFlameCode.Cards;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class Overexert() : VioletFlameCard(2,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<FrailPower>()
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Debuff", 2)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<FreeAttackPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
        await PowerCmd.Apply<FreeSkillPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, Owner.Creature, DynamicVars["Debuff"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(choiceContext, Owner.Creature, DynamicVars["Debuff"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<FrailPower>(choiceContext, Owner.Creature, DynamicVars["Debuff"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Debuff"].UpgradeValueBy(-1M);
    }
}