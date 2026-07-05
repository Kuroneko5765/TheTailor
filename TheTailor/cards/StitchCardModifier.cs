using System.Collections.ObjectModel;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Patches.Localization;
using BaseLib.Patches.Saves;
using BaseLib.Utils;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
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
            if (!cardPlay.IsAutoPlay)
            {
                Creature? target = cardPlay.Target is { IsAlive: true } ? cardPlay.Target : null;
                await CardCmd.AutoPlay(choiceContext, StitchedCard, target, AutoPlayType.Default);
            }
        }
    }

    [HarmonyPatch]
    internal static class StitchHovertipPatch
    {
        [HarmonyPatch(typeof(CardModel), "HoverTips", MethodType.Getter)]
        internal static IEnumerable<IHoverTip> Postfix(IEnumerable<IHoverTip> __result, CardModel __instance)
        {
            StitchCardModifier cardStitch = __instance.GetModifier<StitchCardModifier>();
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
        [HarmonyPatch(typeof(AbstractModel), "AfterCardPlayedLate")]
        internal static async void Postfix(PlayerChoiceContext choiceContext, CardPlay cardPlay, AbstractModel __instance)
        {
            StitchCardModifier cardStitch = cardPlay.Card.GetModifier<StitchCardModifier>();
            if (cardStitch != null)
            {
                if (cardStitch.StitchedCard == null || !cardStitch.StitchedCard.IsInCombat || cardStitch.StitchedCard.Pile == null || cardStitch.StitchedCard.Pile.Type == PileType.Exhaust)
                {
                    await StitchCmd.UnstitchCard(cardPlay.Card);
                }
            }
        }
    }

    [HarmonyPatch]
    internal static class StitchOverlayPatch
    {
        [HarmonyPatch(typeof(CardModel), "OverlayPath", MethodType.Getter)]
        internal static string Postfix(string __result, CardModel __instance)
        {
            StitchCardModifier cardStitch = __instance.GetModifier<StitchCardModifier>();
            if (__instance.Affliction == null && cardStitch != null)
            {
                __result = "res://TheTailor/scenes/cards/overlays/stitch.tscn";
            }

            return __result;
        }

        [HarmonyPatch(typeof(CardModel), "HasBuiltInOverlay", MethodType.Getter)]
        internal static bool Postfix(bool __result, CardModel __instance)
        {
            StitchCardModifier cardStitch = __instance.GetModifier<StitchCardModifier>();
            if (cardStitch != null)
            {
                __result = true;
            }
            return __result;
        }
    }
}