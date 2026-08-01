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
using HarmonyLib;

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class Pincushion() : CustomCardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self), IOnStitchEffect
    {
        public List<CardModel> relatedCards;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new DynamicVar("Delicate", 2), new DynamicVar("RelatedCards", 0), new DynamicVar("HasRelatedCards", 1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Delicate), HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            foreach (CardModel card in relatedCards)
            {
                await CardCmd.AutoPlay(choiceContext, card, StitchCardModifier.GetTarget(card, cardPlay.Card.CombatState), AutoPlayType.Default);
                await CardCmd.Exhaust(choiceContext, card, false, true);
            }
        }

        public async void OnStitch(CardModel card, CardModel stitchedCard)
        {
            StitchCardModifier? cardStitch = this.GetModifier<StitchCardModifier>();
            if ((card == this || stitchedCard == this) && IsMutable && cardStitch != null && cardStitch.StitchedCard != null)
            {
                relatedCards.Add(cardStitch.StitchedCard);
                await CardCmd.Exhaust(new ThrowingPlayerChoiceContext(), cardStitch.StitchedCard);
                DynamicVars["RelatedCards"].UpgradeValueBy(1);
                DynamicVars["HasRelatedCards"].UpgradeValueBy(1);
            }
        }
        public async void OnUnstitch(CardModel card)
        {
            
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Delicate"].UpgradeValueBy(1);
            RemoveKeyword(CardKeyword.Exhaust);
        }

        protected override void AfterCloned()
        {
            base.AfterCloned();
            relatedCards = new List<CardModel>();
        }
    }
}