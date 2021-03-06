﻿using MenschensKinder.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MenschensKinder
{
    /// <summary>
    /// Die UI-Klasse des Spielbretts. Verwaltet das zeichnen der jeweiligen UI-Elemente auf dem Spielbrett wie die Ellipsen oder Button. Das Zeichnen geschiet auf einem Grid,
    /// da dieser wie eine Tabelle angeordnet ist (X,Y-Positionen). Vereinfacht es die Ellipsen anzuordnen.
    /// </summary>
    public partial class GamePage : Page
    {
        // Fülle eine Liste mit den zu zeichnenden Ellipsen (Spielfeld)
        private IEnumerable<Ellipse> ellipses;
        // Erstelle ein Objekt der Klasse GameBoard um die Ellipsen zu einem GameField zuzuweisen (Logikverknüpfung)
        private readonly GameBoard boardManager;
        // Deklariere ein Objekt der Klasse Player um diesen zeichnen zu können
        private readonly Player player;
        // Deklariere eine Liste mit Computergegenern
        private readonly List<Player> kiplayer;
        // Deklariere eine Liste mit dem Spieler und den Computergegnern
        private readonly List<Player> allPlayers;
        // Würfel Button
        private Button diceButton;
        private readonly BackgroundWorker worker;

        public TaskCompletionSource<object> figureChosen;

        

        /// <summary>
        /// Konstruktor der Klasse GamePage. Initialisiert alle nötigen UI-Komponenten
        /// </summary>
        /// <param name="color">Empfängt die ausgewählte Farbe aus der ColorPage und weist diese dem Player zu.</param>
        public GamePage(PlayerColor color)
        {
            InitializeComponent();
            // Instanziiere Objekte
            worker = new BackgroundWorker();

            player = new Player(color);
            kiplayer = new List<Player>();
            allPlayers = new List<Player>();
            boardManager = new GameBoard(allPlayers);
            
            
            // Zeichne die UI-Elemente
            InitGameboard();
            InitPlayers();
            //DrawPlayer();
            //DrawKI();
            //GameLoop();
            Game game = new Game(allPlayers, boardManager, this);
            worker.DoWork += game.Start;
            //game.KiClickedFigureEvent += OnKiClickedFigure;
            worker.RunWorkerAsync();
#if DEBUG
            Coordinate2D coordinates = boardManager.ReturnPosition(ellipses.LastOrDefault());
#endif
        }

        public void OnKiClickedFigure(object sender, EventArgs e)
        {
            MessageBox.Show("Eine KI hat gewählT!");
        }



        /// <summary>
        /// Initialisere das Spielbrett. Sprich alle Ellipsen werden gezeichnet und der Würfel Button erzeugt.
        /// </summary>
        private void InitGameboard()
        {
            // 10x10 Spielbrett, da wir bei 0 zählen müssen wir bis < 11 zählen.
            ellipses = CreateField(11);
            // Füge jede kreierte Ellipse dem Grid hinzu.
            foreach (Ellipse ellipse in ellipses)
                grid.Children.Add(ellipse);   
        }



        private void InitPlayers()
        {
            allPlayers.Add(player);
            foreach (Figure figure in player.ReturnFigures())
            {
                DrawButtonForFigure(figure);
                figure.CurrentGameField = boardManager.ReturnField(figure.FigureCoordinate);
                figure.LastGameField = figure.CurrentGameField;
            }

            foreach(PlayerColor colors in Enum.GetValues(typeof(PlayerColor)))
            {
                if (colors != player.Color)
                    kiplayer.Add(new Player(colors));
            }
            foreach(Player ki in kiplayer)
            {
                allPlayers.Add(ki);
                foreach(Figure kifigure in ki.ReturnFigures())
                {
                    DrawButtonForFigure(kifigure);
                    kifigure.CurrentGameField = boardManager.ReturnField(kifigure.FigureCoordinate);
                    kifigure.LastGameField = kifigure.CurrentGameField;
                }
            }
            
            
        }

        /// <summary>
        /// Zeichnet den jeweiligen Button an der Position der Player Figure
        /// </summary>
        /// <param name="figure">Die Figur für welche der Button gezeichnet werden soll</param>
        public void DrawButtonForFigure(Figure figure)
        {
            Button figBtn = figure.FigureButton;
            Grid.SetColumn(figBtn, figure.FigureCoordinate.X);
            Grid.SetRow(figBtn, figure.FigureCoordinate.Y);
            grid.Children.Add(figBtn);
        }

        /// <summary>
        /// Erstelle eine Liste von Ellipsen die iterierbar ist. Die Ellipsen in der Liste haben bereits die angepassten Eigenschaften, sodass diese nur dem Grid hinzugefügt
        /// werden müssen.
        /// </summary>
        /// <param name="number">Die Größe des Spielfelds.</param>
        /// <returns>Iterierbare Liste mit Ellipsen Objekten</returns>
        private IEnumerable<Ellipse> CreateField(int number)
        {
            // Weise den UI-Elementen ihre zugehörigen Styles (Designs) zu.
            Style ellipseStyle = this.FindResource("FieldEllipse") as Style;
            Style diceBtnStyle = App.Current.MainWindow.FindResource("NoHoverButton") as Style;
            for (int i = 0; i < number; i++)
            {
                // Weise dem Grid dynamisch Spalten und Zeilen zu. Die jeweilge Breite und Höhe wird abhängig von der Gridgröße gemacht.
                ColumnDefinition colDef = new ColumnDefinition()
                {
                    Width = new GridLength(grid.Width / number),
                };

                RowDefinition rowDef = new RowDefinition()
                {
                    Height = new GridLength(grid.Height / number),
                };

                grid.ColumnDefinitions.Add(colDef);
                grid.RowDefinitions.Add(rowDef);

                // Beginne nun die UI-Elemente zu initialisieren.
                for(int j = 0; j < number; j++)
                {
                    // Farbe der Ellipse festlegen durch die Position im Grid
                    SolidColorBrush colorBrush = new SolidColorBrush()
                    {
                        Color = DetermineColor(i, j),
                    };

                    // Erstelle die Ellipse mit ihren Eigenschaften
                    Ellipse ellipse = new Ellipse()
                    {
                        // Entscheide ob die Ellipse ein Style anhand der Position. Da eine Ellipse genau in der Mitte gezeichnet wird, darf diese kein Style bekommen, da dort der Button ist.
                        Style = (i == 5 && j == 5) ? null : ellipseStyle,
                        Width = Math.Round(grid.Width / number),
                        Height = Math.Round(grid.Height / number),
                        Fill = colorBrush,
                        // Zeichne viele Ellipsen nicht sichtbar, da diese nicht zum Spielfeld gehören.
                        Visibility = DetermineVisibility(i,j) ? Visibility.Visible : Visibility.Hidden,
                    };

                    // Überprüfe die Position der For-Schleifen, um den Würfel-Button zu positionieren.
                    if (i == 5 && j == 5)
                    {
                        diceButton = new Button()
                        {
                            // Weise dem Button seinen XAML-Style zu.
                            Content = new Image() { Source = new BitmapImage(new Uri("/img/dice.png", UriKind.RelativeOrAbsolute)) },
                            Background = Brushes.Transparent,
                            BorderThickness = new Thickness(0.0),
                            Style = diceBtnStyle,
                        };
                        // Click-Handler für den Würfel-Button
                        diceButton.Click += boardManager.RollDice;

                        // Füge den Würfel-Button dem Grid hinzu
                        Grid.SetColumn(diceButton, i);
                        Grid.SetRow(diceButton, j);
                        grid.Children.Add(diceButton);
                    }

                    // Setze die Positionen für die jeweiligen Ellipsen.
                    Grid.SetColumn(ellipse, i);
                    Grid.SetRow(ellipse, j);

                    // Verknüpfe die jeweilige Ellipse mit dem GameBoard Objekt, um die Ellipsen logisch einem GameField Objekt zuweisen zu können.
                    // Dabei wird ebenfalls die jeweilige Position der Ellipse übergeben.
                    boardManager.AddField(ellipse, i, j);

                    // Bei der DEBUG-Version, zeichne DEBUG-Textblöcke um nachvollziehen zu können, ob die Ellipse dem GameField Objekt richtig zugeordnet wurde.
#if DEBUG

                    TextBlock txtBlock = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = String.Format(boardManager.ReturnField(ellipse).ToString()),

                    };

                    Grid.SetColumn(txtBlock, i);
                    Grid.SetRow(txtBlock, j);

                    grid.Children.Add(txtBlock);
#endif

                    // Füge die jeweilig erzeugte Ellipse dem Objekt "ellipses" hinzu.
                    yield return ellipse;
                }
            }
        }

        /// <summary>
        /// Hardcoded Methode um zu entscheiden, welche Ellipse mit welcher Farbe gefüllt werden soll.
        /// </summary>
        /// <param name="x">X-Position der Ellipse</param>
        /// <param name="y">Y-Position der Ellipse</param>
        /// <returns>Die jeweilige Farbe mit der zu zeichnen ist.</returns>
        private Color DetermineColor(int x, int y)
        {
            // Rotes Team
            if (((x == 0 || x == 1) && (y == 0 || y == 1)) 
                || (y == 5 && (x == 1 || x == 2|| x == 3|| x == 4)) 
                || (y == 4 && x == 0))
                return Color.FromArgb(255, 255, 0, 0);
            // Blaues Team
            else if (((x == 9 || x == 10) && (y == 0 || y == 1))
                || x == 5 && (y == 1 || y == 2 || y == 3 || y == 4)
                || (y == 0 && x == 6))
                return Color.FromArgb(255, 0, 0, 255);
            // Grünes Team
            else if (((x == 0 || x == 1) && (y == 9 || y == 10))
                || (x == 5 && (y == 6 || y == 7 || y == 8 || y == 9))
                || (y == 10 && x == 4))
                return Color.FromArgb(255, 0, 255, 0);
            // Gelbes Team
            else if (((x == 9 || x == 10) && (y == 9 || y == 10))
                || (y == 5 && (x == 6 || x == 7 || x == 8 || x == 9))
                || (y == 6 && x == 10))
                return Color.FromArgb(255, 255, 255, 0);
            else
                return Color.FromArgb(255, 255, 255, 255);
        }

        /// <summary>
        /// Hardcoded Methode um zu entscheiden, welche Ellipse sichtbar sein soll.
        /// </summary>
        /// <param name="x">X-Position der Ellipse</param>
        /// <param name="y">Y-Position der Ellipse</param>
        /// <returns>Bool-Wert ob sie sichtbar ist, oder nicht.</returns>
        private bool DetermineVisibility(int x, int y)
        {
            if (x == 5 && y == 5)
                return false;
            else if (((x == 2 || x == 3) && (y == 0 || y == 1 || y == 2 || y == 3)) 
                || ((x == 0 || x == 1) && (y == 2 || y == 3)))
                return false;
            else if (((x == 7 || x == 8) && (y == 0 || y == 1 ||y == 2 || y == 3))
                || ((x == 9 || x == 10) && (y == 2 || y == 3)))
                return false;
            else if (((x == 2 || x == 3) && (y == 7 || y == 8 || y == 9 || y == 10))
                || ((x == 0 || x == 1) && (y == 7 || y == 8)))
                return false;
            else if (((x == 7 || x == 8) && (y == 7 || y == 8 || y == 9 || y == 10))
                || ((x == 9 || x == 10) && (y == 7 || y == 8)))
                return false;
            else
                return true;
        }
    }
}
