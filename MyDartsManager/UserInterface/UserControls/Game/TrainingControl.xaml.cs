using MyDartsManager.Entity;
using MyDartsManager.Helper;
using MyDartsManager.Process;
using MyDartsManager.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyDartsManager.UserInterface.UserControls.Game
{
    /// <summary>
    /// Interaction logic for TrainingControl.xaml
    /// </summary>
    public partial class TrainingControl : UserControl
    {
        private TrainingProcess _trainingProcess;

        private DartBoard _board;

        private DartboardSpeechRecognizer _speechRecognizer;

        private bool _speechRecognitionOn;

        public TrainingControl(Player player, TrainingType type)
        {
            InitializeComponent();
            _trainingProcess = new TrainingProcess(player, type);
            _trainingProcess.TargetChanged += targetChange;
            _trainingProcess.CounterChanged += counterChange;
            _trainingProcess.AverageChanged += averageChange;

            _trainingProcess.TriggerEvents();

            _board = new DartBoard(680, new Point(690, 690));
            _board.DartBoardClicked += dartThrow;
            rootGrid.Children.Add(_board);
            _speechRecognitionOn = false;

            _speechRecognizer = new DartboardSpeechRecognizer();
            _speechRecognizer.ThrowRecognized += dartThrow;
            _speechRecognizer.SpeechCaptureRecognized += captureRecognized;
            _speechRecognizer.RevertRecognized += revertThrow;
        }

        private void counterChange(object sender, int counter)
        {
            throwCountLabel.Content = counter;
        }

        private void averageChange(object sender, double average)
        {
            averageLabel.Content = Math.Round(average,1);
        }

        private void targetChange(object sender, Tuple<int,int> target)
        {
            targetValueLabel.Content = target.Item1;
            targetMultiplierLabel.Content = target.Item2;
        }

        private void dartThrow(object sender, Tuple<int, int> dartThrow)
        {
            hitValue.Content = dartThrow.Item1;
            hitMulti.Content = dartThrow.Item2;
            _trainingProcess.DartThrow(dartThrow.Item1, dartThrow.Item2);
        }

        private void revertThrow(object sender, EventArgs e)
        {
            _trainingProcess.RevertThrow();
        }

        private void captureRecognized(object sender, bool b)
        {
            if (b)
            {
                speechCaptureCircle.Fill = Brushes.DarkGreen;
            }
            else
            {
                speechCaptureCircle.Fill = Brushes.DarkRed;
            }
        }

        private void speechButtonClick(object sender, RoutedEventArgs e)
        {
            if (_speechRecognitionOn)
            {
                _speechRecognitionOn = false;
                _speechRecognizer.StopSpeechRecognition();
                return;
            }
            _speechRecognitionOn = true;
            _speechRecognizer.StartSpeechRecognition();
        }

        private void endButtonClick(object sender, RoutedEventArgs e)
        {
            _trainingProcess.EndTraining();
            this.Content = new MainMenu();
        }

        private void cancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
        }

        private void revertBtnClick(object sender, RoutedEventArgs e)
        {
            _trainingProcess.RevertThrow();
        }
    }
}
