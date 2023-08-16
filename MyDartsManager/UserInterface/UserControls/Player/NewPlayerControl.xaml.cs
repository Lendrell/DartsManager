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
    /// UserControl for creating a new player.
    /// </summary>
    public partial class NewPlayerControl : UserControl
    {
        DartsDbContext db = new DartsDbContext();
        public NewPlayerControl()
        {
            InitializeComponent();
        }

        private void cancelBtnOnClick(object sender, RoutedEventArgs e)
        {
            this.Content = new PlayerList();
        }
        private void addBtnOnClick(object sender, RoutedEventArgs e)
        {
            if (nameTb.Text.Length <= 0)
            {
                return;
            }

            Player player = new Player(nameTb.Text);



            db.Database.EnsureCreated();
            if (db.Players.Select(p => p.Name).Contains(nameTb.Text))
            {
                return;
            }
            db.Players.Add(player);
            db.SaveChanges();

            this.Content = new PlayerList();
        }
    }
}
