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

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class GreatExpectations() : CustomCardModel(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Minion };
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/greatExpectationsBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/greatExpectationsBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/greatExpectationsBeta.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheTailor.Keywords.LeatherMinion), HoverTipFactory.FromKeyword(TheTailor.Keywords.DenimMinion), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            List<Creature> petsToKill = new();

            PetsOrderAccessor accessor = new PetsOrderAccessor(Owner);
            if (accessor != null && accessor.Pets != null)
            {
                for (int i = 0; i < accessor.Pets.Count; i++)
                {
                    if (accessor.Pets[i].Monster is MinionLeather)
                    {
                        petsToKill.Add(accessor.Pets[i]);
                    }
                }
            }

            int replaceAmount = petsToKill.Count;

            foreach (Creature creature in petsToKill)
            {
                creature.RemoveAllPowersInternalExcept();
                await CreatureCmd.Kill(creature, true);
            }

            for (int i = 0; i < replaceAmount; i++)
            {
                await TailorMinionCmd.AddMinion<MinionDenim>(choiceContext, Owner);   
            }

            _ = MinionAnimCmd.Rearrange(duration: 0.5f);
            accessor.SetManualRearranged();
            PetOrderSnapshotManager.TakeSnapshot(Owner);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}