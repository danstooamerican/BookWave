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

            //select btnStart by default
            MenuButton_Click(btnStart, new RoutedEventArgs(Button.ClickEvent, btnStart));
        }

        /// <summary>
        /// Click Handler for all MenuButtons in the left StackPanel
        /// </summary>
        /// <param name="sender"></param> MenuButton that was clicked
        /// <param name="e"></param> EventArgs of the click event
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is MenuButton clickedMenuBtn)
            {
                if ((menuBtnLastClicked != null) && (!clickedMenuBtn.Equals(menuBtnLastClicked)))
                {
                    menuBtnLastClicked.ClickedRectVisibility = Visibility.Hidden;
                }
                menuBtnLastClicked = clickedMenuBtn;
                clickedMenuBtn.ClickedRectVisibility = Visibility.Visible;

                frmPage.Navigate(new Uri(clickedMenuBtn.Page, UriKind.Relative));

                e.Handled = true;
            }            
        }

    }
}
