using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace MyDartsManager.Helper
{
    public class DartboardSpeechRecognizer
    {
        private SpeechRecognitionEngine _recognitionEngine;

        public event EventHandler<bool> SpeechCaptureRecognized;
        public event EventHandler RevertRecognized;
        public event EventHandler<Tuple<int,int>> ThrowRecognized;

        private bool _capture;

        public DartboardSpeechRecognizer()
        {
            InitializeSpeechRecognition();
            _capture = false;
        }

        private void InitializeSpeechRecognition()
        {
            _recognitionEngine = new SpeechRecognitionEngine();

            // Define grammar rules for dartboard throw combinations
            GrammarBuilder possibleMultiThrowsBuilder = new GrammarBuilder();
            possibleMultiThrowsBuilder.Append(getMultiplierChoiceLibrary());
            possibleMultiThrowsBuilder.Append(getValueChoiceLibrary());

            GrammarBuilder possibleSingleThrowsBuilder = new GrammarBuilder();
            possibleSingleThrowsBuilder.Append(getValueChoiceLibrary());

            Grammar possibleMultiThrowsGrammar = new Grammar(possibleMultiThrowsBuilder);
            Grammar possibleSingleThrowsGrammar = new Grammar(possibleSingleThrowsBuilder);

            _recognitionEngine.LoadGrammar(possibleMultiThrowsGrammar);
            _recognitionEngine.LoadGrammar(possibleSingleThrowsGrammar);

            // Register event handlers
            _recognitionEngine.SpeechRecognized += SpeechRecognized;
        }

        public void StartSpeechRecognition()
        {

            _recognitionEngine.SetInputToDefaultAudioDevice();
            _recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            _capture = true;
            SpeechCaptureRecognized?.Invoke(this, _capture);

        }

        public void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Contains("capture"))
            {
                
                _capture = !_capture;
                SpeechCaptureRecognized?.Invoke(this, _capture);
                return;
            }

            if (!_capture)
            {
                return;
            }

            if (e.Result.Text.Contains("revert"))
            {
                RevertRecognized?.Invoke(this, null);
                return;
            }

            int value = 0;
            int multi = 1;
            Dictionary<string, int> numberMap = new Dictionary<string, int>
            {
                { "twentyfive", 25 },
                { "twenty", 20 },
                { "nineteen", 19 },
                { "eighteen", 18 },
                { "seventeen", 17 },
                { "sixteen", 16 },
                { "fifteen", 15 },
                { "fourteen", 14 },
                { "thirteen", 13 },
                { "twelve", 12 },
                { "eleven", 11 },
                { "ten", 10 },
                { "nine", 9 },
                { "eight", 8 },
                { "seven", 7 },
                { "six", 6 },
                { "five", 5 },
                { "four", 4 },
                { "three", 3 },
                { "two", 2 },
                { "one", 1 },
                { "zero", 0 }
            };

            foreach (var entry in numberMap)
            {
                if (e.Result.Text.Contains(entry.Key))
                {
                    value = entry.Value;
                    break;
                }
            }

            if (e.Result.Text.Contains("double"))
            {
                multi = 2;
            }
            else if (e.Result.Text.Contains("triple") || e.Result.Text.Contains("dribble"))
            {
                multi = 3;
            }
            Tuple<int,int> dartThrow = new Tuple<int,int>(value, multi);

            ThrowRecognized?.Invoke(this, dartThrow);
        }



        public void StopSpeechRecognition()
        {
            _recognitionEngine.RecognizeAsyncStop();
            _capture = false;
            SpeechCaptureRecognized?.Invoke(this, _capture);
        }

        private Choices getValueChoiceLibrary()
        {
            Choices choices = new Choices();
            choices.Add("twentyfive");
            choices.Add("twenty");
            choices.Add("nineteen");
            choices.Add("eighteen");
            choices.Add("seventeen");
            choices.Add("sixteen");
            choices.Add("fifteen");
            choices.Add("fourteen");
            choices.Add("thirteen");
            choices.Add("twelve");
            choices.Add("eleven");
            choices.Add("ten");
            choices.Add("nine");
            choices.Add("eight");
            choices.Add("seven");
            choices.Add("six");
            choices.Add("five");
            choices.Add("four");
            choices.Add("three");
            choices.Add("two");
            choices.Add("one");
            choices.Add("zero");
            choices.Add("revert");
            choices.Add("capture");
            return choices;
        }

        private Choices getMultiplierChoiceLibrary()
        {
            Choices choices = new Choices();
            choices.Add("single");
            choices.Add("double");
            choices.Add("triple");
            //dribble cause the speech recognition engine seems to have problem recognizing triple and dribble works better.
            choices.Add("dribble");
            return choices;
        }
    }
}
