using MyDartsManager.Entity;
using MyDartsManager.Helper;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MyDartsManager.process;
using MyDartsManager.DataStructure;

namespace MyDartsManager.UserControls
{
    /// <summary>
    /// UserControl used to simulate game of darts.
    /// </summary>
    public partial class GameControl : UserControl
    {
        //Class for the simulation of running the game.
        private GameProcess _game;
        //Datagrid for showing players and scores. Acts kinda wonky.
        private DataGrid _dataGrid;
        //Speech recognizer used.
        private DartboardSpeechRecognizer _speechRecognizer;
        //Bool stored to keep information about usage of speech recognition.
        private bool _speechRecognitionOn;
        //DartBoard user control.
        private DartBoard _dartBoard;

        public GameControl(List<Player> players, int scoreGoal, bool doubleOut)
        {
            InitializeComponent();

            _game = new GameProcess(players, scoreGoal, doubleOut);
            _game.PlayerChanged += changePlayerLabel;
            _game.ThrowChanged += changeThrowLabel;
            _game.GameEnded += changeContent;
            _game.TriggerEvents();

            createDataGrid(_game.PlayerScores);

            _dartBoard = new DartBoard(680, new Point(690, 690));
            _dartBoard.DartBoardClicked += dartThrow;
            RootGrid.Children.Add(_dartBoard);
            _speechRecognitionOn = false;

            _speechRecognizer = new DartboardSpeechRecognizer();
            _speechRecognizer.ThrowRecognized += dartThrow;
            _speechRecognizer.SpeechCaptureRecognized += captureRecognized;
            _speechRecognizer.RevertRecognized += revertThrow;
        }

        private void changeContent(object sender, EventArgs e)
        {
            this.Content = new MainMenu();
        }
        private void changeThrowLabel(object sender, Tuple<int, string> data)
        {
            switch (data.Item1)
            {
                case 1:
                    throw1Label.Content = data.Item2;
                    break;
                case 2:
                    throw2Label.Content = data.Item2;
                    break;
                case 3:
                    throw3Label.Content = data.Item2;
                    break;
            }
        }

        private void changePlayerLabel(object sender, string s)
        {
            currentPlayerLabel.Content = s;
        }

        private void revertThrow(object sender, EventArgs e)
        {
            _game.RevertThrow();
        }

        private void dartThrow(object sender, Tuple<int, int> dartThrow)
        {
            _game.DartThrow(dartThrow.Item1, dartThrow.Item2);
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

        /// <summary>
        /// Ends current match and discards all data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButtonClick(object sender, RoutedEventArgs e)
        {
            _game.CancelGame();
            this.Content = new MainMenu();
        }

        /// <summary>
        /// Reverts last taken throw at the dartboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void revertButtonClick(object sender, RoutedEventArgs e)
        {
            _game.RevertThrow();
        }

        private void createDataGrid(ObservableDictionary<Player, int> source)
        {
            //Remove cell borders and set background color
            Style cellStyle = new Style(typeof(DataGridCell));
            cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, System.Windows.Application.Current.Resources["SecondaryColor"]));
            cellStyle.Setters.Add(new Setter(DataGridColumnHeader.BorderThicknessProperty, new Thickness(0)));

            //Remove header from the datagrid
            Style columnHeaderStyle = new Style(typeof(DataGridColumnHeader));
            columnHeaderStyle.Setters.Add(new Setter(DataGridColumnHeader.BorderThicknessProperty, new Thickness(0)));

            _dataGrid = new DataGrid
            {
                AutoGenerateColumns = false,
                HeadersVisibility = DataGridHeadersVisibility.None,
                Margin = new Thickness(0, 0, 0, 300),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 300,
                FontSize = 50,
                Foreground = (Brush)System.Windows.Application.Current.Resources["TextColor"],
                CellStyle = cellStyle,
                ColumnHeaderStyle = columnHeaderStyle,
                GridLinesVisibility = DataGridGridLinesVisibility.None,
                IsHitTestVisible = false,
            };
            _dataGrid.Style = (Style)System.Windows.Application.Current.Resources["ColoredDataGridStyle"];

            DataGridTextColumn nameColumn = new DataGridTextColumn
            {
                Header = "Name",
                Binding = new System.Windows.Data.Binding("Key.Name")
            };
            nameColumn.IsReadOnly = true;
            nameColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);



            _dataGrid.Columns.Add(nameColumn);

            DataGridTextColumn valueColumn = new DataGridTextColumn
            {
                Header = "Value",
                Binding = new System.Windows.Data.Binding("Value")
            };
            valueColumn.IsReadOnly = true;
            _dataGrid.Columns.Add(valueColumn);

            _dataGrid.ItemsSource = source;

            _dataGrid.Margin = new Thickness(0, 0, 0, 300);
            statsGrid.Children.Add(_dataGrid);
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
    }
}
