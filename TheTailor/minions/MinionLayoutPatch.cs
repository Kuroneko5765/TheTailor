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
using HarmonyLib;
using MinionLib.Layout;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Godot;
using MegaCrit.Sts2.Core.Logging;

namespace TheTailor.Minions
{
    [HarmonyPatch]
    internal static class MinionLayoutPatch
    {
        [HarmonyPatch(typeof(DefaultMinionLayout), "GenerateGridPoints")]
        internal static void Postfix(ref IReadOnlyList<Vector2> __result, DefaultMinionLayout __instance)
        {
            // TODO does not discriminate between The Tailor and other characters' minions if they're also using MinionLib
            // - May have to request this as a feature to MinionLib, or create a separate patch/singleton

            List<Vector2> newList = new();
            foreach (var vector in __result)
            {
                newList.Add(new Vector2((vector.X * 1.6f) - 0f, 0f));
            }

            __result = newList;
        }
    }
}