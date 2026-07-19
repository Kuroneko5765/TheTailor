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

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class Pincushion() : CustomCardModel(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2), new DynamicVar("Delicate", 2), new EnergyVar(1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Delicate), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        }

        protected override CardLocation GetResultLocationForCardPlay()
        {
            CardLocation resultLocationForCardPlay = base.GetResultLocationForCardPlay();
            if (resultLocationForCardPlay.pileType == PileType.Discard)
            {
                resultLocationForCardPlay.pileType = PileType.Draw;
                resultLocationForCardPlay.position = CardPilePosition.Random;
            }
            return resultLocationForCardPlay;
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Delicate"].UpgradeValueBy(1);
        }
    }
}