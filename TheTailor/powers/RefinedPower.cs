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
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheTailor.Powers
{
    public sealed class RefinedPower : CustomPowerModel
    {
        private class Data
        {
            public int etherealCount;
        }
        protected override object InitInternalData()
        {
            return new Data();
        }
        public override string? CustomPackedIconPath => "res://TheTailor/images/powers/refinedSmall.png";
        public override string? CustomBigIconPath => "res://TheTailor/images/powers/refined.png";
        public override string? CustomBigBetaIconPath => "res://TheTailor/images/powers/refined.png";
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
        {
            if (card.Owner.Creature == Owner)
            {
                Data data = GetInternalData<Data>();
                if (data.etherealCount < Amount)
                {
                    if (causedByEthereal)
                    {
                        GetInternalData<Data>().etherealCount++;
                    }
                    else
                    {
                        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner.Player);
                        await PowerCmd.Decrement(this);
                    }
                }
            }
        }

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner))
            {
                Data data = GetInternalData<Data>();
                for (int i = 0; i < data.etherealCount; i++)
                {
                    await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner.Player);
                    await PowerCmd.Decrement(this);
                }
                data.etherealCount = 0;
            }
        }
    }
}