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

        public MainWindow()
        {
            InitializeComponent();

            viewModel = (MainWindowViewModel) DataContext;

            //select btnStart by default
            this.UpdateClickRect(btnStart, null);
            viewModel.SelectPage(btnStart);
            SetSharedPageTitleTemplate(btnStart.TitleBarTemplate);
            frmPage.Navigate(new Uri(btnStart.Page, UriKind.Relative));
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
                this.UpdateClickRect(clickedMenuBtn, viewModel.NavigationHistory.CurrentElement.Element);
                viewModel.SelectPage(clickedMenuBtn);
                SetSharedPageTitleTemplate(clickedMenuBtn.TitleBarTemplate);
                frmPage.Navigate(new Uri(clickedMenuBtn.Page, UriKind.Relative));
            }
            e.Handled = true;
        }

        private void UpdateClickRect(MenuButton clickedMenuBtn, MenuButton previous)
        {
            if ((previous != null) && (!clickedMenuBtn.Equals(previous)))
            {
                previous.ClickedRectVisibility = Visibility.Hidden;
            }
            clickedMenuBtn.ClickedRectVisibility = Visibility.Visible;
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
            MenuButton previous = viewModel.NavigationHistory.Back();
            this.UpdateClickRect(viewModel.NavigationHistory.CurrentElement.Element, previous);
            frmPage.GoBack();
        }
        private void PageForward_Click(object sender, RoutedEventArgs e)
        {
            MenuButton previous = viewModel.NavigationHistory.Forward();
            this.UpdateClickRect(viewModel.NavigationHistory.CurrentElement.Element, previous);
            frmPage.GoForward();
        }
    }
}
