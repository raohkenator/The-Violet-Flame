using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.CustomEnums;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class FinalStrike() : VioletFlameCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike, VioletFlameTag.Fatal];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(40M, ValueProp.Move),
        new PowerVar<VigorPower>(10M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    public bool HasKilledThisPlay { get; set; } = false;

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null) return;
        AttackCommand atkCmd = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play)
            .Targeting(play.Target);
        
        var result = await atkCmd.ExecuteAndCheckFatal(choiceContext);
        bool killed = result.Count > 0;
        
        if (killed)
            HasKilledThisPlay = true;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.IsLastInSeries)
        {
            if (HasKilledThisPlay)
            {
                await PowerCmd.Apply<VigorPower>(choiceContext, this.Owner.Creature, DynamicVars["VigorPower"].IntValue, this.Owner.Creature, this);
                HasKilledThisPlay = false;
                return;
            }

            await CreatureCmd.Kill(Owner.Creature);
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(10M);
    }
}