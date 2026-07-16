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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;

namespace TheTailor.Cards.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class CoupDeGrace() : CustomCardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/coupDeGraceBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/coupDeGraceBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/coupDeGraceBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(8m),
            new ExtraDamageVar(4m),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.HappenedThisTurn(card.CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner == card.Owner))
        ];
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(this, cardPlay)
                .WithHitCount(DynamicVars["Hits"].IntValue)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(2);
        }
    }
}