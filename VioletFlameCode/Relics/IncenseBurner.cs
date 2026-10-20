using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using VioletFlame.VioletFlameCode.Relics;

namespace VioletFlame.VioletFlameCode.Relics;

public class IncenseBurner() : VioletFlameRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    private bool _isActivating;
    private int _turnsSeen;

    public override bool ShowCounter => true;

    public override int DisplayAmount
    {
        get => !IsActivating ? TurnsSeen : DynamicVars["Turns"].IntValue;
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Turns", 6M)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];

    public bool IsActivating
    {
        get => _isActivating;
        set
        {
            AssertMutable();
            _isActivating = value;
            InvokeDisplayAmountChanged();
        }
    }
    
    [SavedProperty]
    public int TurnsSeen
    {
        get => _turnsSeen;
        set
        {
            AssertMutable();
            _turnsSeen = value;
            InvokeDisplayAmountChanged();
        }
    }

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        TurnsSeen = (TurnsSeen + 1) % DynamicVars["Turns"].IntValue;
        Status = TurnsSeen == DynamicVars["Turns"].IntValue ? RelicStatus.Active : RelicStatus.Normal;
        if (TurnsSeen != 0)
            return;
        TaskHelper.RunSafely(DoActivateVisuals());
        await PowerCmd.Apply<IntangiblePower>(new ThrowingPlayerChoiceContext(), Owner.Creature, 1M, Owner.Creature, null);
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }
}