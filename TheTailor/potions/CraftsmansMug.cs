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
    public class CraftsmansBrew : CustomPotionModel
    {
        public override string? CustomPackedImagePath => "res://TheTailor/images/potions/craftsmansBrew.png";
        public override string? CustomPackedOutlinePath => "res://TheTailor/images/potions/craftsmansBrewOutline.png";
        public override PotionRarity Rarity => PotionRarity.Uncommon;
        public override PotionUsage Usage => PotionUsage.CombatOnly;
        public override TargetType TargetType => TargetType.Self;

        public override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromKeyword(Keywords.LeatherMinion)
        ];

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            for (int i = 0; i < 3; i++)
            {
                if (TailorMinionCmd.CanMinionBeAdded(target.Player))
                {
                    await MinionCmd.AddMinion<MinionLeather>(choiceContext, target.Player, new MinionSummonOptions(Position: MinionPosition.Front));
                }
            }
        }
    }
}