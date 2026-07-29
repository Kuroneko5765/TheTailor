using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Extensions;
using BaseLib.Abstracts;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;
using TheTailor;

namespace TheTailor.Cards
{
    public static class StitchCmd
    {
        public static List<CardType> UnstitchableTypes = new List<CardType> { CardType.Status, CardType.Curse, CardType.Quest, CardType.None };

        public static async Task StitchCards(CardModel card1, CardModel card2)
        {
            if (UnstitchableTypes.Contains(card1.Type) || UnstitchableTypes.Contains(card2.Type))
            {
                Log.Error("Attempted to stitch 2 unstitchable cards; these should be filtered by StitchCmd.CanBeStitched!");
                return;
            }

            CardModifier.AddModifier<StitchCardModifier>(card1);
            CardModifier.AddModifier<StitchCardModifier>(card2);
            StitchCardModifier? card1Stitch = card1.GetModifier<StitchCardModifier>();
            StitchCardModifier? card2Stitch = card2.GetModifier<StitchCardModifier>();
            card1Stitch.StitchedCard = card2;
            card2Stitch.StitchedCard = card1;
            if (card1 is IOnStitchEffect) { (card1 as IOnStitchEffect).OnStitch(); }
            if (card2 is IOnStitchEffect) { (card2 as IOnStitchEffect).OnStitch(); }
            NCard.FindOnTable(card1)?.ReloadOverlay();
            NCard.FindOnTable(card2)?.ReloadOverlay();
        }

        public static async Task UnstitchCard(CardModel card1)
        {
            if (card1 == null)
            {
                return;
            }

            StitchCardModifier? cardStitch1 = card1.GetModifier<StitchCardModifier>();
            if (cardStitch1 != null)
            {
                card1.RemoveKeyword(Keywords.Stitched);
                CardModifier.RemoveModifier(card1, cardStitch1);
                if (card1 is IOnStitchEffect) { (card1 as IOnStitchEffect).OnUnstitch(); }
                NCard.FindOnTable(card1)?.ReloadOverlay();
            }
        }

        public static async Task UnstitchRelatedCard(CardModel card1)
        {
            if (card1 == null)
            {
                return;
            }

            StitchCardModifier? cardStitch1 = card1.GetModifier<StitchCardModifier>();
            if (cardStitch1 != null)
            {
                CardModel? card2 = cardStitch1.StitchedCard;
                if (card2 != null && card2.IsInCombat && card2.Pile != null)
                {
                    StitchCardModifier? cardStitch2 = card2.GetModifier<StitchCardModifier>();
                    if (cardStitch2 != null)
                    {
                        card2.RemoveKeyword(Keywords.Stitched);
                        CardModifier.RemoveModifier(card2, cardStitch2);
                        if (card2 is IOnStitchEffect) { (card2 as IOnStitchEffect).OnUnstitch(); }
                        NCard.FindOnTable(card2)?.ReloadOverlay();
                    }
                }
            }
        }

        public static bool CanBeStitched(CardModel cardModel)
        {
            bool ret = cardModel.GetModifier<StitchCardModifier>() == null
            && !cardModel.Keywords.Contains(CardKeyword.Unplayable)
            && !UnstitchableTypes.Contains(cardModel.Type)
            && cardModel.IsMutable;

            return ret;
        }
    }
}