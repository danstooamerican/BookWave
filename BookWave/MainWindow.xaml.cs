using Commons.Controls;
using Commons.Exceptions;
using Commons.Logic;
using Commons.Util;
using Commons.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AudiobookPlayer
{
    /// <summary>
    /// Code behind for the MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainWindowViewModel viewModel;

        /// <summary>
        /// Creates a new Code behind for the MainWindow. 
        /// The MenuButton btnStart is preselected.
        /// </summary>
        public MainWindow()
        {
            ViewModelLocator locator = new ViewModelLocator();
            viewModel = locator.MainViewModel;
            viewModel.SetupBorderlessWindow(this);
            this.DataContext = viewModel;

            InitializeComponent();           

            viewModel.NavigationHistory.CurrentElementChangedEvent += UpdateNavigationUI;

            //select btnStart by default
            MenuButton_Click(btnStart, new RoutedEventArgs(MenuButton.ClickEvent, btnStart));
        }

        /// <summary>
        /// Handles a click on a MenuButton. Only changes the CurrentElement in
        /// the HistoryList if the clicked button is not the CurrentElement at the moment.
        /// </summary>
        /// <param name="sender">MenuButton which was clicked.</param> 
        /// <param name="e">EventArgs of the click event.</param> 
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

        /// <summary>
        /// Updates the MenuBar (sidebar) to select the CurrentElement in the HistoryList.
        /// </summary>
        private void UpdateNavigationUI()
        {
            MenuButton current = viewModel.NavigationHistory.CurrentElement.Element;

            if (current != null)
            {
                UpdateClickRect();

                SetSharedPageTitleTemplate();                
            }                
        }
        /// <summary>
        /// Hides the CllickedRect of the previous element if it was set and shows it on the 
        /// CurrentElement of the HistoryList.
        /// If the button is set as the PreviousElement in the HistoryList the animation 
        /// is not played by omitting to set the ClickedRect to hidden.
        /// </summary>
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
        /// <summary>
        /// Loads the custom TitleTemplate of the CurrentElement into the SharedPageTitle by setting the
        /// SharedPageTitleTemplate property in the viewModel.
        /// 
        /// <throws>InvalidArgumentException if the template could not be located.</throws>
        /// </summary>
        private void SetSharedPageTitleTemplate()
        {
            try
            {
                MenuButton current = viewModel.NavigationHistory.CurrentElement.Element;

                if (current != null)
                {
                    viewModel.SharedPageTitleTemplate = (ControlTemplate)App.Current.FindResource(current.TitleBarTemplate);
                    viewModel.SelectedPageTitle = current.PageTitle;
                }
                
            }
            catch (NullReferenceException)
            {
                throw new InvalidArgumentException("Could not load titlebar.");
            }
        }

        /// <summary>
        /// Performs a PageBack by updating the HistoryList and using the frame navigation.
        /// This method triggers the UpdateNavigationUI method.
        /// </summary>
        /// <param name="sender">Clicked element.</param>
        /// <param name="e">EventArgs of the click event.</param>
        private void PageBack_Click(object sender, RoutedEventArgs e)
        {
            viewModel.NavigationHistory.Back();

            frmPage.GoBack();
        }
        /// <summary>
        /// Performs a PageForward by updating the HistoryList and using the frame navigation.
        /// This method triggers the UpdateNavigationUI method.
        /// </summary>
        /// <param name="sender">Clicked element.</param>
        /// <param name="e">EventArgs of the click event.</param>
        private void PageForward_Click(object sender, RoutedEventArgs e)
        {
            viewModel.NavigationHistory.Forward();

            frmPage.GoForward();
        }

        private void AppWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AudiobookManager.Instance.LoadRepository();
        }
    }
}
