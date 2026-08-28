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
using TheTailor.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using TheTailor.Character;
using TheTailor.Cards.Token;

namespace TheTailor.Cards.Multiplayer.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class BoxOfChocolates() : CustomCardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
    {
        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/boxOfChocolatesBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/boxOfChocolatesBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/boxOfChocolatesBeta.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Target != null && cardPlay.Target.IsPlayer)
            {
                foreach (CardModel item in PileType.Hand.GetPile(cardPlay.Target.Player).Cards.Where((CardModel c) => c.IsUpgradable))
                {
                    CardCmd.Upgrade(item);
                }
            }
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}