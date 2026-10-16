using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using VioletFlame.VioletFlameCode.Cards;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class DivineSmite() : VioletFlameCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DamageThreshold", 30M)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // Thank You Lamali292 for the Watcher Judgement code <3
        if (play.Target == null)
            return;
        
        var target = play.Target;
        if (target.CurrentHp <= DynamicVars["DamageThreshold"].IntValue)
        {
            target.SetCurrentHpInternal(0M);
            await CreatureCmd.Kill(target);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DamageThreshold"].UpgradeValueBy(10M);
    }
}