using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace VioletFlame.VioletFlameCode.Relics;

public class PhoenixSpark() : VioletFlameRelic
{
    private bool _wasUsed = false;
    
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5M, ValueProp.Unpowered)];
    
    [SavedProperty]
    private bool WasUsed
    {
        get => _wasUsed;
        set
        {
            AssertMutable();
            _wasUsed = value;
            if (!_wasUsed)
                return;
            this.Status = RelicStatus.Disabled;
        }
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (WasUsed)
        {
            WasUsed = false;
            this.Status = RelicStatus.Active;
        }
        return Task.CompletedTask;
    }

    public override bool ShouldDieLate(Creature creature)
    {
        return creature != this.Owner.Creature || this.WasUsed;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        PlayerChoiceContext choiceContext = new ThrowingPlayerChoiceContext();
        var combatState = creature.CombatState;
        ArgumentNullException.ThrowIfNull(combatState);
        this.WasUsed = true;
        await CreatureCmd.Heal(creature, 1M);
        await CreatureCmd.Damage(choiceContext, combatState.HittableEnemies, this.DynamicVars.Damage, this.Owner.Creature);
    }
    
}