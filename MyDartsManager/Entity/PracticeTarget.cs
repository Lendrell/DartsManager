using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDartsManager.Entity
{
    public class PracticeTarget
    {
        public int Id { get; set; }


        public int ThrowCombinationId { get; set; }
        public virtual ThrowCombination ThrowCombination { get; set; }

        public int ThrowsToHit { get; set; }

        public int TrainingSessionId { get; set; }
        public virtual TrainingSession TrainingSession { get; set; }
    }
}
