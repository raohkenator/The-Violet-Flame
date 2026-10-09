using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using VioletFlame.VioletFlameCode.CustomEnums;
using VioletFlame.VioletFlameCode.Utils;

namespace VioletFlame.VioletFlameCode.Powers;

public class DoubleTapPower() : VioletFlamePower
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
    
    public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if(!(power is DoubleTapPower) || power.Owner != this.Owner)
            return Task.CompletedTask;
        Player? player = this.Owner.Player;
        object? obj;
        if (player == null)
            obj = null;
        else
        {
            PlayerCombatState? playerCombatState = player.PlayerCombatState;
            obj = playerCombatState != null ? playerCombatState.AllCards : null;
        }
        if (obj == null)
            obj = Array.Empty<CardModel>();
        foreach (CardModel card in (IEnumerable<CardModel>)obj)
            this.TryAddReplay(card, (int) amount);
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.IsClone)
            return Task.CompletedTask;
        this.TryAddReplay(card, this.Amount);
        return Task.CompletedTask;
    }

    public override Task AfterRemoved(Creature oldOwner)
    {
        Player? player = oldOwner.Player;
        object? obj;
        if (player == null)
            obj = null;
        else
        {
            PlayerCombatState? playerCombatState = player.PlayerCombatState;
            obj = playerCombatState != null ? playerCombatState.AllCards : null;
        }
        if (obj == null)
            obj = Array.Empty<object>();
        foreach (CardModel card in (IEnumerable<CardModel>) obj)
        {
            if (card.Tags.Contains(VioletFlameTag.Fatal))
                card.BaseReplayCount -= this.Amount;
        }
        return Task.CompletedTask;
    }

    private void TryAddReplay(CardModel card, int amount)
    {
        if (card.Owner != this.Owner.Player || !card.IsFatalCard())
            return;
        card.BaseReplayCount += amount;
    }
}