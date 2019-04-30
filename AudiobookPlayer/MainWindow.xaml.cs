using Commons.Controls;
using Commons.Exceptions;
using Commons.Util;
using Commons.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

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

            viewModel.NavigationHistory.CurrentElementChangedEvent += UpdateNavigationUI;

            //select btnStart by default
            MenuButton_Click(btnStart, new RoutedEventArgs(MenuButton.ClickEvent, btnStart));
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
                if (viewModel.NavigationHistory.IsNotCurrentElement(clickedMenuBtn))
                {
                    viewModel.NavigationHistory.AddAtCurrentElementDeleteBehind(clickedMenuBtn);

                    frmPage.Navigate(new Uri(clickedMenuBtn.Page, UriKind.Relative));
                    scrollViewer.ScrollToTop();
                    lblSelectedPageTitle.Opacity = 0;
                }                
            }
            e.Handled = true;
        }

        private void UpdateNavigationUI()
        {
            MenuButton current = viewModel.NavigationHistory.CurrentElement.Element;

            if (current != null)
            {
                UpdateClickRect();

                SetSharedPageTitleTemplate();

                viewModel.SelectedPageTitle = current.PageTitle;
            }                
        }
        private void UpdateClickRect()
        {
            if (!viewModel.NavigationHistory.IsRepeatedElement())
            {
                HistoryListElement<MenuButton> previousElement = viewModel.NavigationHistory.PreviousElement;

                if (previousElement != null)
                {
                    previousElement.Element.ClickedRectVisibility = Visibility.Hidden;
                }                
            }
            viewModel.NavigationHistory.CurrentElement.Element.ClickedRectVisibility = Visibility.Visible;
        }
        private void SetSharedPageTitleTemplate()
        {
            try
            {
                MenuButton current = viewModel.NavigationHistory.CurrentElement.Element;

                if (current != null)
                {
                    viewModel.SharedPageTitleTemplate = (ControlTemplate)App.Current.FindResource(current.TitleBarTemplate);
                }
                
            }
            catch (NullReferenceException)
            {
                throw new InvalidArgumentException("Could not load Titlebar.");
            }
        }

        private void PageBack_Click(object sender, RoutedEventArgs e)
        {
            viewModel.NavigationHistory.Back();

            frmPage.GoBack();
        }
        private void PageForward_Click(object sender, RoutedEventArgs e)
        {
            viewModel.NavigationHistory.Forward();

            frmPage.GoForward();
        }
    }
}
