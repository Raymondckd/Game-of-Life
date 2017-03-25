using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public class GraphicsPanel : Panel
    {
        public GraphicsPanel()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
    }

    public class Cell
    {
        Point pos;
        bool lifeStatus = false;
        public int was = 0;

        public Cell(int _x, int _y)
        {
            pos.X = _x;
            pos.Y = _y;
            lifeStatus = false;
            was = 0;
        }

        public Point Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public bool Status
        {
            get { return lifeStatus; }
            set { lifeStatus = value; }
        }

    }

    public class Grid
    {
        public enum Boundary
        {
            Toroidal,
            Finite
        };
        public Boundary boundsType;
        public int width, height;
        Cell[] cells;

        public Cell this[int _x, int _y]
        {
            get { return cells[_y * width + _x]; }
            set { cells[_y * width + _x] = value; }
        }

        public Grid(int w, int h, Boundary b)
        {
            width = w;
            height = h;
            boundsType = b;
            cells = new Cell[w * h];
        }
    }

}
