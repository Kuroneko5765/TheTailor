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

namespace TheTailor.Cards.Common
{
    [Pool(typeof(TheTailorCardPool))]
    public class Wardrobe() : CustomCardModel(2, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        public override bool GainsBlock => true;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/wardrobeBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/wardrobeBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/wardrobeBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(14, ValueProp.Move), new CardsVar(2)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            for (int i = 0; i < DynamicVars.Cards.BaseValue; i++)
            {
                CardPile pile = PileType.Hand.GetPile(Owner);
                CardModel cardModel = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards.Where(cm => cm.MaxUpgradeLevel > cm.CurrentUpgradeLevel && cm.Type != CardType.Status && cm.Type != CardType.Curse));
                if (cardModel != null)
                {
                    CardCmd.Upgrade(cardModel);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(4);
            DynamicVars.Cards.UpgradeValueBy(1);
        }
    }
}