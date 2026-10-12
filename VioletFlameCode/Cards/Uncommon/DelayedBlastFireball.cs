using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class DelayedBlastFireball() : VioletFlameCard(2,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Turns", 2M),
        new DynamicVar("BombDamage", 28M)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        (await PowerCmd.Apply<DelayedBlastFireballPower>(choiceContext, Owner.Creature, DynamicVars["Turns"].BaseValue, Owner.Creature, this)).SetDamage(DynamicVars["BombDamage"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BombDamage"].UpgradeValueBy(7M);
    }
}