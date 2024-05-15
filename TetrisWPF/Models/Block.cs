using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWPF.Models
{
    public abstract class Block
    {
        private int rotationState;
        private Position offset;
        protected abstract Position[][] TetrominoTiles { get; }
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }
        public abstract string Color { get; }

        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Col);
        }
        public IEnumerable<Position> TilesPositions()
        {
            foreach (var position in TetrominoTiles[rotationState])
            {
                yield return new Position(position.Row + offset.Row, position.Col + offset.Col);
            }
        }

        public void RotateCW()
        {
            rotationState = (rotationState + 1) % TetrominoTiles.Length;
        }
        public void RotateCCW() 
        {
            if (rotationState == 0)
            {
                rotationState = TetrominoTiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }
        public void Move(int row, int col)
        {
            // Update our position as we move the block
            offset.Row = row;
            offset.Col = col;
        }
        public void Reset()
        {
            // Reset the position for the every new block
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Col = StartOffset.Col;
        }
    }
}
