using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using VioletFlame.VioletFlameCode.Cards;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class PowerUp() : VioletFlameCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override bool HasEnergyCostX => true;
    
    protected override HashSet<CardTag> CanonicalTags => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VigorPower>(10M),
        new EnergyVar(1)
    ];
    
    

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        Creature player = Owner.Creature;
        for (int i = 0; i < ResolveEnergyXValue(); i++)
            await PowerCmd.Apply<VigorPower>(choiceContext, player, DynamicVars["VigorPower"].BaseValue, player, this);
        await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, player, 2M, player, this);
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, player, DynamicVars.Energy.BaseValue, player, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["VigorPower"].UpgradeValueBy(2M);
        DynamicVars.Energy.UpgradeValueBy(1M);
    }
}