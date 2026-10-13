using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using VioletFlame.VioletFlameCode.CustomEnums;
using VioletFlame.VioletFlameCode.Powers;
using VioletFlame.VioletFlameCode.Utils;

namespace VioletFlame.VioletFlameCode.Powers;

public class PursuitPower() : VioletFlamePower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(Owner.Player);
        if (!play.Card.IsFatalCard() || play.Card.Type != CardType.Attack)
            return;
        await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
    }
}