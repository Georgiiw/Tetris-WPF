
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
           Colors.White,
           Color.FromRgb(0xAD, 0xD8, 0xE6),
           Color.FromRgb(0x00, 0x00, 0x8B),
           Color.FromRgb(0xFF, 0xA5, 0x00),
           Color.FromRgb(0xFF, 0xFF, 0x00),
           Color.FromRgb(0x00, 0xFF, 0x00),
           Color.FromRgb(0x99, 0x32, 0xCC),
           Color.FromRgb(0xFF, 0x00, 0x00),
           
        };
        private readonly Rectangle[,] recControls;
        private GameState gameState = new GameState();
        public MainWindow()
        {
            InitializeComponent();
           recControls = SetUpGameCanvas(gameState.Grid);
        }
        private Rectangle[,] SetUpGameCanvas(Models.Grid grid)
        {
           Rectangle[,] recControls = new Rectangle[grid.Rows, grid.Cols];
            int cellSize = 30;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Cols; c++)
                {
                    Rectangle rec = new Rectangle
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    Canvas.SetTop(rec, (r - 2) * cellSize);
                    Canvas.SetLeft(rec, c * cellSize);
                    GameCanvas.Children.Add(rec);
                    recControls[r, c] = rec;
                }
            }
            return recControls;
        }
        private void DrawGrid(Models.Grid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Cols; c++)
                {
                    int id = grid[r, c];
                    Color color = colors[id];
                    recControls[r, c].Fill = new SolidColorBrush(color);
                }
            }
        }
        private void DrawBlock(Block block)
        {
            foreach (var position in block.TilesPositions())
            {
                Color color = colors[block.Id];
                recControls[position.Row, position.Col].Fill = new SolidColorBrush(color);
            }
        }
        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.Grid);
            DrawBlock(gameState.CurrBlock);
        }
        private async Task StartGame()
        {
            Draw(gameState);

            while(!gameState.GameOver)
            {
                await Task.Delay(100);
                gameState.MoveDown();
                Draw(gameState);
            }
            GameOverMenu.Visibility = Visibility.Visible;
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch(e.Key)
            {
                case Key.A:
                    gameState.MoveLeft();
                    break;
                case Key.D:
                    gameState.MoveRight();
                    break;
                case Key.S:
                    gameState.MoveDown();
                    break;
                case Key.W:
                    gameState.RotateBlockCW();
                    break;
                case Key.Q:
                    gameState.RotateBlockCCW();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                default: return;
            }
            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await StartGame();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await StartGame();
        }
    }
}