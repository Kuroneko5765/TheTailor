using System.Collections.Generic;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Minion;
using MinionLib.Powers.Patches;
using HarmonyLib;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Entities.Players;
using TheTailor.Minions;
using MegaCrit.Sts2.Core.Localization;

/*
namespace TheTailor
{
    public static class Debug
    {
        [HarmonyPatch(typeof(LocTable))]
        internal static class LocStringPatch
        {
            [HarmonyPatch("GetLocString")]
            [HarmonyPrefix]
            public static bool Prefix(LocTable __instance, string key, string ____name, ref LocString __result)
            {
                Log.Warn($"GetLocString:'{key}' from '{____name}'");
                return true;
            }

            [HarmonyPatch("GetRawText")]
            [HarmonyPrefix]
            public static bool Prefix(LocTable __instance, string key, string ____name, ref string __result)
            {
                Log.Warn($"GetLocString:'{key}' from '{____name}'");
                return true;
            }
        }
    }
}
*/