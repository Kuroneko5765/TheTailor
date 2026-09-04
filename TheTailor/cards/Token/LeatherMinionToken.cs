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
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Combat;

namespace TheTailor.Cards.Token
{
    [Pool(typeof(TokenCardPool))]
    public class LeatherMinionToken() : CustomCardModel(-1, CardType.Status, CardRarity.Token, TargetType.None)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/leatherMinionTokenBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/leatherMinionTokenBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/leatherMinionTokenBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("ChoiceIndex", -1), new DynamicVar("Health", 0), new DynamicVar("HealthPluralize", 1)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {

        }
    }
}