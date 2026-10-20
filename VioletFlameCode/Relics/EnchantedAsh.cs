using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using VioletFlame.VioletFlameCode.Relics;

namespace VioletFlame.VioletFlameCode.Relics;

public class EnchantedAsh() : VioletFlameRelic, IHealAmountModifier
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    public decimal ModifyHealMultiplicative(Creature creature, decimal amount)
    {
        return creature == Owner.Creature ? 1.5M : 1;
    }
}