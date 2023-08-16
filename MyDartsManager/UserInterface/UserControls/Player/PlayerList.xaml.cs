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
    /// UserControl showing list of players already added to the app.
    /// </summary>
    public partial class PlayerList : UserControl
    {
        DartsDbContext db = new DartsDbContext();

        public PlayerList()
        {
            InitializeComponent();
        }

        private void PlayerList_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            ObservableDictionary<Player, PlayerStats> playerData = new ObservableDictionary<Player, PlayerStats>();

            foreach (Player player in db.Players)
            {
                playerData.Add(player, new PlayerStats(player.PlayerId));
            }
            playersDataGrid.ItemsSource = playerData;

            DefineDataGrid();
        }


        private void DefineDataGrid()
        {
            DataGridTextColumn nameColumn = new DataGridTextColumn
            {
                Header = "Name",
                Binding = new System.Windows.Data.Binding("Key.Name"),
                Width = DataGridLength.Auto,
                IsReadOnly = true
            };
            playersDataGrid.Columns.Add(nameColumn);

            DataGridTextColumn matchesColumn = new DataGridTextColumn
            {
                Header = "Matches played",
                Binding = new System.Windows.Data.Binding("Value.MatchesPlayed"),
                Width = DataGridLength.Auto,
                IsReadOnly = true,
                CanUserSort = true
            };
            playersDataGrid.Columns.Add(matchesColumn);


            DataGridTextColumn matchesWonColumn = new DataGridTextColumn
            {
                Header = "Matches won",
                Binding = new System.Windows.Data.Binding("Value.MatchesWon"),
                Width = DataGridLength.Auto,
                IsReadOnly = true,
                CanUserSort = true
            };
            playersDataGrid.Columns.Add(matchesWonColumn);

            
            DataGridTextColumn averageColumn = new DataGridTextColumn
            {
                Header = "Average throws to hit target",
                Binding = new System.Windows.Data.Binding("Value.ThrowsToHitTarget") { StringFormat = "N1" },
                Width = DataGridLength.Auto,
                IsReadOnly = true,
                CanUserSort = true
            };
            playersDataGrid.Columns.Add(averageColumn);
            


        }

        private void newPlayerBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new NewPlayerControl();
        }

        private void deleteBtnOnClick(object sender, RoutedEventArgs e)
        {
            Player selectedPlayer = (Player)playersDataGrid.SelectedItem;
            if (selectedPlayer != null)
            {
                db.Players.Remove(selectedPlayer);
                db.SaveChanges();
                this.Content = new PlayerList();
            }
        }

        private void backBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
            
        }

    }
}
