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
        if (!participants.Contains(Owner.Creature))
            return;
        int vigorCount = Owner.Creature.GetPowerAmount<VigorPower>();
        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, vigorCount, ValueProp.Unpowered, null);
    }
}