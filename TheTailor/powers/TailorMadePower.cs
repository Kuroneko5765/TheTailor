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

namespace TheTailor.Powers
{
    public sealed class TailorMadePower : CustomPowerModel
    {
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/tailorMadeSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/tailorMade.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/tailorMade.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;

        public override Creature ModifyUnblockedDamageTarget(Creature target, decimal amount, ValueProp props, Creature? dealer)
        {
            if (base.Owner.Monster is MinionModel { Position: not MinionPosition.Front })
            {
                return target;
            }

            if (target != base.Owner.PetOwner?.Creature)
            {
                bool flag = true;
                if (target.PetOwner == base.Owner.PetOwner && base.Owner.PetOwner != null && target.GetPower<TailorMadePower>() != null)
                {
                    IReadOnlyList<Creature> pets = target.PetOwner.PlayerCombatState.Pets;
                    if (pets.IndexOf(base.Owner) < pets.IndexOf(target))
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    return target;
                }
            }

            if (base.Owner.IsDead)
            {
                return target;
            }

            if (!props.HasFlag(ValueProp.Move) || props.HasFlag(ValueProp.Unpowered))
            {
                return target;
            }

            return base.Owner;
        }
    }

    [HarmonyPatch]
    internal static class MinionLayoutPatch
    {
        [HarmonyPatch(typeof(MinionGuardianOverkillPatch), "IsFrontGuardian")]
        internal static void Postfix(ref bool __result, Creature creature)
        {
            if (__result == false)
            {
                if (creature.GetPower<TailorMadePower>() != null)
                {
                    if (creature.Monster is MinionModel minionModel)
                    {
                        __result = minionModel.Position == MinionPosition.Front;
                        return;
                    }

                    __result = true;
                    return;
                }
            }
        }
    }
}