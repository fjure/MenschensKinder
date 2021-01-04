using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MenschensKinder
{
    /// <summary>
    /// Erste Seite der Applikation. Methoden um die Buttons interaktiv zu gestalten.
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Startet den Einzelspieler Modus und navigiert zur nächsten Seite (ColorPage). Bei dieser kann der Spieler seine Farbe wählen.
        /// </summary>
        /// <param name="sender">Der Button</param>
        /// <param name="e"></param>
        private void MenuBtnSingleplayer (object sender, RoutedEventArgs e)
        {
            ColorPage cp = new ColorPage();
            this.NavigationService.Navigate(cp);
        }

        private void MenuBtnMultiplayer(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("Multiplayer.xaml", UriKind.RelativeOrAbsolute));
        }

        private void MenuBtnSettings(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void MenuBtnExit(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
