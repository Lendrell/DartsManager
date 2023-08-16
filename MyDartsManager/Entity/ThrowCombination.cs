using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDartsManager.Entity
{
    public class ThrowCombination
    {
        public int CombinationId { get; set; }
        public int Value { get; set; }
        public int Multiplier { get; set; }

        public ThrowCombination(int value, int multiplier)
        { 
            Value = value;
            Multiplier = multiplier;
        }

        public int TrueValue()
        {
            return Value * Multiplier;
        }

        public override string ToString()
        {
            if (Value == -1) return "-";
            return TrueValue().ToString();
        }

    }
}
