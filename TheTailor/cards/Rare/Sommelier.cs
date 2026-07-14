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
using MegaCrit.Sts2.Core.Factories;
using TheTailor.Potions;

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class Sommelier() : CustomCardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/sommelierBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/sommelierBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/sommelierBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPotion<WeaversWine>()];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PotionCmd.TryToProcure(ModelDb.Potion<WeaversWine>().ToMutable(), Owner);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}