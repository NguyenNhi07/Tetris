using System;

namespace Tetris
{
    // Lớp hàng đợi dùng để chọn khối tiếp theo xuất hiện
    public class BlockQuece
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };
        
        private readonly Random random = new Random();  //chọn số ngẫu nhiên

        public Block NextBlock { get; private set; }    //giá trị của khối tiếp theo

        public BlockQuece()
        {
            NextBlock = RandomBlock();  //gán giá trị của khối ngẫu nhiên cho khối tiếp theo
        }

        //Khối ngẫu nhiên
        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];    //số ngẫu nhiên sẽ làm chỉ số chọn phần tử ngẫu nhiên từ mảng blocks
        }

        //Trả về khối NextBlock rồi cập nhật NextBlock sao cho giá trị mới không trùng với giá trị trước
        public Block GetAndUpdate()
        {
            //gán khối hiện tại bằng khối NextBlock
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();  //gán Khối NextBlock cho khối ngẫu nhiên
            }
            while (block.Id == NextBlock.Id);    // gán cho đến khi Khối NextBlock không trùng với khối hiện tại thì dừng lại

            return block;
        }
    }
}
