using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWPF.Models
{
    public class Grid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Cols { get; }
        public int this[int r, int c]
        {
            get { return grid[r, c]; }
            set { grid[r, c] = value; }
        }
        public Grid(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            grid = new int[Rows, Cols];
        }
        public bool IsValidIndex(int rows, int cols)
        {
            return rows >=0 && rows <= Rows && cols >= 0 && cols <= Cols;
        }
        public bool IsRowFull(int row)
        {
            for (int col = 0; col < Cols; col++)
            {
                // If any index of the row is empty return false
                if (grid[row, col] == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsRowEmpty(int row)
        {
            for (int col = 0; col < Cols; col++)
            {
                // If any index of the row is taken return false
                if (grid[row, col] != 0)
                {
                    return false;
                }
            }
            return true;
        }
        public void ClearRow(int row)
        {
            for(int col = 0; col < Cols; col++)
            {
                grid[row, col] = 0;
            }
        }
        public void MoveRowDown(int row, int rowsToMove)
        {
            // We move the row down "rowsToMove" times.
            for (int col = 0; col < Cols; col++)
            {
                grid[row + rowsToMove, col] = grid[row, col];
                grid[row, col] = 0;
            }
        }
        public int ClearFullRows()
        {
            int cleard = 0;

            // For every row we have cleard, we move the other rows "cleard" times down.
            for (int r = Rows - 1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleard++;
                }
                else if(cleard > 0)
                {
                    MoveRowDown(r, cleard);
                }
            }
            return cleard;
        }
    }
}
