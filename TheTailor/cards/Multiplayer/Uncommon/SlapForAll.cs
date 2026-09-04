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
using TheTailor.Character;
using TheTailor.Cards.Token;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders;
using TheTailor.Cards.Uncommon;
using TheTailor.Cards.Common;

namespace TheTailor.Cards.Multiplayer.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class SlapForAll() : CustomCardModel(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        public override int MaxUpgradeLevel => 99999;
        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/slapForAllBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/slapForAllBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/slapForAllBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move), new DynamicVar("Slaps", 2)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<SingleSlap>(), HoverTipFactory.FromKeyword(TheTailor.Keywords.Premium)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .Targeting(cardPlay.Target)
                .FromCard(this, cardPlay)
                .WithHitCount(DynamicVars["Slaps"].IntValue)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            IEnumerable<Creature> enumerable = from c in CombatState.GetTeammatesOf(Owner.Creature)
                                               where c != null && c.IsAlive && c.IsPlayer && c != Owner.Creature
                                               select c;
            foreach (Creature teammate in enumerable)
            {
                CardModel card = CombatState.CreateCard<SingleSlap>(teammate.Player);
                card.AddKeyword(CardKeyword.Ethereal);
                await CardPileCmd.AddGeneratedCardsToCombat([card], PileType.Hand, teammate.Player);
                await Cmd.Wait(0.1f);

                foreach (CardModel item in PileType.Hand.GetPile(teammate.Player).Cards.Where((CardModel c) => c.IsUpgradable && (c is SingleSlap || c is UltimateSlap || c is SlapForAll || c is DisarmingSlap)))
                {
                    CardCmd.Upgrade(item);
                }
            }

            CardCmd.Upgrade(this);

            foreach (CardModel item in PileType.Hand.GetPile(Owner).Cards.Where((CardModel c) => c.IsUpgradable && (c is SingleSlap || c is UltimateSlap || c is SlapForAll || c is DisarmingSlap)))
            {
                CardCmd.Upgrade(item);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Slaps"].UpgradeValueBy(1);
        }
    }
}