using BaseLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MinionLib.Commands;

namespace TheTailor.Minions
{
    public static class TailorMinionCmd
    {
        public static bool CanMinionBeAdded(Player owner)
        {   
            if (GetMinionCount(owner) >= 3)
            {
                return false;
            }
            return true;
        }

        public static int GetMinionCount(Player owner)
        {
            int ret = 0;
            if (owner.Creature.Pets.Count > 0)
            {
                foreach (Creature minion in owner.Creature.Pets)
                {
                    if (minion?.Monster is (MinionLeather or MinionCotton or MinionDenim or MinionLinen or MinionSilk or MinionWool))
                    {
                        ret++;
                    }
                }
            }

            return ret;
        }

        public static int GetMinionCount<T>(Player owner)
        {
            int ret = 0;
            if (owner.Creature.Pets.Count > 0)
            {
                foreach (Creature minion in owner.Creature.Pets)
                {
                    if (minion?.Monster is (T))
                    {
                        ret++;
                    }
                }
            }

            return ret;
        }
    }
}