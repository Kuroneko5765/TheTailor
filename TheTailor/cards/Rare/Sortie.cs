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
using TheTailor.Minions;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MinionLib.Utilities;
using MinionLib.Commands;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace TheTailor.Cards.Rare
{
    [Pool(typeof(TheTailorCardPool))]
    public class Sortie() : CustomCardModel(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        public override string? CustomPortraitPath => "res://TheTailor/images/card_portraits/sortieBeta.png";
        public override string? PortraitPath => "res://TheTailor/images/card_portraits/sortieBeta.png";
        public override string? BetaPortraitPath => "res://TheTailor/images/card_portraits/sortieBeta.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(24, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            List<Creature> petsToKill = new();

            PetsOrderAccessor accessor = new PetsOrderAccessor(Owner);
            if (accessor != null && accessor.Pets != null)
            {
                for (int i = 0; i < accessor.Pets.Count; i++)
                {
                    if (accessor.Pets[i].Monster is TailorMinion)
                    {
                        petsToKill.Add(accessor.Pets[i]);
                    }
                }
            }

            foreach (Creature creature in petsToKill)
            {
                creature.RemoveAllPowersInternalExcept();
                await CreatureCmd.Kill(creature, true);
            }

            _ = MinionAnimCmd.Rearrange(duration: 0.5f);
            accessor.SetManualRearranged();
            PetOrderSnapshotManager.TakeSnapshot(Owner);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .TargetingAllOpponents(CombatState)
                .WithAttackerFx(() => NMinionDiveBombVfx.Create(Owner.Creature, cardPlay.Target))
                .Execute(choiceContext);
        }

        public override bool ShouldPlay(CardModel card, AutoPlayType autoPlayType)
        {
            if (card.Owner != Owner || card is not Sortie)
            {
                return true;
            }

            if (TailorMinionCmd.GetMinionCount<TailorMinion>(card.Owner) < 3)
            {
                return false;
            }

            return true;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(8m);
        }
    }
}