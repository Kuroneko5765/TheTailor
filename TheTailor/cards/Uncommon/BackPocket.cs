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
using HarmonyLib;
using TheTailor.Character;

namespace TheTailor.Cards.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class BackPocket() : CustomCardModel(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/backPocketBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/backPocketBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/backPocketBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Delicate", -999)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch), IsUpgraded ? HoverTipFactory.FromKeyword(TheTailor.Keywords.Delicate) : HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            IEnumerable<CardModel> cardModel = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefsExtensions.StitchSelectionPrompt, 1), context: choiceContext, player: base.Owner, filter: StitchCmd.CanBeStitched, source: this);
            IEnumerable<CardModel> cardModel2 = await CardSelectCmd.FromCombatPile(prefs: new CardSelectorPrefs(CardSelectorPrefsExtensions.StitchSelectionPrompt, 1), context: choiceContext, player: base.Owner, filter: StitchCmd.CanBeStitched, pile: PileType.Discard.GetPile(Owner));
            if (cardModel != null && cardModel2 != null && cardModel.Count() == 1 && cardModel2.Count() == 1)
            {
                await StitchCmd.StitchCards(cardModel.ElementAt(0), cardModel2.ElementAt(0));
            }
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
            DynamicVars["Delicate"].BaseValue = 2;
        }
    }
}