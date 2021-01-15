using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MenschensKinder.data
{
    class Game
    {
        private readonly List<Player> allPlayers;
        private readonly GameBoard gameBoard;
        private Player userPlayer;
        private bool gameOver;


        public Game(List<Player> allPlayers, GameBoard gameBoard)
        {
            this.allPlayers = allPlayers;
            this.gameBoard = gameBoard;
            this.userPlayer = allPlayers.First();
            this.gameOver = false;
        }

        public void Start(object sender, DoWorkEventArgs e)
        {
            MessageBox.Show(sender.GetType().ToString());
            if(allPlayers != null && gameBoard != null)
            {
                foreach (Player players in allPlayers)
                {
                    foreach (Figure figure in players.ReturnFigures())
                    {
                        figure.FigureClickedEvent += OnFigureClicked;
                    }
                }
                gameBoard.DiceRolledEvent += OnDiceRolled;
            }
            else
                throw new Exception("Da ist was ganz mies falsch gelaufen bruder");

            while (!gameOver)
            {
                CheckForWin();
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void CheckForWin()
        {
            bool finished = false;
            foreach(Player player in allPlayers)
            {
                List<Figure> figures = player.ReturnFigures();
                if(figures != null)
                {
                    finished = figures.All(figure => figure.CurrentGameField.FieldType == GameFieldType.BANK);
                }
                if (finished)
                {
                    gameOver = true;
                    MessageBox.Show("Spieler mit Farbe " + player.Color.ToString() + " gewinnt!");
                }
            }
        }
        
        private void OnFigureClicked(Object sender, EventArgs e) 
        {
            //MessageBox.Show("Ich reagiere auch!");
        }

        private void OnDiceRolled()
        {
            //MessageBox.Show("ICH EBENFALLS!");
        }

    }
}
