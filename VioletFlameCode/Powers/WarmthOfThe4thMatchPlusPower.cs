using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Powers;

public class WarmthOfThe4thMatchPlusPower() : VioletFlamePower
{

    public override int DisplayAmount => 1 + (AttacksPlayedThisCombat % 4);

    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        return DisplayAmount == 4 && dealer == Owner ? 4M : 1M;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.CardPlay.Player != Owner.Player)
            return;
        InvokeDisplayAmountChanged();
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    private int AttacksPlayedThisCombat
    {
        get
        {
            return CombatManager.Instance.History.CardPlaysFinished.Count(c => 
                c.CardPlay.Player == Owner.Player && c.CardPlay.Card.Type == CardType.Attack);
        }
    }
    
}