using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class DesperateMeasure() : VioletFlameCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DesperateMeasurePower>(3M)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<DesperateMeasurePower>(choiceContext, Owner.Creature, DynamicVars["DesperateMeasurePower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DesperateMeasurePower"].UpgradeValueBy(1M);
    }
}