using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class WarmthOfThe4thMatch() : VioletFlameCard(4,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Owner.Creature.Powers.Any(p => p is WarmthOfThe4thMatchPower or WarmthOfThe4thMatchPlusPower))
            return;
        if (IsUpgraded)
            await PowerCmd.Apply<WarmthOfThe4thMatchPlusPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
        else
            await PowerCmd.Apply<WarmthOfThe4thMatchPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);

    }

    protected override void OnUpgrade()
    {

    }
}