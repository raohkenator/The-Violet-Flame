using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using VioletFlame.VioletFlameCode.Relics;

namespace VioletFlame.VioletFlameCode.Relics;

public class CrackedObsidianVisor() : VioletFlameRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Shop;

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState.TurnNumber > 1)
            return;
        Flash();
        await PowerCmd.Apply<ArtifactPower>(choiceContext, Owner.Creature, 1M, Owner.Creature, null);
    }
}