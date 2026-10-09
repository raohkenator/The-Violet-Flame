using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using VioletFlame.VioletFlameCode.Character;
using VioletFlame.VioletFlameCode.Extensions;

namespace VioletFlame.VioletFlameCode.Potions;

[Pool(typeof(VioletFlamePotionPool))]
public abstract class VioletFlamePotion : CustomPotionModel
{
    public override string? CustomPackedImagePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();

    public override string? CustomPackedOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}