using Microsoft.EntityFrameworkCore;
using MyDartsManager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDartsManager.Process
{
    class TrainingProcess
    {

        private DartsDbContext db;

        public TrainingType TrainingType { get; private set; }

        
        private TrainingSession _session;

        private Player _player;
        public ThrowCombination CurrentTarget { get; private set; }

        public int ThrowCounter { get; private set; }

        private PracticeTarget? _lastTarget;

        public double AveragePerTarget { get; private set; }
        private int _totalTargetCount;
        private int _totalThrowCount;

        public event EventHandler<Tuple<int, int>> TargetChanged;
        public event EventHandler<int> CounterChanged;
        public event EventHandler<double> AverageChanged;

        public TrainingProcess(Player player, TrainingType trainingType)
        {
            this._player = player;
            this._session = new TrainingSession();
            _session.PlayerId = player.PlayerId;
            this.TrainingType = trainingType;
            db = new DartsDbContext();
            db.TrainingSessions.Add(_session);
            db.SaveChanges();
            this.ThrowCounter = 1;
            AveragePerTarget = 0;
            getNewTarget();
        }

        public void TriggerEvents()
        {
            TargetChanged?.Invoke(this, new Tuple<int, int>(CurrentTarget.Value, CurrentTarget.Multiplier));
            CounterChanged?.Invoke(this, ThrowCounter - 1);
            AverageChanged?.Invoke(this, AveragePerTarget);
        }

        private void getNewTarget()
        {
            Random random = new Random();
            int value = random.Next(1, 21);
            int multiplier = random.Next(1,4);
            if (TrainingType == TrainingType.ValueOnly)
            {
                CurrentTarget = db.ThrowCombinations.Where(x => x.Value == value && x.Multiplier == 1).First();
                return;
            }
            if(TrainingType == TrainingType.ValuesAndMultipliers)
            {
                if(random.Next(1,3) == 1)
                {
                    multiplier = 1;
                }
                CurrentTarget = db.ThrowCombinations.Where(x => x.Value == value && x.Multiplier == multiplier).First();
                return;
            }
            multiplier = random.Next(2, 4);
            CurrentTarget = db.ThrowCombinations.Where(x => x.Value == value && x.Multiplier == multiplier).First();
        }

        public void DartThrow(int value, int multiplier)
        {
            if(CurrentTarget.Value == value && CurrentTarget.Multiplier == multiplier)
            {
                PracticeTarget practiceTarget = new PracticeTarget();
                practiceTarget.TrainingSession = _session;
                practiceTarget.ThrowCombination = CurrentTarget;
                practiceTarget.ThrowsToHit = ThrowCounter;



                _totalThrowCount += ThrowCounter;
                _totalTargetCount++;
                AveragePerTarget = (double)_totalThrowCount / (double)_totalTargetCount;





                db.PracticeTargets.Add(practiceTarget);
                db.SaveChanges();
                _lastTarget = practiceTarget;

                getNewTarget();
                ThrowCounter = 1;
                TriggerEvents();
                return;
            }
            CounterChanged?.Invoke(this, ThrowCounter);
            ThrowCounter++;
        }

        public void RevertThrow()
        {
            if (ThrowCounter == 1)
            {
                if (_lastTarget == null) return;
                db.PracticeTargets.Remove(_lastTarget);
                CurrentTarget = _lastTarget.ThrowCombination;
                ThrowCounter = _lastTarget.ThrowsToHit;
                TriggerEvents();
                return;
            }
            ThrowCounter--;
            CounterChanged?.Invoke(this, ThrowCounter - 1);

        }

        public void EndTraining()
        {
            db.SaveChanges();
        }

    }
}
