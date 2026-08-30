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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Monsters;

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
            if (Owner.Monster is MinionModel { Position: not MinionPosition.Front })
            {
                return target;
            }

            if (target != Owner.PetOwner?.Creature)
            {
                bool flag = true;
                if (target.PetOwner == Owner.PetOwner && Owner.PetOwner != null && (target.GetPower<TailorMadePower>() != null || target.GetPower<DieForYouPower>() != null))
                {
                    IReadOnlyList<Creature> pets = target.PetOwner.PlayerCombatState.Pets;
                    if (pets.IndexOf(Owner) < pets.IndexOf(target))
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    return target;
                }
            }

            if (Owner.IsDead)
            {
                return target;
            }

            if (!props.HasFlag(ValueProp.Move) || props.HasFlag(ValueProp.Unpowered))
            {
                return target;
            }

            return Owner;
        }
    }
}