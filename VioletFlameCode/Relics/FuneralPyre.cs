using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.CustomEnums;
using VioletFlame.VioletFlameCode.Relics;

namespace VioletFlame.VioletFlameCode.Relics;

public class FuneralPyre() : VioletFlameRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource,
        CardPlay? cardPlay)
    {
        return cardSource.Tags.Contains(VioletFlameTag.Fatal) ? 6M : 1M;
    }
}