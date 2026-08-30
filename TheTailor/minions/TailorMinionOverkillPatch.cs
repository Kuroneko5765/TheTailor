using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MinionLib.Commands;
using MinionLib.Minion;
using TheTailor.Powers;

#pragma warning disable CS8600
namespace TheTailor.Minions
{
    /// <summary>
    ///     A clone of MinionGuardianOverkillPatch which actually works on Multiplayer :) im tired
    /// </summary>
    [HarmonyPatch(typeof(CreatureCmd), "Damage", new Type[]
    {
        typeof(PlayerChoiceContext),
        typeof(IEnumerable<Creature>),
        typeof(decimal),
        typeof(ValueProp),
        typeof(Creature),
        typeof(CardModel),
        typeof(CardPlay)
    })]
    public static class TailorMinionOverkillPatch
    {
        private static readonly AsyncLocal<bool> IsHandling = new AsyncLocal<bool>();
        public static readonly AsyncLocal<Creature?> SuppressedOwner = new AsyncLocal<Creature>();

        [HarmonyPrefix]
        private static bool Prefix(PlayerChoiceContext choiceContext, IEnumerable<Creature> targets, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay, ref Task<IEnumerable<DamageResult>> __result)
        {
            if (IsHandling.Value)
            {
                return true;
            }
            if (targets.Count() == 1 && !ShouldHandle(targets.First(), props))
            {
                return true;
            }
            if (targets.Any(c => c.IsEnemy))
            {
                return true;
            }

            List<Creature> list = targets.ToList();
            __result = HandleWithOverkillRedirect(choiceContext, list, amount, props, dealer, cardSource, cardPlay);

            return false;
        }

        private static bool ShouldHandle(Creature target, ValueProp props)
        {
            if (!target.IsPlayer || target.Player == null || target.IsDead || target.CombatState == null)
            {
                return false;
            }

            if (!props.HasFlag(ValueProp.Move) || props.HasFlag(ValueProp.Unpowered))
            {
                return false;
            }

            return target.Pets.Any((Creature p) => p.IsAlive && IsFrontGuardian(p));
        }

        private static async Task<IEnumerable<DamageResult>> HandleWithOverkillRedirect(PlayerChoiceContext choiceContext, IReadOnlyList<Creature> targets, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
        {
            IsHandling.Value = true;
            List<DamageResult> ret = new List<DamageResult>();

            foreach(Creature owner in targets)
            {
                if (!ShouldHandle(owner, props))
                {
                    ret.Concat(await CreatureCmd.Damage(choiceContext, owner, amount, props, dealer, cardSource, cardPlay));
                    continue;
                }

                List<uint> guardianOrder = 
                (
                    from p in PetOrderSnapshotManager.GetSnapshot(owner.Player, onlyAlive: false)
                    where IsFrontGuardian(p) && p.CombatId.HasValue
                    select p.CombatId.Value
                ).ToList();

                SuppressedOwner.Value = owner;
                List<DamageResult> initialResults;
                try
                {
                    initialResults = (await CreatureCmd.Damage(choiceContext, owner, amount, props, dealer, cardSource, cardPlay)).ToList();
                }
                finally
                {
                    SuppressedOwner.Value = null;
                }

                DamageResult damageResult = initialResults.FirstOrDefault(delegate (DamageResult r)
                {
                    if (r.Receiver != owner && r.Receiver.PetOwner == owner.Player)
                    {
                        if (!IsFrontGuardian(r.Receiver))
                        {
                            uint? combatId = r.Receiver.CombatId;
                            if (combatId.HasValue)
                            {
                                uint valueOrDefault = combatId.GetValueOrDefault();
                                return guardianOrder.Contains(valueOrDefault);
                            }

                            return false;
                        }

                        return true;
                    }

                    return false;
                });
                if (damageResult == null || damageResult.OverkillDamage <= 0 || !damageResult.Receiver.CombatId.HasValue)
                {
                    ret.Concat(initialResults);
                    continue;
                }

                List<DamageResult> redirectedResults = new List<DamageResult>();
                decimal overkill = damageResult.OverkillDamage;
                uint value = damageResult.Receiver.CombatId.Value;
                ValueProp directProps = props | ValueProp.Unpowered;
                int num2 = guardianOrder.IndexOf(value);
                if (num2 < 0)
                {
                    if (overkill > 0m)
                    {
                        DamageResult item = (await CreatureCmd.Damage(choiceContext, owner, overkill, directProps, dealer, cardSource, cardPlay)).FirstOrDefault() ?? new DamageResult(owner, directProps);
                        redirectedResults.Add(item);
                    }

                    initialResults.AddRange(redirectedResults);
                    ret.Concat(initialResults);
                    continue;
                }

                foreach (uint item3 in guardianOrder.Skip(num2 + 1))
                {
                    if (!(overkill <= 0m))
                    {
                        Creature defender = owner.CombatState.GetCreature(item3);
                        if (defender != null && defender.IsAlive && IsFrontGuardian(defender))
                        {
                            DamageResult damageResult2 = (await CreatureCmd.Damage(choiceContext, defender, overkill, directProps, dealer, cardSource, cardPlay)).FirstOrDefault() ?? new DamageResult(defender, directProps);
                            redirectedResults.Add(damageResult2);
                            overkill = damageResult2.OverkillDamage;
                        }

                        continue;
                    }

                    break;
                }

                if (overkill > 0m)
                {
                    DamageResult item2 = (await CreatureCmd.Damage(choiceContext, owner, overkill, directProps, dealer, cardSource, cardPlay)).FirstOrDefault() ?? new DamageResult(owner, directProps);
                    redirectedResults.Add(item2);
                }

                initialResults.AddRange(redirectedResults);
                ret.Concat(redirectedResults);
                continue;
            }

            IsHandling.Value = false;
            return ret;
        }

        private static bool IsFrontGuardian(Creature creature)
        {
            if (creature.GetPower<TailorMadePower>() != null || creature.GetPower<DieForYouPower>() != null)
            {
                return true;
            }

            return false;
        }
    }
}