using Commons.Dialogs;
using Commons.Util;
using Commons.ViewModel;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Commons.Pages
{
    /// <summary>
    /// Interaction logic for AuthorsPage.xaml
    /// </summary>
    public partial class EditLibrary : Page
    {

        private EditLibraryViewModel viewModel;

        public EditLibrary()
        {
            viewModel = ViewModelLocator.Instance.EditLibraryViewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        /// <summary>
        /// Capture the MouseWheel event of the DataGrid and pass it to the parent
        /// so it can be boubbled to the scroll viewer in the MainWindow.
        /// </summary>
        /// <param name="sender">mouse wheel event sender</param>
        /// <param name="e">event args</param>
        private void DtgChapters_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is DataGrid)
            {
                e.Handled = true;
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }            
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
                        viewModel.Audiobook.Metadata.Path = files[0];
                    }
                }
            }
        }

        private void BtnBrowseLibrary_Click(object sender, RoutedEventArgs e)
        {
            SelectLibraryItemDialog dialog = new SelectLibraryItemDialog(this);

            if (dialog.ShowDialog() == SelectLibraryItemDialog.ITEM_SELECTED)
            {
                viewModel.Destination = dialog.Selected.Metadata.Path;
            }
        }

    }
}
