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

namespace TheTailor.Cards.Ancient
{
    [Pool(typeof(TheTailorCardPool))]
    public class Atelier() : CustomCardModel(9, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
        public override int MaxUpgradeLevel => 99999;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/atelierBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/atelierBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/atelierBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Premium)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<AtelierPower>(choiceContext, Owner.Creature, DynamicVars.Cards.BaseValue, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1m);
        }
    }
}