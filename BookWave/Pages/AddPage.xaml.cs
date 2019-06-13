using Commons.ViewModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Commons.Pages
{
    /// <summary>
    /// Interaction logic for AuthorsPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        private AddPageViewModel viewModel;

        public AddPage()
        {
            InitializeComponent();
            viewModel = (AddPageViewModel)DataContext;
        }

        /// <summary>
        /// Capture the MouseWheel event of the DataGrid and pass it to the parent
        /// so it can be boubbled to the scroll viewer in the MainWindow.
        /// </summary>
        /// <param name="sender">mouse wheel event sender</param>
        /// <param name="e">event args</param>
        private void DtgChapters_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            var parent = ((Control)sender).Parent as UIElement;
            parent.RaiseEvent(eventArg);
        }

        /// <summary>
        /// Handle drop of folder in datagrid. Only one folder at a time is allowed.
        /// </summary>
        /// <param name="sender">sender of the drop event</param>
        /// <param name="e">event args</param>
        private void PerformDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length == 1)
                {
                    if (Directory.Exists(files[0]))
                    {
                        AddPageViewModel viewModel = DataContext as AddPageViewModel;

                        viewModel.Audiobook.Metadata.Path = files[0];
                    }
                }
            }
        }

        private void Destination_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TODO make this pretty
            viewModel.Audiobook.Metadata.Path = txbDestination.Text;
            viewModel.AnalyzeFolder();
        }

        private void BtnBrowseLibrary_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ContextMenu contextMenu = btn.ContextMenu;
            contextMenu.PlacementTarget = btn;
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            contextMenu.IsOpen = true;
            e.Handled = true;
        }

        private void BtnBrowseLibrary_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
