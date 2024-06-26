﻿
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TetrisWPF.Models;
using TetrisWPF.Models.Blocks;

namespace TetrisWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Color[] colors =
        {
            // Bloack colors to fill the required tiles
           Colors.Black,
           Color.FromRgb(0xFF, 0xFF, 0xFF),
           Color.FromRgb(0x00, 0x00, 0x8B),
           Color.FromRgb(0xFF, 0xA5, 0x00),
           Color.FromRgb(0xFF, 0xFF, 0x00),
           Color.FromRgb(0x00, 0xFF, 0x00),
           Color.FromRgb(0x80, 0x00, 0x80),
           Color.FromRgb(0xFF, 0x00, 0x00),
           
        };      
        private readonly ImageSource[] blockImages =
        {
            // Image of each block
            new BitmapImage(new Uri("/Assets/1.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Assets/2.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Assets/3.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Assets/4.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Assets/5.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Assets/6.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Assets/7.png", UriKind.Relative))
        };
        private readonly Rectangle[,] recControls;
        private GameState gameState = new GameState();
        private readonly int maxDelay = 1000;
        public MainWindow()
        {
            InitializeComponent();
            recControls = SetUpGameCanvas(gameState.Grid);
            mediaElement.Source = new Uri("Assets/gameSong.wav", UriKind.Relative);
            mediaElement.Play();
        }
        private Rectangle[,] SetUpGameCanvas(Models.Grid grid)
        {
           Rectangle[,] recControls = new Rectangle[grid.Rows, grid.Cols];
            int cellSize = 30;

            // Fill a matrix of rectangles, each representing a tile, to draw on the canvas
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Cols; c++)
                {
                    Rectangle rec = new Rectangle
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Stroke = Brushes.Gray,
                        StrokeThickness = 0.5,
                    };

                    Canvas.SetTop(rec, (r - 2) * cellSize);
                    Canvas.SetLeft(rec, c * cellSize);
                    GameCanvas.Children.Add(rec);
                    recControls[r, c] = rec;
                }
            }
            return recControls;
        }
        private void DrawNextBlock(BlockQueue blockQueue)
        {
            // Get and draw the image of the next block
            Block next = blockQueue.NextBlock;
            NextBlockImg.Source = blockImages[next.Id - 1];
        }
        private void DrawGrid(Models.Grid grid)
        {
            // Draw the game grid on the canvas 
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Cols; c++)
                {
                    int id = grid[r, c];
                    Color color = colors[id];
                    recControls[r, c].Opacity = 1;
                    recControls[r, c].Fill = new SolidColorBrush(color);
                }
            }
        }
        private void DrawBlock(Block block)
        {
            foreach (var position in block.TilesPositions())
            {
                recControls[position.Row, position.Col].Opacity = 1;
                // Draw current block
                Color color = colors[block.Id];
                recControls[position.Row, position.Col].Fill = new SolidColorBrush(color);
                // Draw ghost block of the current block
                if (gameState.BlockDropDistance() > 1)
                {
                    recControls[gameState.BlockDropDistance() + position.Row, position.Col].Fill = new SolidColorBrush(color);
                    recControls[gameState.BlockDropDistance() + position.Row, position.Col].Opacity = 0.40;  
                }
            }
        }
        private void Draw(GameState gameState)
        {
            // Method to draw everyting at once when generating a new state of the game
            DrawGrid(gameState.Grid);  
            DrawBlock(gameState.CurrBlock);
            DrawNextBlock(gameState.BlockQueue);
            ScoreText.Text = $"{gameState.Score}";
                
        }
        private async Task StartGame()
        {
            // Draw current game state
            Draw(gameState);

            // Until the game is over we generate our next state
            while(!gameState.GameOver)
            {         
                int delay = maxDelay - (gameState.Score / 10);
                await Task.Delay(delay);

                if (!gameState.IsPaused)
                {
                    gameState.MoveDown();
                    Draw(gameState);   
                    if (gameState.BlockDropDistance() == 0)
                    {
                        await Task.Delay(500);
                    }
                }
            }

            // Show game over menu when the game is over
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScore.Text = $"Score: {gameState.Score}";
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            // Attach our movement actions to buttons
            switch(e.Key)
            {
                case Key.A:
                    if (!gameState.IsPaused)
                    {
                        gameState.MoveLeft();                      
                    }
                    break;
                case Key.D:
                    if (!gameState.IsPaused)
                    {
                        gameState.MoveRight();
                    }
                    break;
                case Key.S:
                    if (!gameState.IsPaused)
                    {
                        gameState.MoveDown(); gameState.Score++;
                    }             
                    break;
                case Key.W:
                    if (!gameState.IsPaused)
                    {
                        gameState.RotateBlockCW();
                    }
                    break;
                case Key.Q:
                    if (!gameState.IsPaused)
                    {
                        gameState.RotateBlockCCW();
                    }
                    break;
                case Key.Space:
                    if (!gameState.IsPaused)
                    {
                        gameState.DropBlock();
                    }
                    break;
                case Key.Escape:
                    gameState.Pause(); PauseMenu.Visibility = gameState.IsPaused ? Visibility.Visible: Visibility.Hidden;
                    break;
                default: return;
            }
            Draw(gameState);
        }
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            // Start the game when it loads
            await StartGame();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            // In game over menu click to start the game again and hide this menu
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await StartGame();
        }
        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.Zero;
            mediaElement.Play();
        }

        private void Mute_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Volume != 0)
            {
                mediaElement.Volume = 0;
            }
            else
            {
                mediaElement.Volume = 0.4;
            }

        }
    }
}