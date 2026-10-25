using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace VioletFlame.VioletFlameCode.Cards.Common;

public class HeatMetal() : VioletFlameCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<FrailPower>(),
        HoverTipFactory.FromPower<DemisePower>(),
        HoverTipFactory.FromPower<ThornsPower>()
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<FrailPower>(3M),
        new PowerVar<DemisePower>(5M)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null) return;
        await PowerCmd.Apply<FrailPower>(choiceContext, play.Target, DynamicVars["FrailPower"].BaseValue, this.Owner.Creature, this);
        await PowerCmd.Apply<DemisePower>(choiceContext, play.Target, DynamicVars["DemisePower"].BaseValue, this.Owner.Creature, this);
        await PowerCmd.Apply<ThornsPower>(choiceContext, play.Target, 1M, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["FrailPower"].UpgradeValueBy(2M);
        this.DynamicVars["DemisePower"].UpgradeValueBy(2M);
    }
}