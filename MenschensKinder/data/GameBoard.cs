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
        private readonly List<Player> allPlayer;

        public event EventHandler<EventArgs> DiceRolledEvent;

        private int rolledDice = 0;
        public int RolledDice
        {
            get => rolledDice;
            set => this.rolledDice = value;
        }

        public GameBoard(List<Player> player)
        {
            allPlayer = player;
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

        public bool CanKick(Figure kicker, GameField field, bool kickEnabled)
        {
            //MessageBox.Show(kickEnabled.ToString());
            if (field.IsTaken && kickEnabled)
            {
                //MessageBox.Show("ICH KANN KICKEN!");
                Figure figureOnField = ReturnFigureForGameField(field);
                if(figureOnField.FigureCoordinate.Equals(field.Coordinates))
                {
                    return true;
                }
            }
            //MessageBox.Show("Kein Kick gefunden!");
            return false;     
        }

        public void Kick(Figure kicker, Figure figureOnField ,GameField field)
        {
            figureOnField.FigureCoordinate = figureOnField.StartCoordinate;
            kicker.FigureCoordinate = field.Coordinates;
            kicker.CurrentGameField = field;
        }

        public Figure ReturnFigureForGameField(GameField field)
        {
            foreach(Player player in allPlayer)
            {
                foreach(Figure figure in player.ReturnFigures())
                {
                    if(figure.FigureCoordinate.Equals(field.Coordinates))
                    {
                        return figure;
                    }
                }
            }
            return null;
        }

        public GameField DetermineNextField(Figure figure, GameField currentField, bool kickEnabled)
        {
            GameField nextField = currentField;
            //List<GameField> fieldsAround = ScanFieldsAround(figure);
            figure.FieldsAround = ScanFieldsAround(figure);
            List<GameField> fieldsAround = figure.FieldsAround;
            if (currentField.FieldType == GameFieldType.HOUSE)
            {
                foreach (GameField allFields in gameField.Values)
                {
                    if (allFields.Color.ToString().Equals(figure.HexColor) && allFields.FieldType == GameFieldType.STARTFIELD)
                    {
                        if(allFields.IsTaken && kickEnabled)
                        {
                            if (CanKick(figure, allFields, false))
                            {
                                Kick(figure, ReturnFigureForGameField(allFields), allFields);
                            }
                        }
                        nextField = allFields;
                    }
                }
            }
            else if (currentField.FieldType == GameFieldType.BANK)
            {
                foreach (GameField fields in fieldsAround)
                {
                    if (fields.Color.ToString().Equals(figure.HexColor)
                        && (fields.FieldType == GameFieldType.BANK)
                        && !fields.Coordinates.Equals(figure.LastGameField.Coordinates)
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
            else if(currentField.FieldType != GameFieldType.HOUSE)
            {
                foreach (GameField fields in fieldsAround)
                {
                    if(currentField.FieldType == GameFieldType.STARTFIELD)
                    {
                        if (!fields.Coordinates.Equals(figure.LastGameField.Coordinates)
                        && (fields.FieldType != GameFieldType.ENDFIELD && fields.FieldType != GameFieldType.BANK)
                        && fields.IsVisible)
                        {
                            if (CanKick(figure, fields, kickEnabled))
                            {
                                Kick(figure, ReturnFigureForGameField(fields), fields);
                            }
                            nextField = fields;
                        }
                    }
                    else if(currentField.FieldType == GameFieldType.FIELD)
                    {
                        if (!fields.Coordinates.Equals(figure.LastGameField.Coordinates)
                        && (fields.FieldType != GameFieldType.HOUSE && fields.FieldType != GameFieldType.BANK)
                        && fields.IsVisible)
                        {
                            if (CanKick(figure, fields, kickEnabled))
                            {
                                Kick(figure, ReturnFigureForGameField(fields), fields);
                            }
                            nextField = fields;
                        }
                    }
                    else if(currentField.FieldType == GameFieldType.ENDFIELD)
                    {
                        if (fields.Color.ToString().Equals(figure.HexColor) && fields.FieldType == GameFieldType.BANK)
                        {
                            if (CanKick(figure, fields, kickEnabled))
                            {
                                Kick(figure, ReturnFigureForGameField(fields), fields);
                            }
                            nextField = fields;
                        }
                        else
                        {
                            if (!fields.Coordinates.Equals(figure.LastGameField.Coordinates)
                            && (fields.FieldType != GameFieldType.HOUSE && fields.FieldType != GameFieldType.BANK)
                            && (fields.FieldType == GameFieldType.STARTFIELD && !fields.Color.ToString().Equals(figure.HexColor))
                            && fields.IsVisible)
                            {
                                if (CanKick(figure, fields, kickEnabled))
                                {
                                    Kick(figure, ReturnFigureForGameField(fields), fields);
                                }
                                nextField = fields;
                            }
                        }
                    }
                }
            }
            currentField.IsTaken = false;
            nextField.IsTaken = true;
            return nextField;
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
            DiceRolledEvent?.Invoke(sender, e);
        }

        private int GetRolledDice()
        {
            // Instanziere ein neues Random Objekt und lass es 2x von 1-6 würfeln.
            var rand = new Random();
            int dice = rand.Next(1, 7);

            // Überprüfe ob die Würfel-Ergebnisse gleich sind.
            /*if (dice == 6)
                MessageBox.Show(String.Format("Würfel: {0}, 6!", dice.ToString()));
            else
                MessageBox.Show(String.Format("Würfel: {0}", dice.ToString()));*/
            return dice;
        }
    }
}
