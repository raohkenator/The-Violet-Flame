using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.CustomEnums;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class VolcanicAsh() : VioletFlameCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [VioletFlameTag.Fatal];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10M, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        var atkCmd = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play)
            .Targeting(play.Target);
        var result = await atkCmd.ExecuteAndCheckFatal(choiceContext);
        bool fatal = result.Count > 0;
        if (fatal)
        {
            var upgradableCards = PileType.Deck.GetPile(Owner).Cards.Where(c => c.IsUpgradable).ToList();
            if (upgradableCards.Count > 0)
            {
                await Cmd.Wait(0.5f);
                var cardModel = Owner.RunState.Rng.Niche.NextItem(upgradableCards);
                if (cardModel == null) return;
                Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(Owner.NetId).UpgradedCards.Add(cardModel.Id);
                cardModel.UpgradeInternal();
                cardModel.FinalizeUpgradeInternal();
                if (LocalContext.IsMe(Owner))
                    NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(NCardSmithVfx.Create([
                        cardModel
                    ])!);
            }
        }
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3M);
    }
}