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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;
using TheTailor;
using TheTailor.Cards;
using TheTailor.Cards.Rare;

namespace TheTailor
{
    [HarmonyPatch]
    internal static class PowerupAnimPatch
    {
        [HarmonyPatch(typeof(CreatureCmd), "TriggerAnim")]
        internal static void Postfix(Creature creature, string triggerName, float waitTime)
        {
            if (triggerName == "PowerUp")
            {
                NCreatureVisuals visuals = creature.GetCreatureNode().Visuals;
                IEnumerable<GpuParticles2D> particles = visuals.GetChildrenRecursive<GpuParticles2D>();
                if (particles != null && particles.Count() > 0)
                {
                    foreach(GpuParticles2D particle in particles)
                    {
                        if (particle.Name == "TailorStringParticles" || particle.Name == "TailorGlowParticles")
                        {
                            particle.Emitting = true;
                        }
                    }
                }
            }
        }
    }
}