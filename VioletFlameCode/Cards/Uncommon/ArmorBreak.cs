using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class ArmorBreak() : VioletFlameCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7M, ValueProp.Move),
        new PowerVar<FrailPower>(3M),
        new PowerVar<VigorPower>(6M)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<FrailPower>(),
        HoverTipFactory.FromPower<VigorPower>()
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "play.Target");

        AttackCommand atkCmd = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play)
            .Targeting(play.Target)
            .Execute(choiceContext);
        //  Apply Frail and Vigor to Player
        var playerFrail = await PowerCmd.Apply<FrailPower>(choiceContext, this.Owner.Creature, DynamicVars["FrailPower"].IntValue, this.Owner.Creature, this);
        var playerVigor = await PowerCmd.Apply<VigorPower>(choiceContext, this.Owner.Creature, DynamicVars["VigorPower"].IntValue, this.Owner.Creature, this);
        //  Apply Frail to Target Enemy
        var targetFrail = await PowerCmd.Apply<FrailPower>(choiceContext, this.Owner.Creature, DynamicVars["VigorPower"].IntValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(4M);
        this.DynamicVars["VigorPower"].UpgradeValueBy(2M);
    }
}