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
using TheTailor.Character;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Extensions;
using TheTailor.Minions;

namespace TheTailor.Cards.Common
{
    [Pool(typeof(TheTailorCardPool))]
    public class Retreat() : CustomCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        public override bool GainsBlock => true;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/retreatBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/retreatBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/retreatBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(4, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int plays = 1;
            if (TailorMinionCmd.GetMinionCount<TailorMinion>(Owner) <= 0)
            {
                plays += 2;
            }

            for (int i = 0; i < plays; i++)
            {
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(1);
        }
    }
}