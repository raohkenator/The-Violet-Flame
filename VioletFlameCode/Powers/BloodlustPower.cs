using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using VioletFlame.VioletFlameCode.Powers;

namespace VioletFlame.VioletFlameCode.Powers;

public class BloodlustPower() : VioletFlamePower
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterDeath(
        PlayerChoiceContext choiceContext,
        Creature creature,
        bool wasRemovalPrevented,
        float deathAnimLength)
    {
        ArgumentNullException.ThrowIfNull(Owner.Player);
        if (creature.Side == Owner.Side) return;
        await PlayerCmd.GainEnergy(Amount, Owner.Player);
        await CardPileCmd.Draw(choiceContext, Amount, Owner.Player);
    }
}