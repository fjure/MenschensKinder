using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MenschensKinder.data
{
    class Game
    {
        private readonly List<Player> allPlayers;
        private List<Player> sortedPlayerList = new List<Player>();
        private readonly GameBoard gameBoard;
        private readonly GamePage uiElement;
        private GameState gameState;
        private Player userPlayer;
        private Player currentPlayer;
        private bool gameOver;
        private int rolledDice;
        private int turn;

        //public event EventHandler<EventArgs> KiClickedFigureEvent;


        public Game(List<Player> allPlayers, GameBoard gameBoard, GamePage uiElement)
        {
            this.allPlayers = allPlayers;
            this.gameBoard = gameBoard;
            this.userPlayer = allPlayers.First();            
            this.gameOver = false;
            this.turn = 0;
            this.rolledDice = 0;
            this.uiElement = uiElement;
            this.gameState = GameState.START;
        }

        public void Start(object sender, DoWorkEventArgs e)
        {
            //MessageBox.Show(sender.GetType().ToString());
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

            SetPlayerOrder();

            sortedPlayerList = SortPlayersByOrder(allPlayers);
            currentPlayer = sortedPlayerList.First();

            ShowPlayerOrder();

            turn = 1;


            while (!gameOver)
            {
#if DEBUG
                // KI IST DRAN
                if(!currentPlayer.Color.Equals(userPlayer.Color))
                {
                    // Die KI übernimmt derzeit die Würfelergebnisse des UserPlayer ???
                    // Zudem macht die KI nichts solang der Player nix gemacht hat!
                    Application.Current.Dispatcher.Invoke(new Action(() => OnDiceRolled(null, EventArgs.Empty)));
                    //OnDiceRolled(null, EventArgs.Empty);
                    //System.Threading.Thread.Sleep(50);
                    Figure randomFigure = currentPlayer.ReturnFigures().ElementAt(new Random().Next(currentPlayer.ReturnFigures().Count));
                    try
                    {
                        //Application.Current.Dispatcher.BeginInvoke(new Action(() => randomFigure.CurrentGameField = gameBoard.DetermineNextField(
                        //randomFigure, gameBoard.ReturnField(randomFigure.FigureCoordinate), false)));
                        Application.Current.Dispatcher.Invoke(new Action(() => OnFigureClicked(randomFigure, EventArgs.Empty)));
                        System.Threading.Thread.Sleep(100);
                        //ReDrawFigures();
                        
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.StackTrace);
                    }

                }
#endif
                CheckForWin();
                System.Threading.Thread.Sleep(50);
            }
        }

        private List<Player> SortPlayersByOrder(List<Player> unorderedPlayers)
        {
            return unorderedPlayers.OrderBy(player => player.Order).ToList();
        }

        private void ShowPlayerOrder()
        {
            string orderText = "";
            foreach (Player player in sortedPlayerList)
            {
                orderText += "Spielerfarbe: " + player.Color.ToString() + " Reihenfolge: " + player.Order + "\n";
            }
            MessageBox.Show(orderText);
        }

        private void SetPlayerOrder()
        {
            userPlayer = allPlayers.First();
            List<int> orderPriority = new List<int>{ 1, 2, 3, 4 };
            foreach(Player player in allPlayers)
            {
                Random randomIndexer = new Random();
                int index = randomIndexer.Next(orderPriority.Count);
                player.Order = orderPriority.ElementAt(index);
                orderPriority.RemoveAt(index);
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
            Figure figureClicked = (Figure)sender;
            //MessageBox.Show("Gewählt wurde: " + figureClicked.FigureCoordinate.ToString());
            if (gameState == GameState.START)
            {
                if(figureClicked.Color.ToString().Equals(currentPlayer.Color.ToString()))
                {
                    MessageBox.Show("DU darfst!");
                }
                else
                {
                    MessageBox.Show("DU NICHT!");
                }
            }
            else if(gameState == GameState.DICEROLLED)
            {
                
                if (figureClicked.Color.ToString().Equals(currentPlayer.Color.ToString()))
                {
                    //MessageBox.Show("Geklickte Figure ist currentPlayer");
                    this.gameState = GameState.FIGURECHOSEN;
                    MoveFigure(figureClicked, gameBoard.RolledDice, gameBoard.RolledDice);
                    AdvancePlayer();
                }
            }
        }

        private void AdvancePlayer()
        {
            int currentPlayerIndex = sortedPlayerList.FindIndex(player => player.Color.ToString().Equals(currentPlayer.Color.ToString()));
            int lastIndex = sortedPlayerList.FindIndex(player => player.Color.ToString().Equals(sortedPlayerList.Last().Color.ToString()));
            //MessageBox.Show("LastIndex: " + lastIndex.ToString() + " List Count: " + sortedPlayerList.Count.ToString());
            int nextPlayerIndex = 0;
            if(currentPlayerIndex == sortedPlayerList.Count - 1)
            {
                //MessageBox.Show("hier sollte ich nicht sein?");
                nextPlayerIndex = 0;
            }
            else
            {
                nextPlayerIndex = currentPlayerIndex + 1;
            }
            currentPlayer = sortedPlayerList.ElementAt(nextPlayerIndex);
            MessageBox.Show("Als nächstes ist der Spieler mit der Farbe " + currentPlayer.Color.ToString() + " an der Reihe!");
            System.Threading.Thread.Sleep(100);
        }

        private void MoveFigure(Figure sender, int rolledDice, int leftDice)
        {
            Figure figureClicked = sender;
            GameField currentField = gameBoard.ReturnField(figureClicked.FigureCoordinate);
            if (leftDice == 0)
            {
                return;
            }
            else if(leftDice == 1)
            {
                GameField newField = gameBoard.DetermineNextField(figureClicked, currentField, true);
                if (currentField.Coordinates.Equals(newField.Coordinates))
                {
                    MoveFigure(figureClicked, rolledDice, 0);
                }
                else
                {
                    figureClicked.LastGameField = currentField;
                    figureClicked.CurrentGameField = newField;
                    figureClicked.FigureCoordinate = newField.Coordinates;
                    ReDrawFigures();
                    MoveFigure(figureClicked, rolledDice, leftDice - 1);
                }
            }
            else
            {
                GameField newField = gameBoard.DetermineNextField(figureClicked, currentField, false);
                if (currentField.Coordinates.Equals(newField.Coordinates))
                {
                    MoveFigure(figureClicked, rolledDice, 0);
                }
                else
                {
                    figureClicked.LastGameField = currentField;
                    figureClicked.CurrentGameField = newField;
                    figureClicked.FigureCoordinate = newField.Coordinates;
                    ReDrawFigures();
                    MoveFigure(figureClicked, rolledDice, leftDice - 1);
                }
            }
            
        }

        private void ReDrawFigures()
        {
            foreach (Player players in sortedPlayerList)
            {
                foreach (Figure figures in players.ReturnFigures())
                {
                    uiElement.grid.Children.Remove(figures.FigureButton);
                    uiElement.DrawButtonForFigure(figures);
                }
            }
        }

        private void OnDiceRolled(Object sender, EventArgs e)
        {
            this.gameState = GameState.DICEROLLED;
            rolledDice = GetRolledDice();
            MessageBox.Show("Spieler mit der Farbe: " + currentPlayer.Color.ToString() + " würfelt eine " + rolledDice);
            MessageBox.Show("Bitte wähle eine deiner Figuren um diese zu bewegen!");
        }

        private int GetRolledDice()
        {
            return new Random().Next(1, 7);
        }

    }
}
