using Commons.Controls;
using Commons.Exceptions;
using Commons.ViewModel;
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

        private MainWindowViewModel viewModel;

        private MenuButton menuBtnLastClicked;

        public MainWindow()
        {
            InitializeComponent();

            viewModel = (MainWindowViewModel) DataContext;

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
            string path = null;

            if (e.Source is Button clickedBtn)
            {
                if ((menuBtnLastClicked != null) && (!clickedBtn.Equals(menuBtnLastClicked)))
                {
                    menuBtnLastClicked.ClickedRectVisibility = Visibility.Hidden;
                }

                if (e.Source is MenuButton clickedMenuBtn)
                {
                    clickedMenuBtn.ClickedRectVisibility = Visibility.Visible;
                    menuBtnLastClicked = clickedMenuBtn;
                    path = clickedMenuBtn.Page;
                    viewModel.SelectedPageTitle = clickedMenuBtn.PageTitle;
                    SetSharedPageTitleTemplate(clickedMenuBtn.TitleBarTemplate);
                }
                else
                {
                    if (clickedBtn.Tag is string page)
                    {
                        path = page;
                    }

                    menuBtnLastClicked = null;
                }
            }

            if (path != null)
            {
                frmPage.Navigate(new Uri(path, UriKind.Relative));               
            }

            e.Handled = true;
        }

        private void SetSharedPageTitleTemplate(string path)
        {
            try
            {
                viewModel.SharedPageTitleTemplate = (ControlTemplate)App.Current.FindResource(path);
            }
            catch (NullReferenceException)
            {
                throw new InvalidArgumentException(path, "is invalid");
            }

        }

        private void PageBack_Click(object sender, RoutedEventArgs e)
        {
            frmPage.GoBack();
        }
        private void PageForward_Click(object sender, RoutedEventArgs e)
        {
            frmPage.GoForward();
        }
    }
}
