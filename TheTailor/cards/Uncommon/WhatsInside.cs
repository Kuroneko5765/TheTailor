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
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace TheTailor.Cards.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class WhatsInside() : CustomCardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Minion };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/whatsInsideBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/whatsInsideBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/whatsInsideBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.CottonMinion)];
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Triggers", 1)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            PetsOrderAccessor accessor = new PetsOrderAccessor(cardPlay.Card.Owner);
            if (accessor != null && accessor.Pets != null && accessor.Pets[0] != null)
            {
                for (int i = 0; i < DynamicVars["Triggers"].BaseValue; i++)
                {
                    await TailorMinionCmd.TriggerMinionAbility(choiceContext, cardPlay.Card.Owner, 0);
                }

                await TailorMinionCmd.ReplaceMinion<MinionCotton>(choiceContext, cardPlay.Card.Owner, 0);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Triggers"].UpgradeValueBy(1);
        }
    }
}