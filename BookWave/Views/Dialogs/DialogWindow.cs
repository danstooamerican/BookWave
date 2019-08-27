using BookWave.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BookWave.Desktop.Views.Dialogs
{
    public abstract class DialogWindow : Window
    {       

        public DialogWindow(Page parent)
        {
            this.Owner = Window.GetWindow(parent);
        }

        /// <summary>
        /// Darken the parent window so the focus is on the dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            ViewModelLocator.Instance.MainViewModel.DarkenBackground = true;
        }

        /// <summary>
        /// If the dialog closes remove the darken effect from the parent.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Window_Closed(object sender, EventArgs e)
        {
            ViewModelLocator.Instance.MainViewModel.DarkenBackground = false;
        }
    }
}
