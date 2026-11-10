using BaseLib.Abstracts;
using BaseLib.Patches.UI;
using BaseLib.Utils.NodeFactories;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using VioletFlame.VioletFlameCode.Cards.Basic;
using VioletFlame.VioletFlameCode.Extensions;
using VioletFlame.VioletFlameCode.Relics;

namespace VioletFlame.VioletFlameCode.Character;

public class VioletFlame : PlaceholderCharacterModel
{
    public const string CharacterId = "VioletFlame";

    public static readonly Color Color = new("5e447a");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 75;

    public override Color MapDrawingColor => Color;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeVioletFlame>(),
        ModelDb.Card<StrikeVioletFlame>(),
        ModelDb.Card<StrikeVioletFlame>(),
        ModelDb.Card<StrikeVioletFlame>(),
        ModelDb.Card<DefendVioletFlame>(),
        ModelDb.Card<DefendVioletFlame>(),
        ModelDb.Card<DefendVioletFlame>(),
        ModelDb.Card<DefendVioletFlame>(),
        ModelDb.Card<Ignite>(),
        ModelDb.Card<MantleOfFlame>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<PhoenixSpark>()
    ];
    
    public override CardPoolModel CardPool => ModelDb.CardPool<VioletFlameCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<VioletFlameRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<VioletFlamePotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    
    public override string CustomIconTexturePath => "character_icon_violet_flame.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_violet_flame.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_violet_flame_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_violet_flame.png".CharacterUiPath();
    
    //Chest Screen
    public override string CustomArmPointingTexturePath => "res://VioletFlame/images/character/ui/hand_point.png";
    public override string CustomArmRockTexturePath => "res://VioletFlame/images/character/ui/hand_rock.png";
    public override string CustomArmPaperTexturePath => "res://VioletFlame/images/character/ui/hand_paper.png";
    public override string CustomArmScissorsTexturePath => "res://VioletFlame/images/character/ui/hand_scissors.png";

    public override RelicIconData? CustomYummyCookie => new(
        "res://VioletFlame/images/relics/big/yummy_cookie_violet_flame.png",
        "res://VioletFlame/images/relics/yummy_cookie_violet_flame_outline.png",
        "res://VioletFlame/images/relics/yummy_cookie_violet_flame_outline.png"
    );

    //Scene
    public override string CustomVisualPath => "res://VioletFlame/images/scene/violet_flame_sprite.tscn";
    public override string CustomMerchantAnimPath => "res://VioletFlame/images/scene/merchant/violet_flame_merchant.tscn";
    public override string CustomCharacterSelectBg => "res://VioletFlame/images/scene/charselect/char_background_violet_flame.tscn";
    public override string CustomRestSiteAnimPath => "res://VioletFlame/images/scene/restsite/violet_flame_rest_site.tscn";
}