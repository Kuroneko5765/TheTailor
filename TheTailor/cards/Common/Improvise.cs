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
using TheTailor.Character;
using MinionLib.Utilities;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Immutable;

namespace TheTailor.Cards.Common
{
    [Pool(typeof(TheTailorCardPool))]
    public class Improvise() : CustomCardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/improviseBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/improviseBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/improviseBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => IsUpgraded ? [HoverTipFactory.FromCard<Token.Patch>()] : [];
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Delicate", 3), new CardsVar(1), new BlockVar(2, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
            if (IsUpgraded)
            {
                IEnumerable<CardModel> cm = await Token.Patch.CreateInHand(Owner, 1, CombatState);
            }
            await TailorMinionCmd.AddOrReplaceMinion<MinionLeather>(choiceContext, Owner, true);
        }
    }
}