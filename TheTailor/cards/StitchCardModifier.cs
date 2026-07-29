using System.Collections.ObjectModel;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Patches.Localization;
using BaseLib.Patches.Saves;
using BaseLib.Utils;
using BaseLib.Utils.Patching;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Saves.Runs;
using TheTailor;
using TheTailor.Cards;

namespace TheTailor.Cards
{
    public class StitchCardModifier : CardModifier
    {
        private CardModel _stitchedCard;
        public CardModel StitchedCard
        {
            get
            {
                return _stitchedCard;
            }
            set
            {
                if (_stitchedCard != null)
                {
                    Log.Error("Card cannot be stitched twice");
                    return;
                }
                if (value == null)
                {
                    Log.Error("Attempt to stitch null card");
                    return;
                }

                _stitchedCard = value;
                _stitchedCard.AddKeyword(Keywords.Stitched);
            }
        }

        public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (!cardPlay.IsAutoPlay && StitchedCard != null && StitchedCard.IsInCombat && StitchedCard.Pile != null)
            {
                Creature? target = cardPlay.Target is { IsAlive: true } ? cardPlay.Target : null;
                await CardCmd.AutoPlay(choiceContext, StitchedCard, target, AutoPlayType.Default);

                if (Owner.Type == CardType.Power || Owner.Keywords.Contains(CardKeyword.Exhaust))
                {
                    await StitchCmd.UnstitchCard(StitchedCard);
                }
            }
        }
    }

    [HarmonyPatch]
    internal static class StitchHovertipPatch
    {
        [HarmonyPatch(typeof(CardModel), "HoverTips", MethodType.Getter)]
        internal static IEnumerable<IHoverTip> Postfix(IEnumerable<IHoverTip> __result, CardModel __instance)
        {
            StitchCardModifier? cardStitch = __instance.GetModifier<StitchCardModifier>();
            if (cardStitch != null && cardStitch.StitchedCard != null)
            {
                return [.. __result, .. new IHoverTip[1] { new CardHoverTip(cardStitch.StitchedCard) }];
            }
            return __result;
        }
    }

    [HarmonyPatch]
    internal static class StitchRemovePatch
    {
        [HarmonyPatch(typeof(AbstractModel), "AfterCardChangedPiles")]
        internal static async void Postfix(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
        {
            if (card == null || card.Pile == null)
            {
                return;
            }

            StitchCardModifier? cardStitch = card.GetModifier<StitchCardModifier>();
            if (cardStitch != null)
            {
                if (cardStitch.StitchedCard == null || !cardStitch.StitchedCard.IsInCombat || cardStitch.StitchedCard.Pile == null || cardStitch.StitchedCard.Pile.Type == PileType.Exhaust)
                {
                    await StitchCmd.UnstitchCard(card);
                }
                else if (card.Pile.Type == PileType.Exhaust)
                {
                    await StitchCmd.UnstitchRelatedCard(card);
                    await StitchCmd.UnstitchCard(card);
                }
            }
        }
    }

    public class StitchOverlayAdd
    {
        private static readonly string _scenePath = "res://TheTailor/scenes/cards/overlays/stitch.tscn";
        public static AddedNode<NCard, StitchOverlay> StitchOverlay = new(_scenePath, (card, display) =>
        {
            Node cardContainer = card.GetChild(0);
            cardContainer.AddChild(display);
            display.Visible = card.Model?.GetModifier<StitchCardModifier>() != null;
        });
    }

    [HarmonyPatch]
    internal static class StitchOverlayPatch
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
                if (node is StitchOverlay)
                {
                    StitchOverlay stitchOverlay = node as StitchOverlay;
                    stitchOverlay.Visible = __instance.Model.GetModifier<StitchCardModifier>() != null;
                }
            }
        }
    }
}