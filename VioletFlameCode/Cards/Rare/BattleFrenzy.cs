using System.Runtime.ExceptionServices;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards;
using VioletFlame.VioletFlameCode.CustomEnums;

namespace VioletFlame.VioletFlameCode.Cards.Rare;

public class BattleFrenzy() : VioletFlameCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override HashSet<CardTag> CanonicalTags => [VioletFlameTag.Fatal];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override IEnumerable<DynamicVar> CanonicalVars => [ 
        new DamageVar(10M, ValueProp.Move),
        new PowerVar<VigorPower>(5M)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        AttackContext attackContext = await AttackCommand.CreateContextAsync(CombatState, choiceContext, play);

        object? obj = null;
        int num = 0;

        try
        {
            IEnumerable<DamageResult> damageResults;
            for (int attackCount = 1; attackCount > 0; attackCount += damageResults.Count(r =>
                     r.WasTargetKilled && r.Receiver.Powers.All(p => p.ShouldOwnerDeathTriggerFatal())))
            {
                --attackCount;
                damageResults = await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, DynamicVars.Damage,
                    Owner.Creature, this, play);
                attackContext.AddHit(damageResults);
                await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].BaseValue,
                    Owner.Creature, this);
            }

            num = 1;
        }
        catch (Exception ex)
        {
            obj = ex;
        }

        if (attackContext != null)
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
        DynamicVars.Damage.UpgradeValueBy(2M);
        DynamicVars["VigorPower"].UpgradeValueBy(3M);
    }
}