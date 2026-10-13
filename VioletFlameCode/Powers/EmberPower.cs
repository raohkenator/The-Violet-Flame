using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Powers;

public class EmberPower() : VioletFlamePower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    private int _vigorCount;

    public override Task BeforeAttack(AttackCommand command)
    {
        if (Owner != command.Attacker) return Task.CompletedTask;
        _vigorCount = command.Attacker.GetPowerAmount<VigorPower>() / 2;
        return  Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Card.Type != CardType.Attack)
            return;
        await PowerCmd.Apply<VigorPower>(choiceContext, Owner, _vigorCount, null, null);
    }
}