using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class ChangeStance() : VioletFlameCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<StrengthPower>(1M),
        new PowerVar<DexterityPower>(1M),
        new EnergyVar(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>(),
        EnergyHoverTip
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature,
            -DynamicVars["StrengthPower"].BaseValue, this.Owner.Creature, this);
        var dexterityPower = await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner.Creature,
            -DynamicVars["DexterityPower"].BaseValue, this.Owner.Creature, this);
        var changeStancePower = await PowerCmd.Apply<PyrePower>(choiceContext, this.Owner.Creature,
            DynamicVars.Energy.BaseValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Energy.UpgradeValueBy(1M);
    }
}