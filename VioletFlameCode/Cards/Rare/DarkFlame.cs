using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class DarkFlame() : VioletFlameCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.ForEnergy(this),
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(7)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, (c => c is { Type: CardType.Attack, Enchantment: null }), this)).FirstOrDefault();
        if (card != null)
            CardCmd.Enchant<Corrupted>(card, 1M);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, Owner.Creature, 1M, Owner.Creature, this);
        if (Owner.Creature.GetPowerAmount<VulnerablePower>() == 7)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, 7M, ValueProp.Unpowered, this, play);
            if (IsUpgraded)
            {
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, 7M, Owner.Creature, this);
                await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, 7M, Owner.Creature, this);
            }
            await CardPileCmd.Draw(choiceContext, 7M, Owner);
            await PlayerCmd.GainEnergy(7M, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        
    }
}