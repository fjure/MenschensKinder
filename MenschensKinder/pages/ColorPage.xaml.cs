﻿using MenschensKinder.data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenschensKinder
{
    /// <summary>
    /// Code-Behind für die Farbauswahl Seite.
    /// </summary>
    public partial class ColorPage : Page
    {
        public ColorPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Eine Methode um auf den Buttonklick zu reagieren. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ColorBtn(object sender, RoutedEventArgs e)
        {
            // Extrahiere den Text des jeweiligen Knopfes, und entscheide am Text welche Farbe der Spieler wählt.
            var btnText = (sender as Button).Content;
            var color = new PlayerColor();
            switch (btnText) {
                case "Rot":
                    color = PlayerColor.RED;
                    break;
                case "Blau":
                    color = PlayerColor.BLUE;
                    break;
                case "Grün":
                    color = PlayerColor.GREEN;
                    break;
                case "Gelb":
                    color = PlayerColor.YELLOW;
                    break;
            }

            // Navigiere zum finalen Spielbrett
            this.NavigationService.Navigate(new GamePage(color));
        }

        private void BackBtn(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MenuPage());
        }

    }
}
