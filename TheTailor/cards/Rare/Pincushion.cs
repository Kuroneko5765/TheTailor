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
    public class Pincushion() : CustomCardModel(3, CardType.Skill, CardRarity.Rare, TargetType.Self), IOnStitchEffect
    {
        public override string Title
        {
            get
            {
                string ret = base.Title;
                if (DynamicVars["PincushionRelatedCards"].IntValue > 0)
                {
                    ret += $" ({DynamicVars["PincushionRelatedCards"].IntValue})";
                }
                return ret;
            }
        }
        public List<CardModel> relatedCards;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/pincushionBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new DynamicVar("Delicate", 2), new DynamicVar("PincushionRelatedCards", 0), new StringVar("CardsString")];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch), HoverTipFactory.Static(StaticHoverTips.Pincushion, DynamicVars["CardsString"])];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            foreach (CardModel card in relatedCards)
            {
                await CardCmd.AutoPlay(choiceContext, card, StitchCardModifier.GetTarget(card, cardPlay.Card.CombatState), AutoPlayType.Default);
                await CardCmd.Exhaust(choiceContext, card, false, true);
            }
        }

        public async void OnStitch(CardModel card, CardModel stitchedCard)
        {
            StitchCardModifier? cardStitch = this.GetModifier<StitchCardModifier>();
            if ((card == this || stitchedCard == this) && IsMutable && cardStitch != null && cardStitch.StitchedCard != null)
            {
                relatedCards.Add(cardStitch.StitchedCard);
                if (relatedCards.Count > 1)
                {
                    ((StringVar)DynamicVars["CardsString"]).StringValue += "\n";
                }
                ((StringVar)DynamicVars["CardsString"]).StringValue += cardStitch.StitchedCard.TitleLocString.GetRawText();
                await CardCmd.Exhaust(new ThrowingPlayerChoiceContext(), cardStitch.StitchedCard);
                DynamicVars["PincushionRelatedCards"].UpgradeValueBy(1);
                // NCard.FindOnTable(this)?.ReloadOverlay();
            }
        }
        public async void OnUnstitch(CardModel card)
        {
            
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }

        protected override void AfterCloned()
        {
            base.AfterCloned();
            relatedCards = new List<CardModel>();
        }
    }

    public class PincushionOverlayAdd
    {
        private static readonly string _scenePath = "res://TheTailor/scenes/cards/overlays/pincushion.tscn";
        public static AddedNode<NCard, PincushionOverlay> PincushionOverlay = new(_scenePath, (card, display) =>
        {
            Node cardContainer = card.GetChild(0);
            cardContainer.AddChild(display);
            display.Visible = card.Model.DynamicVars.ContainsKey("PincushionRelatedCards") && card.Model.DynamicVars["PincushionRelatedCards"].IntValue > 0;
        });
    }

    [HarmonyPatch]
    internal static class PincushionOverlayPatch
    {
        [HarmonyPatch(typeof(NCard), "ReloadOverlay")]
        internal static void Postfix(NCard __instance)
        {
            if (__instance.Model == null)
            {
                return;
            }

            foreach (Node node in __instance.GetChild(0).GetChildren())
            {
                if (node is PincushionOverlay)
                {
                    PincushionOverlay pincushionOverlay = node as PincushionOverlay;
                    pincushionOverlay.Visible = __instance.Model.DynamicVars.ContainsKey("PincushionRelatedCards") && __instance.Model.DynamicVars["PincushionRelatedCards"].IntValue > 0;
                }
            }
        }
    }

    [HarmonyPatch]
    internal static class PincushionHovertipPatch
    {
        [HarmonyPatch(typeof(CardModel), "HoverTips", MethodType.Getter)]
        internal static IEnumerable<IHoverTip> Postfix(IEnumerable<IHoverTip> __result, CardModel __instance)
        {
            if (__instance is Pincushion && __instance.DynamicVars["PincushionRelatedCards"].IntValue <= 0)
            {
                __result = [HoverTipFactory.FromKeyword(TheTailor.Keywords.Stitch), HoverTipFactory.FromKeyword(CardKeyword.Retain), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
            }

            return __result;
        }
    }

}