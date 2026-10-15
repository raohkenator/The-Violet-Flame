using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using VioletFlame.VioletFlameCode.Cards;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class Brawl() : VioletFlameCard(3,
    CardType.Skill, CardRarity.Rare,
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
        List<CardModel> cards = [null, null, null];
        cards[0] = (PileType.Draw.GetPile(Owner).Cards.Where((c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable))).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault() ?? PileType.Draw.GetPile(Owner).Cards.Where((c => c.Type == CardType.Attack)).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault());
        cards[1] = (PileType.Discard.GetPile(Owner).Cards.Where((c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable))).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault() ?? PileType.Discard.GetPile(Owner).Cards.Where((c => c.Type == CardType.Attack)).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault());
        cards[2] = (PileType.Hand.GetPile(Owner).Cards.Where((c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable))).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault() ?? PileType.Hand.GetPile(Owner).Cards.Where((c => c.Type == CardType.Attack)).ToList().StableShuffle(Owner.RunState.Rng.Shuffle).FirstOrDefault());
        foreach (CardModel card in cards)
        {
            if (card == null)
                continue;
            if (!CombatManager.Instance.IsEnding)
            {
                if (card.TargetType == TargetType.AnyEnemy)
                {
                    Creature target = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
                    await CardCmd.AutoPlay(choiceContext, card, target);
                }
                else
                    await CardCmd.AutoPlay(choiceContext, card, null);
            }
            else
                break;
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}