﻿using Infralution.Localization.Wpf;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;

namespace BookWave.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for FavoritesPage.xaml
    /// </summary>
    public partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App app = App.Current as App;

            app.ActiveSkin = "LightTheme";
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            App app = App.Current as App;

            app.ActiveSkin = "DarkTheme";
        }

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            App app = App.Current as App;

            app.ActiveSkin = "TestTheme";
        }

        private void Button_Click_3(object sender, System.Windows.RoutedEventArgs e)
        {
            CultureManager.UICulture = new CultureInfo("en-US");
        }

        private void Button_Click_4(object sender, System.Windows.RoutedEventArgs e)
        {
            CultureManager.UICulture = new CultureInfo("de-DE");
        }
    }
}
