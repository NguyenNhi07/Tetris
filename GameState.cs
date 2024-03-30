namespace Tetris
{
    public class GameState
    {
        private Block currentBlock;     //lưu trữ khối hiện tại 

        //cho phép bên ngoài đọc giá trị
        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                //Gán giá trị cho khối hiện tại rồi reset nó để có thể lưu trữ khối tiếp theo 
                currentBlock = value;
                currentBlock.Reset();

                //Thử di chuyển khối sang phải hai lần để tránh khối đè và trượt ra khỏi lưới chơi
                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);    //di chuyển khối sang phải 1 đơn vị 

                    if (!BlockFits())   //Nếu kiểm tra thấy khối bị chặn
                    {
                        currentBlock.Move(-1, 0);   //thì di chuyển khối sang trái 
                    }
                }
            }
        }

        //Các thuộc tính
        public GameGrid GameGrid { get; }               //chứa thông tin lưới
        public BlockQuece BlockQuece { get; }           //chứa thông tin khối sẽ xuất hiện tiếp theo
        public bool GameOver { get; private set; }      //kiểm tra trạng thái game
        public int Score { get; private set; }          //theo dõi điểm số
        public Block HeldBlock { get; private set; }    //chứa thông tin khối bị giữ
        public bool CanHold { get; private set; }       //xác định có thể giữ khối hay không và ngăn chặn giữ nhiều khối

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQuece = new BlockQuece();
            CurrentBlock = BlockQuece.GetAndUpdate();   //khối vừa được lấy ra sẽ được gán thông tin luôn và là khối để bắt đầu trò chơi
            CanHold = true;
        }

        //Kiểm tra khối hiện tại có vừa lưới chơi và không đè lên khối khác
        private bool BlockFits()
        {
            //Duyệt qua vị trí của từng ô khối
            foreach (Position p in CurrentBlock.TilePositions())
            {
                //kiểm tra xem vị trí của ô đó trên lưới có trống không
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
        }


        //Giữ khối
        public void HoldBlock()
        {
            if (!CanHold)   //không thể giữ vì đang có khối được giữ
            {
                return;
            }

            if (HeldBlock == null)  //không có khối nào được giữ
            {
                HeldBlock = CurrentBlock;   //gán giá trị khối hiện tại cho HeldBlock để đưa ra khỏi lưới
                CurrentBlock = BlockQuece.GetAndUpdate();   //gán cho khối hiện tại một khối mới trong hàng đợi
            }
            //có khối đang giữ
            else
            {
                Block tmp = CurrentBlock;   //gán tạm khối hiện tại cho biến tmp
                CurrentBlock = HeldBlock;   //lấy khối đang giữ gán cho khối hiện tại để lấy khối đang giữ ra
                HeldBlock = tmp;            //gán heldBlock cho tmp để lấy khối muốn giữ ban đầu
            }

            CanHold = false;
        }

        //xoay theo kim đồng hồ
        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            //Nếu không vừa thì xoay ngược lại 
            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
        }

        //xoay ngược kim đồng hồ
        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        //di chuyển sang trái
        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            //Nếu khi di chuyển sang trái không vừa thì di chuyển ngược lại
            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }

        //di chuyển sang phải
        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }

        //Kiểm tra trạng thái trò chơi
        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));     //hàng 0 và 1 không trống thì trò chơi kết thúc
        }

        //Khối hiện tại không thể di chuyển xuống tiếp
        private void PlaceBlock()
        {
            // lặp qua vị trí các ô của khối sau đó đặt chúng trong lưới trò chơi bằng ID khối
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            //Xoá các hàng đã đầy và cộng điểm vào score từ việc xoá hàng
            Score += GameGrid.ClearFullRows();


            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQuece.GetAndUpdate();   //game chưa kết thúc thì lấy một khối tiếp theo từ hàng đợi và gán vào khối hiện tại
                CanHold = true;
            }
        }

        //di chuyển khối xuống 1 hàng
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        //tính khoảng cách của từng ô từ vị trí hiện tại đến điểm nó sẽ rơi xuống mà k có va chạm
        private int TileDropDistance(Position p)
        {
            int drop = 0;
            //nếu ô dưới khối là trống thì biến drop sẽ tăng lên 1 đơn vị
            while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }
            return drop;
        }

        //Quyết định khoảng cách mà khối có thể tự do rơi xuống mà k gặp va chạm
        public int BlockDropDistane()
        {
            int drop = GameGrid.Rows;

            //duyệt qua vị trí các ô của khối hiện tại
            foreach (Position p in CurrentBlock.TilePositions())
            {
                //giá trị nhỏ nhất giữa giá trị của dòng va khoảng cách tính được là khoảng cách khối có thể rơi
                drop = System.Math.Min(drop, TileDropDistance(p));
            }
            return drop;
        }

        //di chuyển khối xuống dưới điểm rơi ngay lập tức
        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistane(), 0);
            PlaceBlock();      //kiểm tra cập nhật điểm số, lấy khối mới
        }
    }
}
