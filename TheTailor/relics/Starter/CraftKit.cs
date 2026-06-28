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

namespace TheTailor.Relics.Starter
{
    [Pool(typeof(TheTailorRelicPool))]
    public class CraftKit : CustomRelicModel
    {
        public override RelicRarity Rarity => RelicRarity.Starter;
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(Keywords.LeatherMinion)];
        public override bool SpawnsPets => true;
        public override string PackedIconPath => "res://TheTailor/images/relics/craftKit.png";
        protected override string PackedIconOutlinePath => "res://TheTailor/images/relics/craftKitOutline.png";
        protected override string BigIconPath => "res://TheTailor/images/relics/craftKitBig.png";
        public override async Task BeforeCombatStart()
        {
            await MinionCmd.AddMinion<MinionLeather>(new ThrowingPlayerChoiceContext(), Owner, new MinionSummonOptions(Position: MinionPosition.Front));
        }
    }
}