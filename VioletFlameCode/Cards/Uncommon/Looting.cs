using System.Runtime.InteropServices.JavaScript;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.CustomEnums;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class Looting() : VioletFlameCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [VioletFlameTag.Fatal];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(8M, ValueProp.Move),
        new GoldVar(7)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        AttackCommand atkCmd = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play)
            .Targeting(play.Target);

        var result = await atkCmd.ExecuteAndCheckFatal(choiceContext);
        bool killed = result.Count > 0;

        await CardPileCmd.Draw(choiceContext, Owner);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner,
            new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1), null, this)).FirstOrDefault();
        if (card == null)
            return;
        await CardCmd.Discard(choiceContext, card);
        
        if (killed)
        {
            await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2M);
        DynamicVars.Gold.UpgradeValueBy(3M);
    }
}