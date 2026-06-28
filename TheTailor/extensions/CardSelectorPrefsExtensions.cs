using BaseLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace TheTailor.Extensions
{
    public static class CardSelectorPrefsExtensions
    {
        extension(CardSelectorPrefs cardSelectorPrefs)
        {
            public static LocString StitchSelectionPrompt => new LocString("card_selection", "TO_STITCH");
        }
    }
}