using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace MyDartsManager.Entity
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MatchStatistic> MatchStatistics { get; set; }
        public virtual ICollection<TrainingSession> TrainingSessions { get; set; }

        public virtual ICollection<Round> Rounds { get; set; }
        public Player(string name)
        {
            Name = name;
        }
    }
}
