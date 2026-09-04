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
using TheTailor.Minions;
using MinionLib.Commands;
using MinionLib.Minion;
using TheTailor.Character;
using MinionLib.Utilities;
using MegaCrit.Sts2.Core.Entities.Creatures;
using TheTailor.Powers;

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class Bolster() : CustomCardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Minion };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/bolsterBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/bolsterBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/bolsterBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Power", 1)];
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.BurlapMinion)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await TailorMinionCmd.AddOrReplaceMinion<MinionBurlap>(choiceContext, Owner, true);

            PetsOrderAccessor accessor = new PetsOrderAccessor(Owner);
            if (accessor != null && accessor.Pets != null && accessor.Pets.Count > 0)
            {
                foreach (Creature creature in accessor.Pets)
                {
                    if (creature.Monster is MinionLinen)
                    {
                        await PowerCmd.Apply<LinenPower>(choiceContext, creature, DynamicVars["Power"].IntValue, Owner.Creature, this);
                    }
                    else if (creature.Monster is MinionCotton)
                    {
                        await PowerCmd.Apply<CottonPower>(choiceContext, creature, DynamicVars["Power"].IntValue, Owner.Creature, this);
                    }
                    else if (creature.Monster is MinionDenim)
                    {
                        await PowerCmd.Apply<DenimPower>(choiceContext, creature, DynamicVars["Power"].IntValue, Owner.Creature, this);
                    }
                    else if (creature.Monster is MinionWool)
                    {
                        await PowerCmd.Apply<WoolPower>(choiceContext, creature, DynamicVars["Power"].IntValue, Owner.Creature, this);
                    }
                    else if (creature.Monster is MinionSilk)
                    {
                        await PowerCmd.Apply<SilkPower>(choiceContext, creature, DynamicVars["Power"].IntValue, Owner.Creature, this);
                    }
                    else if (creature.Monster is MinionBurlap)
                    {
                        await PowerCmd.Apply<BurlapPower>(choiceContext, creature, DynamicVars["Power"].IntValue, Owner.Creature, this);
                    }
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Power"].UpgradeValueBy(1);
        }
    }
}