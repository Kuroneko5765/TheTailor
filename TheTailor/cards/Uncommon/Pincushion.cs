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
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Cards;
using Godot;
using TheTailor.Cards.Common;
using TheTailor.Powers;

namespace TheTailor.Cards.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class Pincushion() : CustomCardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move), new DynamicVar("Hits", 2)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PinprickPower>(), HoverTipFactory.FromPower<PincushionPower>()];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<PinprickPower>(choiceContext, cardPlay.Target, 1, Owner.Creature, this);
            await PowerCmd.Apply<PincushionPower>(choiceContext, cardPlay.Target, 1, Owner.Creature, this);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .WithHitCount(DynamicVars["Hits"].IntValue)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2);
        }
    }
}