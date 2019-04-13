using Commons.Controls;
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

namespace AudiobookPlayer
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ((StackPanel)sender).Children)
            {
                if (item.GetType().Equals(typeof(MenuButton)))
                {
                    MenuButton btn = (MenuButton)item;
                    btn.ClickedRectVisibility = Visibility.Hidden;
                }
            }

            FrameworkElement feSource = e.Source as FrameworkElement;
            switch (feSource.Name)
            {
                case "btnStart":                    
                    btnStart.ClickedRectVisibility = Visibility.Visible;
                    break;
                case "btnBrowse":
                    btnBrowse.ClickedRectVisibility = Visibility.Visible;
                    break;
                case "btnAuthors":
                    btnAuthors.ClickedRectVisibility = Visibility.Visible;
                    break;
                case "btnGenres":
                    btnGenres.ClickedRectVisibility = Visibility.Visible;
                    break;
                case "btnFavorites":
                    btnFavorites.ClickedRectVisibility = Visibility.Visible;
                    break;
            }
            e.Handled = true;
        }
    }
}
