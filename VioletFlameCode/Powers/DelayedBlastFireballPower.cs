using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Powers;

public class DelayedBlastFireballPower() : VioletFlamePower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override PowerInstanceType InstanceType =>
        PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(28M, ValueProp.Unpowered)];

    public void SetDamage(Decimal damage)
    {
        AssertMutable();
        DynamicVars.Damage.BaseValue = damage;
    }

    public override async Task BeforeSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        if (Amount > 1)
            await PowerCmd.Decrement(this);
        else
        {
            await Cmd.CustomScaledWait(0.2f, 0.4f);
            foreach (Creature hittableEnemy in CombatState.HittableEnemies)
            {
                NCombatRoom? instance = NCombatRoom.Instance;
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely(NFireSmokePuffVfx.Create(hittableEnemy));
            }
            await Cmd.CustomScaledWait(0.2f, 0.4f);
            await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, DynamicVars.Damage, Owner);
            await PowerCmd.Remove(this);
        }
    }
}