using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        //Hình ảnh chứa hình ảnh các ô. Urikind.Relative là một giá trị liệt kê trong không gian tên System của C#
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png" , UriKind .Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png" , UriKind.Relative)),
        };

        //Mảng chứa hình ảnh các khối
        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png" , UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png" , UriKind.Relative)),
        };

        private readonly Image[,] imageControls;

        //Thời gian chờ dùng để điều chỉnh tốc độ rơi của khối
        private readonly int maxDelay = 1000;       //tốc độ rơi ban đầu
        private readonly int minDelay = 75;         // tốc độ rơi cuối cùng
        private readonly int delayDecrease = 25;    //sau mỗi lần đặt khối, tốc độ rơi sẽ giảm đi vs giá trị là 25

        private GameState gameState = new GameState();  //trạng thái của trò chơi

        //thiết lập cửa sổ và lưới chơi
        public MainWindow()
        {
            InitializeComponent();  //tạo tự động bởi WPF, khởi tạo giao diện người dùng
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;  //chiều rộng mỗi ô trong lưới chơi
            
            //duyệt lần lượt từng ô, rồi tạo imageControl mới và có thể định vị chính xác nó
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        //Phương thức vẽ lưới trò chơi, cập nhật giao diện người dùng
        private void DrawGrid(GameGrid grid) 
        {
            //duyệt qua từng ô trên lưới
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;    //đặt độ mờ của hình ảnh là 1 để hiển thị rõ nhất
                    imageControls[r, c].Source = tileImages[id];    //gán nguồn của hình ảnh đó dựa trên id
                }
            }
        }

        //vẽ khối hiện tại 
        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        //Cập nhật giao diện hiển thị khối tiếp theo 
        private void DrawNextBlock (BlockQuece blockQuece)
        {
            Block next = blockQuece.NextBlock;      //lấy khối tiếp theo từ hàng đợi
            NextImage.Source = blockImages[next.Id];    //gán nguồn hình ảnh
        }

        //Cập nhật giao diện hiển thị khối đang được giữ
        private void DrawHeldBlock (Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];  //gán nguồn là khối trống
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];   //gán nguồn là khối đang giữ
            }
        }

        //Dự đoán vị trí mà khối sẽ rơi xuống
        private void DrawGhostBlock (Block block)
        {
            //lấy khoảng cách sẽ rơi xuống đã được tính toán
            int dropDistance = gameState.BlockDropDistane();

            //duyệt qua các ô trong khối
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;      //gán độ mờ cho mỗi ô là 0.25 biệu thị khối ảo
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];    //gán nguồn cho khối qua ID
            }
        }

        //Vẽ lưới chơi và khối hiện tại để cập nhật giao diện
        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);           //lưới trò chơi
            DrawGhostBlock(gameState.CurrentBlock); //khối ảo
            DrawBlock(gameState.CurrentBlock);      //Khối hiện tại
            DrawNextBlock(gameState.BlockQuece);    //khối tiếp theo
            DrawHeldBlock(gameState.HeldBlock);     //khối bị giữ
            ScoreText.Text = $"Score: {gameState.Score}";   //điểm
        }

        //Thực hiện vòng lặp chính của trò chơi
        private async Task GameLoop()
        {
            Draw(gameState);    //cập nhật giao diện

            while (!gameState.GameOver)
            {
                //Điểm số tăng thì thời gian delay giảm dần (tốc độ rơi nhanh hơn)
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);    //dùng để khiến người chơi chờ một khoảng thời gian 
                gameState.MoveBlockDown();  //di chuyển khối hiện tại xuống một hàng
                Draw(gameState);            //cập nhật giao diện sau mỗi lần di chuyển
            }

            //Game kết thúc thì hiển thị giao diện menu kết thúc và hiển thị điểm số cuối cùng
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }

        //Xử lý khi nhấn phím
        private void Windown_KeyDown(object sender, KeyEventArgs e) 
        {
            if (gameState.GameOver)
            {
                return;
            }
            
            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down: 
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }

        // tải thành công giao diện người dùng GameCanvas 
        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        //Nút bấm PlayAgain
        private async void PlayAgain_Click(object sender, RoutedEventArgs e) 
        { 
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}