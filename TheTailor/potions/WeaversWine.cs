using System.Collections.ObjectModel;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Patches.Localization;
using BaseLib.Patches.Saves;
using BaseLib.Utils;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using TheTailor;
using TheTailor.Cards;
using TheTailor.Character;
using TheTailor.Extensions;

namespace TheTailor.Potions
{
    [Pool(typeof(TheTailorPotionPool))]
    public class WeaversWine : CustomPotionModel
    {
        public override string? CustomPackedImagePath => "res://TheTailor/images/potions/weaversWine.png";
        public override string? CustomPackedOutlinePath => "res://TheTailor/images/potions/weaversWineOutline.png";
        public override PotionRarity Rarity => PotionRarity.Common;
        public override PotionUsage Usage => PotionUsage.CombatOnly;
        public override TargetType TargetType => TargetType.Self;

        public override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromKeyword(Keywords.Stitch)
        ];

        public override bool PassesCustomUsabilityCheck => IsUsable();

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            IEnumerable<CardModel> cardModel = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(TheTailor.Extensions.CardSelectorPrefsExtensions.StitchSelectionPrompt, 2), context: choiceContext, player: base.Owner, filter: StitchCmd.CanBeStitched, source: this);
            if (cardModel != null && cardModel.Count() == 2)
            {
                await StitchCmd.StitchCards(cardModel.ElementAt(0), cardModel.ElementAt(1));
            }
        }

        public bool IsUsable()
        {
            if (Owner.PlayerCombatState?.Hand.Cards.Count() >= 2)
            {
                int playableCards = 0;
                foreach (CardModel card in Owner.PlayerCombatState.Hand.Cards)
                {
                    if (StitchCmd.CanBeStitched(card))
                    {
                        playableCards++;
                    }
                }

                if (playableCards >= 2)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}