using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDartsManager.Entity
{
    public class Round
    {

        public int RoundId { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int RoundNumber { get; set; }

        public int ThrowCombinationId1 { get; set; }
        public int ThrowCombinationId2 { get; set; }
        public int ThrowCombinationId3 { get; set; }

        public virtual ThrowCombination Throw1 { get; set; }
        public virtual ThrowCombination Throw2 { get; set; }
        public virtual ThrowCombination Throw3 { get; set; }
        public virtual Match Match { get; set; }
        public virtual Player Player { get; set; }

        public Round()
        {
        }

        public Round(int roundNumber, Match match, Player player, ThrowCombination throw1, ThrowCombination throw2, ThrowCombination throw3)
        {
            RoundNumber = roundNumber;
            Match = match;
            Player = player;
            Throw1 = throw1;
            Throw2 = throw2;
            Throw3 = throw3;
        }

        public int RoundValue()
        {
            return Throw1.TrueValue() + Throw2.TrueValue() + Throw3.TrueValue();
        }

    }
}
