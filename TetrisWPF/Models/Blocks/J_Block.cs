using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWPF.Models.Blocks
{
    public class J_Block : Block
    {
        private readonly Position[][] terminoTiles = new Position[][]
        {
            [new(0,0), new(1,0), new(1,1), new(1,2)],
            [new(0,1), new(0,2), new(1,1), new(2,1)],
            [new(1,0), new(1,1), new(1,2), new(2,2)],
            [new(2,0), new(2,1), new(1,1), new(0,1)]
        };
        public override string Color => "#00008B";
        public override int Id => 2;
        protected override Position StartOffset => new Position(0, 3);
        protected override Position[][] TetrominoTiles => terminoTiles;
    }
}
