using BookWave.Desktop.Util;
using System.Windows;

namespace BookWave.Desktop
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : SkinnedApplication
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SkinResourceDictionary.ValidateSkins();

            // TODO load user preferences, set menu checkmark to match selected skin

            // without this, we would start with the last skin defined in the XAML
            ActiveSkin = DefaultSkinName;

            // now we can start the party
            MainWindow w = new MainWindow();
            w.Show();
        }

    }
}
