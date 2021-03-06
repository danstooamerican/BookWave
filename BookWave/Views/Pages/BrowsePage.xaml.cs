﻿using BookWave.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookWave.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for BrowsePage.xaml
    /// </summary>
    public partial class BrowsePage : Page
    {
        private BrowseViewModel viewModel;

        public BrowsePage()
        {
            InitializeComponent();

            viewModel = ViewModelLocator.Instance.BrowseViewModel;
            this.DataContext = viewModel;
        }

        /// <summary>
        /// Capture the MouseWheel event of the ListView and pass it to the parent
        /// so it can be bubbled to the scroll viewer in the MainWindow.
        /// </summary>
        /// <param name="sender">mouse wheel event sender</param>
        /// <param name="e">event args</param>
        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ListView)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateLibrariesList();
            viewModel.UpdateBrowseList();
        }
    }
}
