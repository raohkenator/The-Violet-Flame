using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Relics;

namespace VioletFlame.VioletFlameCode.Relics;

public class TorchSconce() : VioletFlameRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        // checks if it's Players' turn and Owner has Vigor before triggering
        if (!participants.Contains(Owner.Creature) || !Owner.Creature.Powers.Any(p => p is VigorPower))
            return;
        int vigorCount = Owner.Creature.GetPower<VigorPower>().Amount;
        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, vigorCount, ValueProp.Unpowered, null);
    }
}