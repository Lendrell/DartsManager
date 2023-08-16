using MyDartsManager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDartsManager.DataStructure
{
    public class PlayerStats
    {
        public int MatchesPlayed { get; private set; }
        public int MatchesWon { get; private set; }

        public int AvgScorePerRound { get; private set; }

        public double ThrowsToHitTarget { get; private set; }


        public PlayerStats(int PlayerId) 
        {
            DartsDbContext db = new DartsDbContext();
            Player player = db.Players.Find(PlayerId);
            List<MatchStatistic> matchStats = player.MatchStatistics.ToList();
            List<PracticeTarget> practiceStats = new List<PracticeTarget>();

            foreach(TrainingSession ts in player.TrainingSessions)
            {
                practiceStats.AddRange(ts.PracticeTargets.ToList());
            }


            ProcessMatchStats(matchStats);
            ProcessPracticeStats(practiceStats);
        }

        private void ProcessMatchStats(List<MatchStatistic> matchStats)
        {
            MatchesPlayed = matchStats.Count;
            MatchesWon = matchStats.Where(ms => ms.Placement == 1).Count();

        }

        private void ProcessPracticeStats(List<PracticeTarget> practiceStats)
        {
            if(practiceStats.Count == 0)
            {
                ThrowsToHitTarget = 0;
                return;
            }
            ThrowsToHitTarget = practiceStats.Select(ps => ps.ThrowsToHit).Average();
        }



    }
}
