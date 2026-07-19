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
using TheTailor.Cards.Token;
using BaseLib.Extensions;

namespace TheTailor.Cards.Common
{
    [Pool(typeof(TheTailorCardPool))]
    public class ReflexStrike() : CustomCardModel(0, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/reflexStrikeBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/reflexStrikeBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/reflexStrikeBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move), new CardsVar(1), new DynamicVar("Hits", 1)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .TargetingRandomOpponents(CombatState)
                .WithHitCount(DynamicVars["Hits"].IntValue)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        }

        public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
        {
            if (card == this && card is ReflexStrike && card.Pile == PileType.Hand.GetPile(Owner))
            {
                if (Owner.HasPower<HellraiserPower>() && oldPileType == PileType.Draw)
                {
                    return;
                }
                await CardCmd.AutoPlay(new ThrowingPlayerChoiceContext(), card, null, AutoPlayType.Default);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Hits"].UpgradeValueBy(1);
        }
    }
}