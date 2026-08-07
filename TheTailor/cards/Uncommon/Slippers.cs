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
using TheTailor.Cards.Token;

namespace TheTailor.Cards.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class Slippers() : CustomCardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/slippersBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/slippersBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/slippersBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);

            if (!StitchCmd.CanBeStitched(this))
            {
                return;
            }

            this.AddModifier<StitchBlockModifier>();

            IEnumerable<CardModel> cardModel = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(TheTailor.Extensions.CardSelectorPrefsExtensions.StitchSlipperSelectionPrompt, 1), context: choiceContext, player: Owner, filter: StitchCmd.CanBeStitchedExcluding, source: this);
            if (cardModel != null && cardModel.Count() == 1)
            {
                await StitchCmd.StitchCards(this, cardModel.ElementAt(0));
            }

            StitchBlockModifier? stitchBlockModifier = this.GetModifier<StitchBlockModifier>();
            if (stitchBlockModifier != null)
            {
                CardModifier.RemoveModifier(this, stitchBlockModifier);
            }
        }
        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1);
        }
    }
}