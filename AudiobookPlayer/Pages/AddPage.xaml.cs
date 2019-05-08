using Commons.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Commons.Pages
{
    /// <summary>
    /// Interaction logic for AuthorsPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        public AddPage()
        {
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

                        viewModel.FolderHandler.FolderPath = files[0];
                        viewModel.AnalyzeFolder();
                    }
                }
            }
        }
    }
}
