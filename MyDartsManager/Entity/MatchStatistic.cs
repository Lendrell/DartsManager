using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDartsManager.Entity
{
    public class MatchStatistic
    {
        public int ID { get; set; }

        public int MatchID { get; set; }

        public int PlayerID { get; set; }

        public int RoundsPlayed { get; set; }

        public int TotalScore { get; set; }

        public double AverageScorePerRound { get; set; }

        public int HighestScore { get; set; }

        public int Placement { get; set; }

        // Navigation properties
        public virtual Match Match { get; set; }

        public virtual Player Player { get; set; }
    }
}
