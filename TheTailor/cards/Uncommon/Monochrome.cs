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
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using TheTailor.Character;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Context;
using Godot;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Models.Cards;

namespace TheTailor.Cards.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class Monochrome() : CustomCardModel(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/monochrome.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/monochrome.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/monochromeBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            IEnumerable<CardModel> cardModels = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefsExtensions.StitchCloneSelectionPrompt, 1), context: choiceContext, player: Owner, filter: StitchCmd.CanBeStitched, source: this);
            if (cardModels != null && cardModels.Count() >= 1)
            {
                CardModel cm = cardModels.First();
                if (IsUpgraded && cm.IsUpgradable)
                {
                    CardCmd.Upgrade(cm);
                }

                CardModel cm2 = cm.CreateClone();
                await CardPileCmd.AddGeneratedCardToCombat(cm2, PileType.Hand, Owner, CardPilePosition.Top);
                await StitchCmd.StitchCards(cm, cm2);
            }
        }
    }
}