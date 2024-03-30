namespace Tetris
{
    //Biểu diễn lưới trò chơi
    public class GameGrid
    {
        private readonly int[,] grid;   //mảng hai chiều biểu diễn lưới trò chơi

        //đọc số hàng và cột của lưới
        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public GameGrid (int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        // kiểm tra xem vị trí này có nằm trong giới hạn của lưới không
        public bool IsInside (int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        //Kiểm tra xem 1 ô trong lưới có trống không
        public bool IsEmpty (int r, int c) 
        {
            return IsInside (r, c) && grid[r, c] == 0;
        }

        //Kiểm tra xem 1 hàng có full giá trị không
        public bool IsRowFull (int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        //Kiểm tra xem 1 hàng có trống không
        public bool IsRowEmpty (int r)
        {
            for(int c = 0; c < Columns; c++)
            {
                if (grid[r, c] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        //Xoá một hàng trong lưới khi đầy
        private void ClearRow (int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        } 

        //Di chuyển một hàng xuống dưới khi xoá một hàng
        private void MoveRowDown (int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        //Xoá các hàng đã đầy và di chuyển hàng phía trên xuống để lấp đầy khoảng trống
        public int ClearFullRows()
        {
            int cleared = 0;    //đếm số lượng hàng đã xoá

            for (int r = Rows-1; r >= 0; r--)   //duyệt từ dưới lên trên 
            {
                if (IsRowFull (r))  //kiểm tra xem hành đầy không
                {
                    ClearRow (r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown (r, cleared);
                }
            }
            return cleared;
        }
    }
}
