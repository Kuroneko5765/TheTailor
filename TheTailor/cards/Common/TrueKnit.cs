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
using MegaCrit.Sts2.Core.Models.Cards;

namespace TheTailor.Cards.Common
{
    [Pool(typeof(TheTailorCardPool))]
    public class TrueKnit() : CustomCardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/trueKnitBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/trueKnitBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/trueKnitBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            CardModel? cardModel = (await CardSelectCmd.FromCombatPile(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 1), context: choiceContext, pile: PileType.Discard.GetPile(Owner), player: Owner)).FirstOrDefault();
            if (cardModel != null)
            {
                await CardPileCmd.Add(cardModel, PileType.Draw, CardPilePosition.Top);
                if (IsUpgraded)
                {
                    CardCmd.Upgrade(cardModel);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}