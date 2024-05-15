using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisWPF.Models
{
    public class Position
    {
        private int row;
        private int col;

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }
        public int Row 
        {
            get { return row; }
            set { row = value; }
        }
        public int Col 
        { 
            get { return col; }
            set {  col = value; }
        }
    }
}
