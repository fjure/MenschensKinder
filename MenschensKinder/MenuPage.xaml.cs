using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MenschensKinder
{
    /// <summary>
    /// Interaktionslogik für MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void MenuBtnSingleplayer (object sender, RoutedEventArgs e)
        {
            GamePage gp = new GamePage();
            this.NavigationService.Navigate(gp);
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
