namespace Tetris
{
    //Lưu trữ thông tin về vị trí trong lưới trò chơi
    public class Position
    {
        public int Row { get; set; }   
        public int Column { get; set; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}


