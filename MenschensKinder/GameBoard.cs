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

    }
}
