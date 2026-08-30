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

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class Pincushion() : CustomCardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self), IOnStitchEffect
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Replay", 1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch)];

        public async void OnStitch(CardModel card, CardModel stitchedCard)
        {
            if (card == this && IsMutable && stitchedCard != null)
            {
                stitchedCard.BaseReplayCount += DynamicVars["Replay"].IntValue;
            }
        }
        public async void OnUnstitch(CardModel card)
        {
            StitchCardModifier? cardStitch = this.GetModifier<StitchCardModifier>();
            if (card == this && IsMutable && cardStitch != null && cardStitch.StitchedCard != null)
            {
                cardStitch.StitchedCard.BaseReplayCount -= DynamicVars["Replay"].IntValue;
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}