using BaseLib.Abstracts;
using VioletFlame.VioletFlameCode.Extensions;
using Godot;

namespace VioletFlame.VioletFlameCode.Character;

public class VioletFlameRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => VioletFlame.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}