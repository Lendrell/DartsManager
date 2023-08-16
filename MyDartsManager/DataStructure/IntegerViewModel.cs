using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDartsManager.DataStructure
{
    public class IntegerViewModel : INotifyPropertyChanged
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // ++ operator
        public static IntegerViewModel operator ++(IntegerViewModel viewModel)
        {
            viewModel.Value++;
            return viewModel;
        }

        // += operator
        public static IntegerViewModel operator +(IntegerViewModel viewModel, int value)
        {
            viewModel.Value += value;
            return viewModel;
        }

        // -= operator
        public static IntegerViewModel operator -(IntegerViewModel viewModel, int value)
        {
            viewModel.Value -= value;
            return viewModel;
        }

        // -- operator
        public static IntegerViewModel operator --(IntegerViewModel viewModel)
        {
            viewModel.Value--;
            return viewModel;
        }

        // Implicit conversion from IntegerViewModel to int
        public static implicit operator int(IntegerViewModel viewModel)
        {
            return viewModel.Value;
        }

        // Implicit conversion from int to IntegerViewModel
        public static implicit operator IntegerViewModel(int value)
        {
            return new IntegerViewModel { Value = value };
        }
    }
}
