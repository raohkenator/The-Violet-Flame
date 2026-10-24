using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards.Uncommon;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Powers;

public class ElementalFormPower() : VioletFlamePower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ("StrengthBase", 0),
        new ("StrengthExtra", 3),
        new CustomCalculatedVar("Strength").WithMultiplier((p, c) => p.Amount),
        new ("DisintegrationBase", 0),
        new ("DisintegrationExtra", 1),
        new CustomCalculatedVar("Disintegration").WithMultiplier((p, c) => p.Amount),
        new ("VigorBase", 0),
        new ("VigorExtra", 7),
        new CustomCalculatedVar("Vigor").WithMultiplier((p, c) => p.Amount)
    ];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player)
            return;
        await PowerCmd.Apply<StrengthPower>(choiceContext, player.Creature, ((CustomCalculatedVar)DynamicVars["Strength"]).CalculateCustom(player.Creature), null, null);
        await PowerCmd.Apply<DisintegrationPower>(choiceContext, player.Creature, ((CustomCalculatedVar)DynamicVars["Disintegration"]).CalculateCustom(player.Creature), null, null);
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power,
        decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (power.Owner == Owner && power is ElementalFormPower)
            InvokeDisplayAmountChanged();
        if (power.GetTypeForAmount(amount) == PowerType.Debuff && power.Owner == Owner)
        {
            await PowerCmd.Apply<VigorPower>(choiceContext, Owner, ((CustomCalculatedVar)DynamicVars["Vigor"]).CalculateCustom(Owner), null, null);
        }
        
    }
}