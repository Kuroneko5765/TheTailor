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
using MinionLib.Utilities;

namespace TheTailor.Cards.Common
{
    [Pool(typeof(TheTailorCardPool))]
    public class Mending() : CustomCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Minion };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/mendingBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/mendingBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/mendingBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(5)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            PetsOrderAccessor accessor = new PetsOrderAccessor(cardPlay.Card.Owner);
            if (accessor != null && accessor.Pets != null && accessor.Pets[0] != null)
            {
                await CreatureCmd.GainMaxHp(accessor.Pets[0], DynamicVars.Heal.BaseValue);
            }
        }

        /*
        public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
        {
            if (card.Owner != Owner || card is not Mending)
            {
                return true;
            }

            if (TailorMinionCmd.GetMinionCount<TailorMinion>(card.Owner) <= 0)
            {
                return false;
            }

            return true;
        }
        */

        protected override void OnUpgrade()
        {
            DynamicVars.Heal.UpgradeValueBy(3);
        }
    }
}