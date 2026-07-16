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

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class OnlyTheFinest() : CustomCardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        public override bool GainsBlock => true;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/onlyTheFinestBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/onlyTheFinestBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/onlyTheFinestBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            foreach (CardModel item in GetCards().ToList())
            {
                await CardCmd.Exhaust(choiceContext, item);
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(1);
        }

        private IEnumerable<CardModel> GetCards()
        {
            CardPile pile = PileType.Hand.GetPile(Owner);
            return pile.Cards.Where((CardModel c) => !c.IsUpgraded);
        }
    }
}