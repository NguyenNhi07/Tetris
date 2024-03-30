using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block
    {
        protected abstract Position[][] Tiles { get; }  //toạ độ các ô
        protected abstract Position StartOffset { get; }    //vị trí bắt đầu
        public abstract int Id { get; }     //id để định danh khối

        private int rotationState;  //trạng thái xoay của khối
        private Position offset;    //thông tin vị trí của khối


        // biến offset 
        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }


        // chứa vị trí của các ô trong khối ở trạng thái xoay
        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        // xoay khối theo chiều kim đồng hồ
        public void RotateCW()
        {
            // trạng thái xoay của khối có 4 trạng thái, cứ tăng lên 1 thì tương ứng với một giá trị xoay của khối trong mảng Tiles
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        // xoay khối theo chiều ngược kim đồng hồ
        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                //gán giá trị của rotationState thành trạng thái cuối cùng của mảng 
                rotationState =  Tiles.Length - 1;
            }
            else
            {
                //không phải trạng thế ban đầu thì giảm đi 1 đơn vị 
                rotationState--;
            }
        }

        //di chuyển khối theo số dòng và số cột
        public void Move (int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }


        //đặt lại ví trị và trạng thái của khối về ban đầu
        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
 