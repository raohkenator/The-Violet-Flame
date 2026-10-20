using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using VioletFlame.VioletFlameCode.Relics;

namespace VioletFlame.VioletFlameCode.Relics;

public class CursedAsh() : VioletFlameRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        ArgumentNullException.ThrowIfNull(Owner.Creature.CombatState);
        if (power.Owner != Owner.Creature || amount >= 0 || power is not DexterityPower)
            return;
        var enemies = Owner.Creature.CombatState.Enemies;
        Flash();
        await PowerCmd.Apply<DexterityPower>(choiceContext, enemies, -1M, Owner.Creature, null);
    }
}