using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using VioletFlame.VioletFlameCode.Relics;

namespace VioletFlame.VioletFlameCode.Relics;

public class BottledFlame() : VioletFlameRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;

    public override async Task AfterObtained()
    {
        CardModel mutableCard = (await CardSelectCmd.FromDeckGeneric(Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), Filter)).FirstOrDefault();
        if (mutableCard == null)
            return;
        mutableCard.AddKeyword(CardKeyword.Innate);
    }

    private bool Filter(CardModel card) => card.Type != CardType.Quest && !card.Keywords.Contains(CardKeyword.Innate);
}