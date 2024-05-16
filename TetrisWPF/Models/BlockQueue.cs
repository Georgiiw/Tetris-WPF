using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TetrisWPF.Models.Blocks;

namespace TetrisWPF.Models
{
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new I_Block(),
            new J_Block(),
            new L_Block(),
            new O_Block(),
            new S_Block(),
            new T_Block(),
            new Z_Block(),
        };

        private readonly Random random = new Random();
        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        // Preview of the next block
        public Block NextBlock {  get; private set; }
        public Block RandomBlock()
        {
            // Returns a random block
            return blocks[random.Next(blocks.Length)];
        }
        public Block GetAndUpdate()
        {
            Block block = NextBlock;

            // Pick a random block until its different from the previous
            do { NextBlock = RandomBlock(); }
            while (block.Id == NextBlock.Id);

            return block;
        }
    }
}
