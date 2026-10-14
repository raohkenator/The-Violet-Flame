using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.CustomEnums;

namespace VioletFlame.VioletFlameCode.Cards.Uncommon;

public class BladeOfAsh() : VioletFlameCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    private const string IncreaseKey = "Increase";
    private const int BaseDamage = 8;
    private int _currentDamage = 8;
    private int _increaseDamage;

    [SavedProperty]
    private int CurrentDamage
    {

        get => _currentDamage;
        set {
            this.AssertMutable();
            _currentDamage = value;
            this.DynamicVars.Damage.BaseValue = _currentDamage;
        }
    }

    [SavedProperty]
    private int IncreaseDamage
    {
        get => _increaseDamage;
        set {
            this.AssertMutable();
            _increaseDamage = value;
        }
    }
    
    protected override HashSet<CardTag> CanonicalTags => [VioletFlameTag.Fatal];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new DamageVar(this.CurrentDamage, ValueProp.Move),
    new IntVar(IncreaseKey, 2M)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target);
        AttackCommand atkCmd = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play)
            .Targeting(play.Target);
        
        var result = await atkCmd.ExecuteAndCheckFatal(choiceContext);
        bool killed = result.Count > 0;
        
        if (!killed) return;
        int intValue = DynamicVars[IncreaseKey].IntValue;
        if (DeckVersion is BladeOfAsh deckVersion)
        {
            this.BuffFromFatal(intValue);
            deckVersion.BuffFromFatal(intValue);
            return;
        }
        this.BuffFromFatal(intValue);
    }

    protected override void OnUpgrade() { this.DynamicVars[IncreaseKey].UpgradeValueBy(2M); }
    protected override void AfterDowngraded() => this.UpdateDamage();

    private void BuffFromFatal(int extraDamage)
    {
        this.IncreaseDamage += extraDamage;
        this.UpdateDamage();
    }
    
    private void UpdateDamage() { this.CurrentDamage = BaseDamage + this.IncreaseDamage; }
}