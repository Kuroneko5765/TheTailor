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
using HarmonyLib;
using MinionLib.Utilities;

namespace TheTailor.Cards.Common
{
    [Pool(typeof(TheTailorCardPool))]
    public class Versatility() : CustomCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Minion };
        public override bool GainsBlock => true;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/versatilityBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/versatilityBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/versatilityBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Triggers", 1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Convert), HoverTipFactory.FromKeyword(TheTailor.Keywords.LeatherMinion), HoverTipFactory.FromKeyword(TheTailor.Keywords.LinenMinion)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int toConvert = DynamicVars["Triggers"].IntValue;

            PetsOrderAccessor accessor = new PetsOrderAccessor(cardPlay.Card.Owner);
            if (accessor != null && accessor.Pets != null)
            {
                for (int i = 0; i < accessor.Pets.Count; i++)
                {
                    if (accessor.Pets[i] != null && accessor.Pets[i].Monster is MinionLeather)
                    {
                        await TailorMinionCmd.ReplaceMinion<MinionLinen>(choiceContext, cardPlay.Card.Owner, i, true);
                        toConvert--;

                        if (toConvert <= 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Triggers"].UpgradeValueBy(1);
        }
    }
}