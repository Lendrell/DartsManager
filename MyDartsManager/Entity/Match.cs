using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MyDartsManager.Entity
{

    public class Match
    {
        public int MatchId { get; set; }
        public DateTime Date { get; set; }
        public int ScoreGoal { get; set; }

        public bool DoubleOut { get; set; }

        public virtual ICollection<MatchStatistic> MatchStatistics { get; set; }

        public virtual ICollection<Round> Rounds { get; set; }
        public Match(int scoreGoal, bool doubleOut)
        {
            Date = DateTime.Now;
            ScoreGoal = scoreGoal;
            DoubleOut = doubleOut;
        }
    }
}
