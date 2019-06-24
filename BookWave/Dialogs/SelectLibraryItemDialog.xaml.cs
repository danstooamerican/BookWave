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
        private static double WINDOW_RATIO = 0.8;

        private SelectLibraryViewModel viewModel;

        public Audiobook Selected {
            get
            {
                return viewModel.Selected;
            }
        }

        public SelectLibraryItemDialog(Page parent)
        {
            InitializeComponent();

            viewModel = ViewModelLocator.Instance.SelectLibraryViewModel;
            viewModel.ReloadLibrary();
            this.DataContext = viewModel;
            this.Owner = Window.GetWindow(parent);

            this.Width = Owner.Width * WINDOW_RATIO;
            this.Height = Owner.Height * WINDOW_RATIO;       
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = ITEM_SELECTED;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Instance.MainViewModel.DarkenBackground = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ViewModelLocator.Instance.MainViewModel.DarkenBackground = false;
        }
    }
}
