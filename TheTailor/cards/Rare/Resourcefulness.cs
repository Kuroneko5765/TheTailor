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
using TheTailor.Powers;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MinionLib.Commands;
using MinionLib.Minion;
using TheTailor.Character;

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class Resourcefulness() : CustomCardModel(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/resourcefulnessBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/resourcefulnessBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/resourcefulnessBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
        protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(2)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
            await PowerCmd.Apply<ResourcefulnessPower>(choiceContext, Owner.Creature, DynamicVars.Heal.IntValue, Owner.Creature, this, false);

            if (IsUpgraded)
            {
                IEnumerable<CardModel> cardModels = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 0, 2), context: choiceContext, player: base.Owner, filter: null, source: this);
                if (cardModels != null && cardModels.Count() > 0)
                {
                    foreach (CardModel cardModel in cardModels)
                    {
                        await CardCmd.Exhaust(choiceContext, cardModel);
                    }
                }
            }
        }
    }
}