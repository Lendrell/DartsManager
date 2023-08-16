using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDartsManager.Entity
{
    public enum TrainingType
    {
        ValueOnly,
        ValuesAndMultipliers,
        MultipliersOnly
    }

    public class TrainingSession
    {
        public int Id { get; set; }

        public virtual ICollection<PracticeTarget> PracticeTargets { get; set; }

        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public TrainingType TrainingType { get; set; }

    }
}
