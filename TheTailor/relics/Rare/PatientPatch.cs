using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MinionLib.Commands;
using TheTailor;
using TheTailor.Minions;
using MinionLib.Minion;
using TheTailor.Character;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using TheTailor.Cards;
using MegaCrit.Sts2.Core.CardSelection;
using TheTailor.Cards.Token;

namespace TheTailor.Relics.Rare
{
    [Pool(typeof(TheTailorRelicPool))]
    public class PatientPatch : CustomRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Rare;
        public override string PackedIconPath => "res://TheTailor/images/relics/patientPatch.png";
        protected override string PackedIconOutlinePath => "res://TheTailor/images/relics/patientPatchOutline.png";
        protected override string BigIconPath => "res://TheTailor/images/relics/patientPatchBig.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Patch>(), HoverTipFactory.FromKeyword(CardKeyword.Retain)];

        private int _uses = 2;
        public int Uses
        {
            get
            {
                return _uses;
            }
            private set
            {
                AssertMutable();
                _uses = value;
            }
        }

        public override Task AfterCardEnteredCombat(CardModel card)
        {
            if (card.Owner != Owner || card is not Patch)
            {
                return Task.CompletedTask;
            }
            CardCmd.ApplyKeyword(card, CardKeyword.Retain);
            return Task.CompletedTask;
        }

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
        {
            if (CombatManager.Instance.IsInProgress && card.Owner == Owner && Uses > 0)
            {
                Flash();
                await Patch.CreateInHand(Owner, 1, card.CombatState);

                Uses--;
            }
        }

        public override Task AfterCombatEnd(CombatRoom _)
        {
            Uses = DynamicVars.Cards.IntValue;
            return Task.CompletedTask;
        }
    }
}