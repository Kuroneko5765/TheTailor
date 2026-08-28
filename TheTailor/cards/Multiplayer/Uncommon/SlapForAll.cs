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

namespace TheTailor.Cards.Multiplayer.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class SlapForAll() : CustomCardModel(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/slapForAllBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/slapForAllBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/slapForAllBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(12m),
            new ExtraDamageVar(4m),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier
            (
                (CardModel card, Creature? _) => GetAllEligible(card)
            )
        ];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .Targeting(cardPlay.Target)
                .FromCard(this, cardPlay)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        public static int GetAllEligible(CardModel card)
        {
            int ret = 0;

            foreach (Creature creature in card.CombatState.GetCreaturesOnSide(CombatSide.Player))
            {
                if (creature.IsPlayer)
                {
                    if (creature != card.Owner.Creature)
                    {
                        ret++;
                    }

                    foreach (Creature pet in creature.Pets)
                    {
                        ret++;
                    }
                }
            }

            return ret;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(2);
        }
    }
}