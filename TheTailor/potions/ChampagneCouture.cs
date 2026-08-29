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
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MinionLib.Commands;
using MinionLib.Minion;
using TheTailor;
using TheTailor.Cards;
using TheTailor.Character;
using TheTailor.Extensions;
using TheTailor.Minions;

namespace TheTailor.Potions
{
    [Pool(typeof(TheTailorPotionPool))]
    public class ChampagneCouture : CustomPotionModel
    {
        public override string? CustomPackedImagePath => "res://TheTailor/images/potions/champagneCouture.png";
        public override string? CustomPackedOutlinePath => "res://TheTailor/images/potions/champagneCoutureOutline.png";
        public override PotionRarity Rarity => PotionRarity.Rare;
        public override PotionUsage Usage => PotionUsage.AnyTime;
        public override TargetType TargetType => TargetType.Self;
        public override bool CanBeGeneratedInCombat => false;

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            IEnumerable<CardModel>? _selection;
            CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, 1)
            {
                Cancelable = false,
                RequireManualConfirmation = true
            };
            _selection = await CardSelectCmd.FromDeckForUpgrade(Owner, prefs);
            if (!_selection.Any())
            {
                return;
            }
            foreach (CardModel item in _selection)
            {
                CardCmd.Upgrade(item, CardPreviewStyle.None);
                if (Owner.PlayerCombatState != null)
                {
                    IEnumerable<CardModel> enumerable = Owner.PlayerCombatState.AllCards.Where((CardModel cm) => cm.IsUpgradable && cm.DeckVersion == item);
                    foreach (CardModel cardModel in enumerable)
                    {
                        CardCmd.Upgrade(cardModel);
                    }
                }
            }
        }
    }
}