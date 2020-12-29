using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace MenschensKinder
{
    class GameBoard
    {
        private IDictionary<Ellipse, GameField> gameField = new Dictionary<Ellipse, GameField>();

        public void AddField(Ellipse ellipse, int x, int y)
        {
            gameField.Add(ellipse, new GameField(x, y));
        }

        public Coordinate2D ReturnPosition(Ellipse ellipse)
        {
            if(gameField.ContainsKey(ellipse))
            {       
                if (gameField.TryGetValue(ellipse, out GameField result))
                {
                    return result.Coordinates;
                }
            }
            return new Coordinate2D(0, 0);
        }

        public GameField ReturnField(Ellipse ellipse)
        {
            if(gameField.ContainsKey(ellipse))
            {
                if(gameField.TryGetValue(ellipse, out GameField field))
                {
                    return field;
                }
            }
            return null;
        }

        internal void RollDice(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            int dice1 = rand.Next(1, 6);
            int dice2 = rand.Next(1, 6);

            if(dice1 == dice2)
                MessageBox.Show(String.Format("{0}, {1}, PASCH!", dice1.ToString(), dice2.ToString()));
            else
                MessageBox.Show(String.Format("{0}, {1}", dice1.ToString(), dice2.ToString()));

        }
    }
}
