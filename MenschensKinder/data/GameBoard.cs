using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace MenschensKinder
{
    /// <summary>
    /// Die Klasse GameBoard weist jeder Ellipse ein Objekt der Klasse GameField zu. Damit lassen sich die UI-Ellipsen mit der Logik verknüpfen.
    /// </summary>
    class GameBoard
    {
        // Dictionary um der Ellipse ein GameField zuzuweisen.
        private IDictionary<Ellipse, GameField> gameField = new Dictionary<Ellipse, GameField>();

        /// <summary>
        /// Füge dem Dictionary einen Eintrag hinzu. Dabei wird der Ellipse ein neues GameField Objekt zugewiesen, welches abhängig von der Position der Ellipse ist.
        /// </summary>
        /// <param name="ellipse">Die Ellipse für das ein GameField generiert wird.</param>
        /// <param name="x">X-Position der Ellipse im Grid</param>
        /// <param name="y">Y-Position der Ellipse im Grid</param>
        public void AddField(Ellipse ellipse, int x, int y)
        {
            gameField.Add(ellipse, new GameField(x, y));
        }

        /// <summary>
        /// Gibt die Position einer Ellipse in Form einer Coordinate2D zurück.
        /// </summary>
        /// <param name="ellipse">Die Ellipse an dessen Position wir interessiert sind</param>
        /// <returns>2D-Coordinate von der jeweiligen Ellipse</returns>
        public Coordinate2D ReturnPosition(Ellipse ellipse)
        {
            // Überprüfe ob die jeweilige Ellipse überhaupt in dem Dictionary vorhanden ist
            if(gameField.ContainsKey(ellipse))
            {       
                // Wenn diese existiert, extrahiere den Wert
                if (gameField.TryGetValue(ellipse, out GameField result))
                {
                    // Gebe von dem Wert-Objekt (GameField result) die Koordinaten wieder
                    return result.Coordinates;
                }
            }
            // Wenn die Ellipse nicht existiert, gebe nichts zurück.
            return null;
        }

        /// <summary>
        /// Gibt das jeweilige GameField einer Ellipse zurück
        /// </summary>
        /// <param name="ellipse">Die Ellipse von dem wir das GameField suchen.</param>
        /// <returns>Ein GameField Objekt welches zu der übergebenen Ellipse gehört</returns>
        public GameField ReturnField(Ellipse ellipse)
        {
            // Überprüfe ob die Ellipse im Dictionary vorhanden ist
            if(gameField.ContainsKey(ellipse))
            {
                // Extrahiere den Wert des Schlüssels (Ellipse)
                if(gameField.TryGetValue(ellipse, out GameField field))
                {
                    return field;
                }
            }
            return null;
        }

        /// <summary>
        /// Button-Handler um dem Würfel-Button Logik hinzuzufügen.
        /// </summary>
        /// <param name="sender">Der Würfel-Button als Objekt</param>
        /// <param name="e"></param>
        internal void RollDice(object sender, RoutedEventArgs e)
        {
            // Instanziere ein neues Random Objekt und lass es 2x von 1-6 würfeln.
            var rand = new Random();
            int dice1 = rand.Next(1, 6);
            int dice2 = rand.Next(1, 6);

            // Überprüfe ob die Würfel-Ergebnisse gleich sind.
            if(dice1 == dice2)
                MessageBox.Show(String.Format("Würfel: {0}, {1}, PASCH!", dice1.ToString(), dice2.ToString()));
            else
                MessageBox.Show(String.Format("Würfel: {0}, {1}", dice1.ToString(), dice2.ToString()));

        }
    }
}
