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

namespace TheTailor.Cards.Common
{
    [Pool(typeof(TheTailorCardPool))]
    public class Slippers() : CustomCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self), IOnStitchEffect
    {
        // public override bool GainsBlock => true;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/slippersBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/slippersBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/slippersBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new BlockVar(4m ,ValueProp.Move), new DynamicVar("ReplayPluralize", 1), new DynamicVar("Replay", 1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitched)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Cards.UpgradeValueBy(1m);
        }

        public void OnStitch()
        {
            BaseReplayCount += 1;
            DynamicVars["ReplayPluralize"].BaseValue = 2;
        }

        public void OnUnstitch()
        {
            BaseReplayCount -= 1;
            DynamicVars["ReplayPluralize"].BaseValue = 1;
        }
    }
}