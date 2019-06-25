using Commons.Models;
using Commons.Util;
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
using System.Windows.Shapes;

namespace Commons.Dialogs
{
    /// <summary>
    /// Interaction logic for SelectLibraryItemDialog.xaml
    /// </summary>
    public partial class SelectLibraryItemDialog : Window
    {
        public static bool ITEM_SELECTED = true;

        /// <summary>
        /// Window always takes the dimensions of the parent window and multiplies
        /// it by the ratio.
        /// </summary>
        private static double WINDOW_RATIO = 0.8;

        private SelectLibraryViewModel viewModel;

        /// <summary>
        /// Currently selected Library Item
        /// </summary>
        public Audiobook Selected {
            get
            {
                return viewModel.Selected;
            }
        }

        /// <summary>
        /// Creates a new SelectLibraryItemDialog.  
        /// </summary>
        /// <param name="parent">parent window</param>
        public SelectLibraryItemDialog(Page parent)
        {
            InitializeComponent();

            viewModel = ViewModelLocator.Instance.SelectLibraryViewModel;
            viewModel.ReloadLibrary();
            this.DataContext = viewModel;
            this.Owner = Window.GetWindow(parent);

            this.Width = Owner.ActualWidth * WINDOW_RATIO;
            this.Height = Owner.ActualHeight * WINDOW_RATIO;       
        }

        /// <summary>
        /// Darken the parent window so the focus is on the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.MainViewModel.DarkenBackground = true;
        }

        /// <summary>
        /// If the dialog closes remove the darken effect from the parent.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            ViewModelLocator.Instance.MainViewModel.DarkenBackground = false;
        }

        /// <summary>
        /// Select a library item by double clicking it. This closes the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = ITEM_SELECTED;
        }
    }
}
