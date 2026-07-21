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

namespace TheTailor.Relics.Uncommon
{
    [Pool(typeof(TheTailorRelicPool))]
    public class SatinGlove : CustomRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;
        public override string PackedIconPath => "res://TheTailor/images/relics/satinGlove.png";
        protected override string PackedIconOutlinePath => "res://TheTailor/images/relics/satinGloveOutline.png";
        protected override string BigIconPath => "res://TheTailor/images/relics/satinGloveBig.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Strength", 1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(Keywords.Delicate), HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromPower<StrengthPower>()];

        public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
        {
            if (card.Owner == Owner && card.DynamicVars.ContainsKey("Delicate") && card.DynamicVars["Delicate"].BaseValue >= 0)
            {
                Flash();
                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars["Strength"].IntValue, Owner.Creature, null);
            }
        }
    }
}