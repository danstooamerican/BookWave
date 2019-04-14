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

        private MenuButton menuBtnLastClicked;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuButton clickedMenuBtn = e.Source as MenuButton;
            if ((menuBtnLastClicked != null) && (!clickedMenuBtn.Equals(menuBtnLastClicked)) )
            {
                menuBtnLastClicked.ClickedRectVisibility = Visibility.Hidden;
            }
            menuBtnLastClicked = clickedMenuBtn;
            clickedMenuBtn.ClickedRectVisibility = Visibility.Visible;

            e.Handled = true;
        }
    }
}
