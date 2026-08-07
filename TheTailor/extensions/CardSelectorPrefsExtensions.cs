using BaseLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace TheTailor.Extensions
{
    public static class CardSelectorPrefsExtensions
    {
        public static LocString StitchSelectionPrompt => new LocString("card_selection", "TO_STITCH");
        public static LocString StitchPatchSelectionPrompt => new LocString("card_selection", "TO_STITCH_PATCH");
        public static LocString StitchSlipperSelectionPrompt => new LocString("card_selection", "TO_STITCH_SLIPPER");
        public static LocString StitchCloneSelectionPrompt => new LocString("card_selection", "TO_STITCH_CLONE");
        public static LocString ReplaceMinionSelectionPrompt => new LocString("card_selection", "TO_REPLACE_MINION");
    }
}