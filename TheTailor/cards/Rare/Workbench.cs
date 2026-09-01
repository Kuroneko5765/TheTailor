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
using TheTailor.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class Workbench() : CustomCardModel(-1, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        public override int MaxUpgradeLevel => 99999;
        public override bool GainsBlock => true;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/workbenchBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/workbenchBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/workbenchBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(1, ValueProp.Unpowered)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Premium)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Retain];

        public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Player)
            {
                foreach (Creature creature in participants)
                {
                    if (creature == Owner.Creature && PileType.Hand.GetPile(Owner).Cards.Contains(this))
                    {
                        int fullBlock = DynamicVars.Block.IntValue;
                        if (Enchantment is Nimble)
                        {
                            fullBlock += Enchantment.Amount;
                        }
                        await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(fullBlock, ValueProp.Unpowered), null);
                    }
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(1);
        }
    }
}