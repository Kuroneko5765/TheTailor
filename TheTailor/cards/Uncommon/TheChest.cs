using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTailor;
using TheTailor.Extensions;
using TheTailor.Cards;
using TheTailor.Minions;
using MinionLib.Commands;
using MinionLib.Minion;
using TheTailor.Character;

namespace TheTailor.Cards.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class TheChest() : CustomCardModel(9, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Minion };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/theChestBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/theChestBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/theChestBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Upgrades", 1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.SilkMinion)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
        {
            if (card == this && card.Owner == Owner)
            {
                for (int i = 0; i < 2; i++)
                {
                    await TailorMinionCmd.AddOrReplaceMinion<MinionSilk>(choiceContext, Owner, true);
                }

                for (int i = 0; i < DynamicVars["Upgrades"].IntValue; i++)
                {
                    foreach (CardModel item in PileType.Hand.GetPile(Owner).Cards.Where((CardModel c) => c.IsUpgradable))
                    {
                        CardCmd.Upgrade(item);
                    }
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Upgrades"].UpgradeValueBy(1);
        }
    }
}