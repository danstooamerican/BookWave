using BookWave.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookWave.Desktop.AudiobookManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for SelectLibraryItemDialog.xaml
    /// </summary>
    public partial class SelectLibraryItemDialog : DialogWindow
    {
        public static bool ITEM_SELECTED = true;

        /// <summary>
        /// Window always takes the dimensions of the parent window and multiplies
        /// it by the ratio.
        /// </summary>
        protected static double WINDOW_RATIO = 0.8;

        private SelectLibraryViewModel viewModel;

        /// <summary>
        /// Currently selected Library Item
        /// </summary>
        public Audiobook Selected
        {
            get
            {
                return viewModel.Selected;
            }
        }

        /// <summary>
        /// Creates a new SelectLibraryItemDialog.  
        /// </summary>
        /// <param name="parent">parent window</param>
        public SelectLibraryItemDialog(Page parent) : base(parent)
        {
            InitializeComponent();

            viewModel = ViewModelLocator.Instance.SelectLibraryViewModel;
            this.DataContext = viewModel;

            this.Width = Owner.ActualWidth * WINDOW_RATIO;
            this.Height = Owner.ActualHeight * WINDOW_RATIO;
        }

        /// <summary>
        /// Darken the parent window so the focus is on the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected new void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateLibrariesList();
            viewModel.UpdateBrowseList();

            base.Window_Loaded(sender, e);
        }

        /// <summary>
        /// Select a library item by double clicking it. This closes the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Selected != null)
            {
                this.DialogResult = ITEM_SELECTED;
            }
        }
    }
}
