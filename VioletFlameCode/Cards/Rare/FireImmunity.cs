using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class FireImmunity() : VioletFlameCard(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Burn>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<FireImmunityPower>(1M),
        new ("Burn", 3M),
        new ("Block", 4M)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<FireImmunityPower>(choiceContext, Owner.Creature, DynamicVars["FireImmunityPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["FireImmunityPower"].UpgradeValueBy(1M);
        DynamicVars["Burn"].UpgradeValueBy(3M);
        DynamicVars["Block"].UpgradeValueBy(4M);
    }
}