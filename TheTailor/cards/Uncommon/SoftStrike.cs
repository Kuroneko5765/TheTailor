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
using HarmonyLib;
using MinionLib.Utilities;
using MegaCrit.Sts2.Core.Entities.Creatures;
using TheTailor.Powers;

namespace TheTailor.Cards.Uncommon
{
    [Pool(typeof(TheTailorCardPool))]
    public class SoftStrike() : CustomCardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Minion, CardTag.Minion };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/softStrikeBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/softStrikeBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/softStrikeBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.WoolMinion)];
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (await TailorMinionCmd.AddOrReplaceMinion<MinionWool>(choiceContext, Owner, true))
            {
                PetsOrderAccessor accessor = new PetsOrderAccessor(Owner);
                if (accessor != null && accessor.Pets != null && accessor.Pets.Count > 0)
                {
                    foreach (Creature creature in accessor.Pets)
                    {
                        if (creature.Monster is MinionWool)
                        {
                            for (int i = 0; i < (IsUpgraded ? 2 : 1); i++)
                            {
                                await PowerCmd.Apply<WoolWeakPower>(choiceContext, Owner.Creature, 1m, creature, null);
                                await CreatureCmd.TriggerAnim(creature, "cast", 0f);
                                await Cmd.Wait(0.25f);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}