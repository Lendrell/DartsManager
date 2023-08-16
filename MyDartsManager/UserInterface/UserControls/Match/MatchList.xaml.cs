using Microsoft.EntityFrameworkCore;
using MyDartsManager.DataStructure;
using MyDartsManager.Entity;
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

namespace MyDartsManager.UserControls
{
    /// <summary>
    /// UserControl showing list of matches that were played.
    /// </summary>
    public partial class MatchList : UserControl
    {
        DartsDbContext db = new DartsDbContext();

        public MatchList()
        {
            InitializeComponent();
        }

        private void MatchList_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            db.MatchStatistics.Load();
            ObservableDictionary<Match, MatchStatistic> matchData = new ObservableDictionary<Match, MatchStatistic>();
            foreach(Match match in db.Matches)
            {
                matchData.Add(match, match.MatchStatistics.Where(ms => ms.Placement == 1).First());
            }
            matchesDataGrid.ItemsSource = matchData;
            DefineDataGrid();

            matchesDataGrid.SelectedIndex = 0;
        }


        private void DefineDataGrid()
        {
            
            DataGridTextColumn dateColumn = new DataGridTextColumn
            {
                Header = "Date",
                Binding = new System.Windows.Data.Binding("Key.Date") { StringFormat = "dd.MM.yyyy" },
                Width = DataGridLength.Auto,
                CanUserSort = true,
                IsReadOnly = true,
            };
            matchesDataGrid.Columns.Add(dateColumn);
            
            DataGridTextColumn scoreColumn = new DataGridTextColumn
            {
                Header = "Score",
                Binding = new System.Windows.Data.Binding("Key.ScoreGoal"),
                Width = DataGridLength.Auto,
                IsReadOnly = true,
                CanUserSort = true
            };
            matchesDataGrid.Columns.Add(scoreColumn);

            DataGridTextColumn doubleOutColumn = new DataGridTextColumn
            {
                Header = "Double out",
                Binding = new System.Windows.Data.Binding("Key.DoubleOut"),
                Width = DataGridLength.Auto,
                IsReadOnly = true
            };
            matchesDataGrid.Columns.Add(doubleOutColumn);


            DataGridTextColumn winnerColumn = new DataGridTextColumn
            {
                Header = "Winner",
                Binding = new System.Windows.Data.Binding("Value.Player.Name"),
                Width = DataGridLength.Auto,
                IsReadOnly = true
            };
            matchesDataGrid.Columns.Add(winnerColumn);

            DataGridTextColumn averageColumn = new DataGridTextColumn
            {
                Header = "Average score per round",
                Binding = new System.Windows.Data.Binding("Value.AverageScorePerRound") { StringFormat = "N1" },
                Width = DataGridLength.Auto,
                IsReadOnly = true,
                CanUserSort = true
            };
            matchesDataGrid.Columns.Add(averageColumn);



        }

        private void backBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
        }

        private void deleteBtnOnClick(object sender, RoutedEventArgs e)
        {
            if (matchesDataGrid.SelectedIndex == -1) return;
            KeyValuePair<Match, MatchStatistic> selected = (KeyValuePair<Match, MatchStatistic>)matchesDataGrid.SelectedItem;
            if (selected.Key != null )
            {
                db.Matches.Remove(selected.Key);
                db.SaveChanges();
                this.Content = new MatchList();
            }
        }

        private void detailsBtnOnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
