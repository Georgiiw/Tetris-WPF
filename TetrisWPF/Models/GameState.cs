using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWPF.Models
{
    public class GameState
    {
        private Block currBlock { get; set; }

        public Block CurrBlock 
        { 
            get {  return currBlock; }  
            private set 
            { 
                value = currBlock;
                // Call the Reset method to set starting position and default rotation state
                currBlock.Reset();

                // Set the spawn position to be the top (block height) visible rows
                for (int i = 0; i < 2; i++)
                {
                    currBlock.Move(1, 0);
                    if (!IsValidPosition())
                    {
                        currBlock.Move(-1, 0);
                    }
                }
            }
        }
        public Grid Grid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }

        public GameState()
        {
            // Set game grid
            Grid = new Grid(22, 10);

            // Set block queue to generate blocks
            BlockQueue = new BlockQueue();
            CurrBlock = BlockQueue.GetAndUpdate();
        }
        public bool IsValidPosition()
        {
            foreach (var position in CurrBlock.TilesPositions())
            {
                // Check if the rotate position is illegal(there is another block in our way or its outside the grid)
                if (!Grid.IsEmpty(position.Row, position.Col))
                {
                    return false;
                }
            }
            return true;
        }
        public void RotateBlockCW()
        {
            CurrBlock.RotateCW();
            // Rotate the block and if its in invalid position rotate it back
            if (!IsValidPosition())
            {
                CurrBlock.RotateCCW();
            }
        }
        public void RotateBlockCCW()
        {
            CurrBlock.RotateCCW();
            // Rotate the block and if its in invalid position rotate it back
            if (!IsValidPosition())
            {
                CurrBlock.RotateCW();
            }
        }
        public void MoveLeft()
        {
            CurrBlock.Move(0, -1);
            // Move the block left and if its in invalid position move it back
            if (!IsValidPosition())
            {
                CurrBlock.Move(0, 1);
            }
        }
        public void MoveRight()
        {
            CurrBlock.Move(0, 1);
            // Move the block right and if its in invalid position move it back
            if (!IsValidPosition())
            {
                CurrBlock.Move(0, -1);
            }
        }
        public bool IsGameOver()
        {
            // If the top two rows aren't empty the game is over
            return !(Grid.IsRowEmpty(0) && Grid.IsRowEmpty(1));
        }
        private void PlaceBlock()
        {
            // Call this method when our block cannot move down anymore
            foreach (var position in CurrBlock.TilesPositions())
            {
                // Current block takes its position in the grid
                Grid[position.Row, position.Col] = CurrBlock.Id;
            }

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                // If game isn't over generate new block
                CurrBlock = BlockQueue.GetAndUpdate();
            }
        }
        public void MoveDown()
        {
            CurrBlock.Move(1, 0);

            if (!IsValidPosition())
            {
                // If the position (1, 0) is not valid we call PlaceBlock method
                CurrBlock.Move(-1, 0);
                PlaceBlock();
            }
        }
        public int TilesDropDistance(Position position)
        {
            int drop = 0;

            // Get the number of empty tiles between the current tile and the last empty tile beneath it
            while (Grid.IsEmpty(position.Row + drop + 1, position.Col))
            {
                drop++;
            }

            return drop;
        }
        public int BlockDropDistance()
        {
            int drop = Grid.Rows;

            // Get the minimum distance between each of the block tiles and the closest empty tile beneath them
            foreach (var position in CurrBlock.TilesPositions())
            {
                drop = Math.Min(drop, TilesDropDistance(position));
            }

            return drop;
        }
        public void DropBlock()
        {
            // Move the current block down as many rows as possible and place it in the grid
            CurrBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
