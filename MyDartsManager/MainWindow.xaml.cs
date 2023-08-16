using MyDartsManager.Helper;
using System.Windows;
using MyDartsManager.UserControls;

namespace MyDartsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DartsDbContext db = new DartsDbContext();
        public MainWindow()
        {
            InitializeComponent();

            /*
            using (var dbContext = new DartsDbContext())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }*/

            db.Database.EnsureCreated();
            DataHelper.SeedThrowCombinations(db);
            this.WindowState = WindowState.Maximized;
        }

        private void welcomeBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new MainMenu();
        }
    }
}
