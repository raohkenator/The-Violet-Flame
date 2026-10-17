using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using VioletFlame.VioletFlameCode.Cards.Other;
using VioletFlame.VioletFlameCode.CustomEnums;


namespace VioletFlame.VioletFlameCode.Cards.Basic;

public class Ignite() : VioletFlameCard(2,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy), ITranscendenceCard
{
    public CardModel GetTranscendenceTransformedCard() => ModelDb.Card<Daybreak>();

    protected override HashSet<CardTag> CanonicalTags => [ VioletFlameTag.Fatal ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6M, ValueProp.Move),
        new HealVar(2M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        ArgumentNullException.ThrowIfNull(play.Target, "play.Target");
        AttackCommand atkCmd = DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount(2)
            .FromCard(this, play)
            .Targeting(play.Target);

        // checks if any valid target was killed for Fatal condition
        var result = await atkCmd.ExecuteAndCheckFatal(choiceContext);
        bool killed = result.Count > 0;
        
        // var shouldTriggerFatal = play.Target!.Powers.All(p => p.ShouldOwnerDeathTriggerFatal());
        // var wasTargetKilled = atkCmd.Results.SelectMany(e => e).Any(e => e.WasTargetKilled);
        if (!killed) return;
        await CreatureCmd.Heal(this.Owner.Creature, DynamicVars.Heal.BaseValue);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Heal.UpgradeValueBy(1M);
    }
}