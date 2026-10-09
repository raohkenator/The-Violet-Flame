using System.Runtime.ExceptionServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.CustomEnums;

namespace VioletFlame.VioletFlameCode.Cards.Common;

public class VioletFlameBlade() : VioletFlameCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [VioletFlameTag.Fatal];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8M, ValueProp.Move)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(this.CombatState);
        ArgumentNullException.ThrowIfNull(play.Target);
        AttackContext attackContext = await AttackCommand.CreateContextAsync(this.CombatState, choiceContext, play);

        object? obj = null;
        try
        {
            var damageResults = await CreatureCmd.Damage(choiceContext, play.Target, DynamicVars.Damage, this, play);
            
            attackContext.AddHit(damageResults);
            
            bool killed = damageResults.Any(c => c.Receiver.Powers.All(p => p.ShouldOwnerDeathTriggerFatal()) && c.WasTargetKilled);

            if (killed)
            {
                var followUpAtk = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .WithHitCount(2)
                    .FromCard(this, play)
                    .TargetingAllOpponents(this.CombatState)
                    .Execute(choiceContext);
                foreach (var hit in followUpAtk.Results)
                {
                    attackContext.AddHit(hit);
                }
            }
        }
        catch (Exception ex)
        {
            obj = ex;
        }
        
        await attackContext.DisposeAsync();
        
        if (obj != null)
        {
            if (!(obj is Exception source))
                throw (Exception)obj;
            ExceptionDispatchInfo.Capture(source).Throw();
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(3M);
    }
}