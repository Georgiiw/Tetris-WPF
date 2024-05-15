using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWPF.Models.Blocks
{
    public class O_Block : Block
    {
        private readonly Position[][] terminoTiles = new Position[][]
        {
            [new(0,0), new(0,1), new(1,0), new(1,1)]

        };
        public override string Color => "#FFFF00";
        public override int Id => 4;
        protected override Position StartOffset => new Position(0, 4);
        protected override Position[][] TetrominoTiles => terminoTiles;
    }
}
