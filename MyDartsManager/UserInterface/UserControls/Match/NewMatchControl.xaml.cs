using Microsoft.EntityFrameworkCore;
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
    /// UserControl used to choose players and set parameters for a new match.
    /// </summary>
    public partial class NewMatchControl : UserControl
    {
        DartsDbContext db = new DartsDbContext();

        private CollectionViewSource _playerViewSource;

        private List<Player> _addedPlayers;
        public NewMatchControl()
        {
            InitializeComponent();
            _playerViewSource = (CollectionViewSource)FindResource(nameof(_playerViewSource));
            _addedPlayers = new List<Player>();
            
        }

        private void NewMatchControl_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            db.Players.Load();
            _playerViewSource.Source =
                db.Players.Local.ToObservableCollection();
            if (_playerViewSource.View.IsEmpty)
            {
                addPlayerBtn.IsEnabled = false;
            }
            startBtn.IsEnabled = false;
        }

        private void cancelBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
        }

        private void startBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new GameControl(_addedPlayers, int.Parse(gameLimitCb.Text), doubleOutCheckBox.IsChecked ?? false);
        }

        private void addPlayerBtnOnClick(object sender, RoutedEventArgs e)
        {
            startBtn.IsEnabled = true;
            Player selectedPlayer = (Player) playersCb.SelectedItem;

            if(!_addedPlayers.Select(p => p.Name).Contains(selectedPlayer.Name))
            {
                _addedPlayers.Add(selectedPlayer);
                addedPlayersLb.Items.Add(selectedPlayer.Name);
            }
        }
    }
}
