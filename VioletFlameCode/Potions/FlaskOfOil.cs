using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using VioletFlame.VioletFlameCode.Potions;

namespace VioletFlame.VioletFlameCode.Potions;

public class FlaskOfOil() : VioletFlamePotion
{
    public override PotionRarity Rarity =>
        PotionRarity.Common;

    public override PotionUsage Usage =>
        PotionUsage.CombatOnly;

    public override TargetType TargetType =>
        TargetType.AnyPlayer;

    protected override async Task OnUse(
        PlayerChoiceContext choiceContext,
        Creature? target)
    {
        AssertValidForTargetedPotion(target);
        await PowerCmd.Apply<VigorPower>(choiceContext, target, 8M, Owner.Creature, null);
    }
}
