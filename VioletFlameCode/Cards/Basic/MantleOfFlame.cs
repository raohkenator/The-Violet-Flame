using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace VioletFlame.VioletFlameCode.Cards.Basic;

public class MantleOfFlame() : VioletFlameCard(1,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new PowerVar<VigorPower>(3M),
    new DynamicVar("DamageBack", 3M)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await PowerCmd.Apply<VigorPower>(choiceContext, this.Owner.Creature, this.DynamicVars["VigorPower"].IntValue, this.Owner.Creature, this);
        await PowerCmd.Apply<FlameBarrierPower>(choiceContext, this.Owner.Creature, this.DynamicVars["DamageBack"].BaseValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["VigorPower"].UpgradeValueBy(2M);
        DynamicVars["DamageBack"].UpgradeValueBy(2M);
    }
}