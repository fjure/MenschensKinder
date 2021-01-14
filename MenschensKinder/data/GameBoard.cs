using MenschensKinder.data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MenschensKinder
{
    /// <summary>
    /// Die Klasse GameBoard weist jeder Ellipse ein Objekt der Klasse GameField zu. Damit lassen sich die UI-Ellipsen mit der Logik verknüpfen.
    /// </summary>
    class GameBoard
    {
        // Dictionary um der Ellipse ein GameField zuzuweisen.
        private readonly IDictionary<Ellipse, GameField> gameField = new Dictionary<Ellipse, GameField>();

        public event Action DiceRolledEvent;

        private int rolledDice = 0;
        public int RolledDice
        {
            get => rolledDice;
            set => this.rolledDice = value;
        }

        /// <summary>
        /// Füge dem Dictionary einen Eintrag hinzu. Dabei wird der Ellipse ein neues GameField Objekt zugewiesen, welches abhängig von der Position der Ellipse ist.
        /// </summary>
        /// <param name="ellipse">Die Ellipse für das ein GameField generiert wird.</param>
        /// <param name="x">X-Position der Ellipse im Grid</param>
        /// <param name="y">Y-Position der Ellipse im Grid</param>
        public void AddField(Ellipse ellipse, int x, int y)
        {
            gameField.Add(ellipse, new GameField(x, y, (SolidColorBrush)ellipse.Fill, ellipse.Visibility));
        }

        /// <summary>
        /// Gibt die Position einer Ellipse in Form einer Coordinate2D zurück.
        /// </summary>
        /// <param name="ellipse">Die Ellipse an dessen Position wir interessiert sind</param>
        /// <returns>2D-Coordinate von der jeweiligen Ellipse</returns>
        public Coordinate2D ReturnPosition(Ellipse ellipse)
        {
            // Überprüfe ob die jeweilige Ellipse überhaupt in dem Dictionary vorhanden ist
            if (gameField.ContainsKey(ellipse))
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
            //MessageBox.Show("Ellipse" + gameField.Count.ToString());
            // Überprüfe ob die Ellipse im Dictionary vorhanden ist
            if (gameField.ContainsKey(ellipse))
            {
                // Extrahiere den Wert des Schlüssels (Ellipse)
                if (gameField.TryGetValue(ellipse, out GameField field))
                {
                    return field;
                }
            }
            return null;
        }

        public GameField ReturnField(Coordinate2D coordinates)
        {
            //MessageBox.Show(gameField.Values.Count.ToString());
            foreach (GameField field in gameField.Values)
            {
                //MessageBox.Show(field.Coordinates.ToString());
                //MessageBox.Show("ReturnField??");
                if (field.Coordinates.X == coordinates.X && field.Coordinates.Y == coordinates.Y)
                {
                    return field;
                }
            }
            return null;
        }

        public GameField DetermineNextField(Figure figure, GameField currentField)
        {
            GameField nextField;
            List<GameField> fieldsAround = ScanFieldsAround(figure);
            if (currentField.FieldType == GameFieldType.HOUSE)
            {
                foreach (GameField allFields in gameField.Values)
                {
                    if (allFields.Color.ToString().Equals(figure.HexColor) && allFields.FieldType == GameFieldType.STARTFIELD)
                    {
                        nextField = allFields;
                        currentField.IsTaken = false;
                        nextField.IsTaken = true;
                        return nextField;
                    }
                }
            }
            else if (currentField.FieldType == GameFieldType.STARTFIELD)
            {

                foreach (GameField fields in fieldsAround)
                {
                    if ((fields.Coordinates.X != figure.LastGameField.Coordinates.X || fields.Coordinates.Y != figure.LastGameField.Coordinates.Y)
                        && (fields.FieldType != GameFieldType.ENDFIELD && fields.FieldType != GameFieldType.BANK)
                        && fields.IsVisible == Visibility.Visible)
                    {
                        //MessageBox.Show("Es gibt so ein Feld!");
                        nextField = fields;
                        currentField.IsTaken = false;
                        nextField.IsTaken = true;
                        return nextField;
                    }
                }

            }
            else if (currentField.FieldType == GameFieldType.FIELD)
            {
                foreach (GameField fields in fieldsAround)
                {
                    //MessageBox.Show(fields.Coordinates.ToString());
                    if ((fields.Coordinates.X != figure.LastGameField.Coordinates.X || fields.Coordinates.Y != figure.LastGameField.Coordinates.Y)
                        && (fields.FieldType != GameFieldType.HOUSE && fields.FieldType != GameFieldType.BANK)
                        && fields.IsVisible == Visibility.Visible)
                    {
                        //MessageBox.Show("Es gibt so ein Feld!");
                        nextField = fields;
                        currentField.IsTaken = false;
                        nextField.IsTaken = true;
                        return nextField;
                    }
                }

            }
            else if (currentField.FieldType == GameFieldType.ENDFIELD)
            {
                foreach (GameField fields in fieldsAround)
                {
                    if (fields.Color.ToString().Equals(figure.HexColor) && fields.FieldType == GameFieldType.BANK)
                    {
                        nextField = fields;
                        currentField.IsTaken = false;
                        nextField.IsTaken = true;
                        return nextField;
                    }
                    else
                    {
                        if ((fields.Coordinates.X != figure.LastGameField.Coordinates.X || fields.Coordinates.Y != figure.LastGameField.Coordinates.Y)
                        && (fields.FieldType != GameFieldType.HOUSE && fields.FieldType != GameFieldType.BANK)
                        && (fields.FieldType == GameFieldType.STARTFIELD && !fields.Color.ToString().Equals(figure.HexColor))
                        && fields.IsVisible == Visibility.Visible)
                        {
                            nextField = fields;
                            currentField.IsTaken = false;
                            nextField.IsTaken = true;
                            return nextField;
                        }
                    }
                }
            }
            else if (currentField.FieldType == GameFieldType.BANK)
            {
                foreach (GameField fields in fieldsAround)
                {
                    //MessageBox.Show("Feld im Umfeld: " + fields.Coordinates.ToString());
                    //MessageBox.Show((fields.FieldType == GameFieldType.BANK).ToString());
                    if (fields.Color.ToString().Equals(figure.HexColor) 
                        && (fields.FieldType == GameFieldType.BANK) 
                        && (fields.Coordinates.X != figure.LastGameField.Coordinates.X || fields.Coordinates.Y != figure.LastGameField.Coordinates.Y)
                        && (fields.IsTaken == false))
                    {
                        nextField = fields;
                        currentField.IsTaken = false;
                        nextField.IsTaken = true;
                        return nextField;
                    }
                }
                figure.FigureButton.IsEnabled = false;

            }
            return currentField;
        }

        private List<GameField> ScanFieldsAround(Figure figure)
        {
            List<GameField> fieldsAround = new List<GameField>();
            Coordinate2D currentPos = figure.FigureCoordinate;
            // FELD RECHTS
            if (currentPos.X + 1 <= 10)
                fieldsAround.Add(ReturnField(new Coordinate2D(currentPos.X + 1, currentPos.Y)));
            // FELD LINKS
            if (currentPos.X - 1 >= 0)
                fieldsAround.Add(ReturnField(new Coordinate2D(currentPos.X - 1, currentPos.Y)));
            // FELD OBEN
            if ((currentPos.Y - 1 >= 0))
                fieldsAround.Add(ReturnField(new Coordinate2D(currentPos.X, currentPos.Y - 1)));
            // FELD UNTEN
            if (currentPos.Y + 1 <= 10)
                fieldsAround.Add(ReturnField(new Coordinate2D(currentPos.X, currentPos.Y + 1)));

            return fieldsAround;
        }

        /// <summary>
        /// Button-Handler um dem Würfel-Button Logik hinzuzufügen.
        /// </summary>
        /// <param name="sender">Der Würfel-Button als Objekt</param>
        /// <param name="e"></param>
        internal void RollDice(object sender, RoutedEventArgs e)
        {
            RolledDice = GetRolledDice();
            DiceRolledEvent?.Invoke();
        }

        private int GetRolledDice()
        {
            // Instanziere ein neues Random Objekt und lass es 2x von 1-6 würfeln.
            var rand = new Random();
            int dice = rand.Next(1, 7);

            // Überprüfe ob die Würfel-Ergebnisse gleich sind.
            if (dice == 6)
                MessageBox.Show(String.Format("Würfel: {0}, 6!", dice.ToString()));
            else
                MessageBox.Show(String.Format("Würfel: {0}", dice.ToString()));
            return dice;
        }
    }
}
