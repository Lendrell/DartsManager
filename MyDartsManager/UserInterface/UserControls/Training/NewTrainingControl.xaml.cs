using Microsoft.EntityFrameworkCore;
using MyDartsManager.Entity;
using MyDartsManager.UserControls;
using MyDartsManager.UserInterface.UserControls.Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MyDartsManager.UserInterface.UserControls.Training
{
    /// <summary>
    /// Interaction logic for NewTrainingControl.xaml
    /// </summary>
    public partial class NewTrainingControl : UserControl
    {
        DartsDbContext db = new DartsDbContext();

        private CollectionViewSource _playerViewSource;

        public NewTrainingControl()
        {
            InitializeComponent();
            _playerViewSource = (CollectionViewSource)FindResource(nameof(_playerViewSource));

            simpleCb.IsChecked = true;
            if(db.Players.Count() == 0)
            {
                selectBtn.IsEnabled = false;
            }
        }

        private void NewTrainingControl_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            var query = db.Players.ToList();
            _playerViewSource.Source = new ObservableCollection<Player>(query);
            
        }

        private void cancelBtnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
        }

        private void selectBtnClick(object sender, RoutedEventArgs e)
        {
            Player player = playersCb.SelectedItem as Player;
            TrainingType type = TrainingType.ValueOnly;
            if(combinedCb.IsChecked == true)
            {
                type = TrainingType.ValuesAndMultipliers;
            }
            if(multiCb.IsChecked == true)
            {
                type = TrainingType.MultipliersOnly;
            }

            this.Content = new TrainingControl(player, type);
        }

        private void combinedCbChecked(object sender, RoutedEventArgs e)
        {
            simpleCb.IsChecked = false;
            multiCb.IsChecked = false;
        }

        private void multiCbChecked(object sender, RoutedEventArgs e)
        {
            simpleCb.IsChecked = false;
            combinedCb.IsChecked = false;
        }

        private void simpleCbChecked(object sender, RoutedEventArgs e)
        {
            combinedCb.IsChecked = false;
            multiCb.IsChecked = false;
        }

        private void cbClicked(object sender, RoutedEventArgs e)
        {
            if(combinedCb.IsChecked == false && simpleCb.IsChecked == false && multiCb.IsChecked == false)
            {
                simpleCb.IsChecked = true;
            }
        }
    }
}
