using MyDartsManager.UserInterface.UserControls.Training;
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
    /// Main menu in the app. Used to navigate between different windows.
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void playerListBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new PlayerList();
        }

        private void matchHistoryBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new MatchList();
        }

        private void newMatchBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new NewMatchControl();
        }

        private void trainingBtnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new NewTrainingControl();
        }
    }
}
