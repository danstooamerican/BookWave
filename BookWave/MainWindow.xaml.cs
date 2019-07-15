using Commons.AudiobookManagemenet;
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
            viewModel = ViewModelLocator.Instance.MainViewModel;
            viewModel.SetupBorderlessWindow(this);
            this.DataContext = viewModel;

            viewModel.MainWindow = this;

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
        public void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is MenuButton clickedMenuBtn)
            {
                SwitchPage(new PageItem(clickedMenuBtn));
            }
            e.Handled = true;
        }

        public void SwitchPage(PageItem pageItem)
        {
            if (viewModel.NavigationHistory.IsNotCurrentElement(pageItem))
            {
                viewModel.NavigationHistory.AddAtCurrentElementDeleteBehind(pageItem);

                ChangePageContent(pageItem.PagePath);
            }
        }

        private void ChangePageContent(string pagePath)
        {
            frmPage.Navigate(new Uri(pagePath, UriKind.Relative));
            scrollViewer.ScrollToTop();
            lblSelectedPageTitle.Opacity = 0;
        }

        public void OpenInvisiblePage(string pagePath, string title, string titleBarTemplate = "DefaultTitle")
        {
            PageItem pageItem = new PageItem(pagePath, title, titleBarTemplate);

            SwitchPage(pageItem);
        }

        /// <summary>
        /// Updates the MenuBar (sidebar) to select the CurrentElement in the HistoryList.
        /// </summary>
        private void UpdateNavigationUI()
        {
            PageItem current = viewModel.NavigationHistory.CurrentElement.Element;

            if (current != null)
            {
                UpdateClickRect();

                SetSharedPageTitleTemplate(current.TitleBarTemplate, current.PageTitle);
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
                HistoryListElement<PageItem> previousElement = viewModel.NavigationHistory.PreviousElement;

                if (previousElement != null && previousElement.Element.MenuButton != null)
                {
                    previousElement.Element.MenuButton.ClickedRectVisibility = Visibility.Hidden;
                }
            }
            HistoryListElement<PageItem> currentElement = viewModel.NavigationHistory.CurrentElement;
            if (currentElement != null && currentElement.Element.MenuButton != null)
            {
                currentElement.Element.MenuButton.ClickedRectVisibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// Loads the custom TitleTemplate of the CurrentElement into the SharedPageTitle by setting the
        /// SharedPageTitleTemplate property in the viewModel.
        /// 
        /// <throws>InvalidArgumentException if the template could not be located.</throws>
        /// </summary>
        private void SetSharedPageTitleTemplate(string titleBarTemplate, string pageTitle)
        {
            try
            {
                viewModel.SharedPageTitleTemplate = (ControlTemplate)App.Current.FindResource(titleBarTemplate);
                viewModel.SelectedPageTitle = pageTitle;

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
            LibraryManager.Instance.LoadLibraries();
        }
    }
}
