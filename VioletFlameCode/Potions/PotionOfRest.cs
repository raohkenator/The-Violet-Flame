using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using VioletFlame.VioletFlameCode.Potions;

namespace VioletFlame.VioletFlameCode.Potions;

public class PotionOfRest() : VioletFlamePotion
{
    public override PotionRarity Rarity =>
        PotionRarity.Rare;

    public override PotionUsage Usage =>
        PotionUsage.CombatOnly;

    public override TargetType TargetType =>
        TargetType.AnyPlayer;

    protected override async Task OnUse(
        PlayerChoiceContext choiceContext,
        Creature? target)
    {
        AssertValidForTargetedPotion(target);
        await PowerCmd.Apply<RegenPower>(choiceContext, target, 10M, Owner.Creature, null);
        await PowerCmd.Apply<StrengthPower>(choiceContext, target, -2M, Owner.Creature, null);
        await PowerCmd.Apply<DexterityPower>(choiceContext, target, -2M, Owner.Creature, null);
    }
}
