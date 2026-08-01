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
using MegaCrit.Sts2.Core.Combat;
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
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;
using TheTailor;
using TheTailor.Cards;
using TheTailor.Cards.Rare;

namespace TheTailor.Cards
{
    public class StitchCardModifier : CardModifier
    {
        private CardModel? _stitchedCard;
        public CardModel? StitchedCard
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

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (!StitchTrackAutoplaySingleton.BlockedFromAutoplay.Contains(cardPlay.Card) && cardPlay.Card == Owner && Owner != null && StitchedCard != null)
            {
                Creature? target = GetTarget(StitchedCard, StitchedCard.CombatState);
                if (cardPlay.Target != null && cardPlay.Target.IsAlive && target != null)
                {
                    target = cardPlay.Target;
                }

                await CardCmd.AutoPlay(choiceContext, StitchedCard, target, StitchedAutoPlayType.Stitched);
            }
        }

        public static Creature? GetTarget(CardModel card, ICombatState combatState)
        {
            Rng combatTargets = card.Owner.RunState.Rng.CombatTargets;
            return card.TargetType switch
            {
                TargetType.AnyEnemy => combatState.HittableEnemies.FirstOrDefault(),
                TargetType.AnyAlly => combatTargets.NextItem(combatState.Allies.Where((Creature c) => c != null && c.IsAlive && c.IsPlayer && c != card.Owner.Creature)),
                TargetType.AnyPlayer => card.Owner.Creature,
                _ => null,
            };
        }
    }

    [HarmonyPatch]
    internal static class StitchHovertipPatch
    {
        [HarmonyPatch(typeof(CardModel), "HoverTips", MethodType.Getter)]
        internal static IEnumerable<IHoverTip> Postfix(IEnumerable<IHoverTip> __result, CardModel __instance)
        {
            StitchCardModifier? cardStitch = __instance.GetModifier<StitchCardModifier>();
            if (cardStitch != null && cardStitch.StitchedCard != null && __instance.IsMutable)
            {
                __result = [.. __result, .. new IHoverTip[1] { new CardHoverTip(cardStitch.StitchedCard) }];
            }

            if (__instance is Pincushion && __instance.IsMutable)
            {
                Pincushion pincushion = __instance as Pincushion;
                foreach(CardModel card in pincushion.relatedCards)
                {
                    __result = [.. __result, .. new IHoverTip[1] { new CardHoverTip(card) }];
                }
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
                if (cardStitch.StitchedCard == null || !cardStitch.StitchedCard.IsInCombat || cardStitch.StitchedCard.Pile == null)
                {
                    await StitchCmd.UnstitchCard(card);
                }
                else if (card.Pile.Type == PileType.Exhaust || cardStitch.StitchedCard.Pile.Type == PileType.Exhaust)
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