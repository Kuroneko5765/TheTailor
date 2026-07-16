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

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class Cheeseboard() : CustomCardModel(-1, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        public override int MaxUpgradeLevel => 99999;
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/cheeseboardBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/cheeseboardBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/cheeseboardBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Vigor", 1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.Premium), HoverTipFactory.FromPower<VigorPower>()];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable, CardKeyword.Retain];

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (side == CombatSide.Player)
            {
                foreach (Creature creature in participants)
                {
                    if (creature == Owner.Creature && PileType.Hand.GetPile(Owner).Cards.Contains(this))
                    {
                        await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["Vigor"].IntValue, Owner.Creature, this);
                    }
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Vigor"].UpgradeValueBy(1);
        }
    }
}