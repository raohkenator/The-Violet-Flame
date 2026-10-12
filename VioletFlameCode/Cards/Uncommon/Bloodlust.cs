using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class Bloodlust() : VioletFlameCard(2,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [EnergyHoverTip];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<BloodlustPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}