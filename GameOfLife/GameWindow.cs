using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class GameWindow : Form
    {
        #region Fields
        int w = 50, h = 50;
        int interval = 1, gens = 0;
        int liveCells = 0, rules = 0;
        int rng = 0;
        bool drawGrid = true;

        Random rand = new Random();

        Color cell = Color.Red, trail = Color.Aqua, bg = Color.Black;

        Grid universe, scratch;
        #endregion

        public GameWindow()
        {
            InitializeComponent();
            #region Initialize

            GenLabel.Text = "Generations: " + gens;
            CellLabel.Text = "Cells: " + liveCells;
            TimeLabel.Text = "Time: " + interval;

            timer.Interval = 25;
            timer.Stop();

            rng = rand.Next();

            universe = new Grid(w, h, Grid.Boundary.Toroidal);
            scratch = new Grid(w, h, Grid.Boundary.Toroidal);

            float width = ((float)graphicsPanel1.ClientSize.Width) / ((float)w);
            float height = ((float)graphicsPanel1.ClientSize.Height) / ((float)h);

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    universe[x, y] = new Cell((int)(x * width), (int)(y * height));
                    scratch[x, y] = new Cell((int)(x * width), (int)(y * height));
                }

            #endregion
        }

        private void Tick(object sender, EventArgs e)
        {
            GenLabel.Text = "Generations: " + gens++;
            CellLabel.Text = "Cells: " + liveCells;

            Generate();
            graphicsPanel1.Invalidate();
        }


        #region Generation

        void Generate()
        {
            liveCells = 0;
            switch (rules)
            {
                case 0:
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            int n = Neighbors(x, y);

                            if (universe[x, y].Status)
                            {
                                if (n < 2)
                                {
                                    scratch[x, y].Status = false;
                                }
                                else if (n > 3)
                                {
                                    scratch[x, y].Status = false;
                                }
                                else
                                {
                                    scratch[x, y].Status = true;
                                    liveCells++;
                                }
                            }
                            else
                            {
                                if (n == 3)
                                {
                                    scratch[x, y].Status = true;
                                    liveCells++;
                                }
                                else
                                {
                                    scratch[x, y].Status = false;
                                }
                            }
                        }
                    }

                    break;
            }

            Swap();
        }

        int Neighbors(int _x, int _y)
        {
            int count = 0;

            switch (universe.boundsType)
            {
                case Grid.Boundary.Toroidal:
                    if (universe[(_x - 1) < 0 ? universe.width - 1 : _x - 1, (_y - 1) < 0 ? universe.height - 1 : _y - 1].Status)
                    {
                        count++;
                    }
                    if (universe[(_x - 1) < 0 ? universe.width - 1 : _x - 1, _y].Status)
                    {
                        count++;
                    }
                    if (universe[(_x - 1) < 0 ? universe.width - 1 : _x - 1, (_y + 1) > universe.height - 1 ? 0 : _y + 1].Status)
                    {
                        count++;
                    }
                    if (universe[_x, (_y - 1) < 0 ? universe.height - 1 : _y - 1].Status)
                    {
                        count++;
                    }
                    if (universe[_x, (_y + 1) > universe.height - 1 ? 0 : _y + 1].Status)
                    {
                        count++;
                    }
                    if (universe[(_x + 1) > universe.width - 1 ? 0 : _x + 1, (_y - 1) < 0 ? universe.height - 1 : _y - 1].Status)
                    {
                        count++;
                    }
                    if (universe[(_x + 1) > universe.width - 1 ? 0 : _x + 1, _y].Status)
                    {
                        count++;
                    }
                    if (universe[(_x + 1) > universe.width - 1 ? 0 : _x + 1, (_y + 1) > universe.height - 1 ? 0 : _y + 1].Status)
                    {
                        count++;
                    }
                    break;
                case Grid.Boundary.Finite:
                    for (int i = _y - 1; i < _y + 2; i++)
                    {
                        if (i < 0 || i >= universe.height)
                            continue;
                        for (int j = _x - 1; j < _x + 2; j++)
                        {
                            if (j >= universe.width || j < 0 || (_x == j && _y == i))
                                continue;
                            if (universe[j, i].Status)
                            {
                                count++;
                            }
                            else
                            {
                                count += 0;
                            }
                        }
                    }
                    break;
            }

            return count;
        }

        void Swap()
        {
            Grid temp = universe;
            universe = scratch;
            scratch = temp;
        }

        #endregion

        private void ClickEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float width = ((float)graphicsPanel1.ClientSize.Width) / ((float)w);
                float height = ((float)graphicsPanel1.ClientSize.Height) / ((float)h);

                float px = e.X / width;
                float py = e.Y / height;

                universe[(int)px, (int)py].Status = !universe[(int)px, (int)py].Status;

                graphicsPanel1.Invalidate();
            }
        }
        private void MoveEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                float width = ((float)graphicsPanel1.ClientSize.Width) / ((float)w);
                float height = ((float)graphicsPanel1.ClientSize.Height) / ((float)h);

                float px = e.X / width;
                float py = e.Y / height;

                if (!universe[(int)px, (int)py].Status)
                    universe[(int)px, (int)py].Status = !universe[(int)px, (int)py].Status;

                graphicsPanel1.Invalidate();
            }
        }
        private void DrawCall(object sender, PaintEventArgs e)
        {
            float width = ((float)graphicsPanel1.ClientSize.Width) / ((float)w);
            float height = ((float)graphicsPanel1.ClientSize.Height) / ((float)h);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (drawGrid)
                        e.Graphics.DrawLine(new Pen(bg), x * width, 0, x * width, graphicsPanel1.ClientSize.Height);

                    if (universe[x, y].was == 1)
                    {
                        scratch[x, y].was = universe[x, y].was;
                        e.Graphics.FillRectangle(new SolidBrush(trail), x * width, y * height, width, height);
                    }
                    if (universe[x, y].Status == false)
                        continue;
                    if (universe[x, y].Status)
                    {
                        if (universe[x, y].was == 0)
                            universe[x, y].was = 1;

                        e.Graphics.FillRectangle(new SolidBrush(cell), x * width, y * height, width, height);
                    }
                }
                if (drawGrid)
                    e.Graphics.DrawLine(new Pen(bg), 0, y * height, graphicsPanel1.ClientSize.Width, y * height);
            }
        }

        #region Events

        private void Play(object sender, EventArgs e)
        {
            timer.Start();
            PauseButton.Enabled = true;
            PlayButton.Enabled = false;
        }

        private void Pause(object sender, EventArgs e)
        {
            timer.Stop();
            PlayButton.Enabled = true;
            PauseButton.Enabled = false;
        }

        private void Step(object sender, EventArgs e)
        {
            if (!PauseButton.Enabled)
            {
                Tick(sender, e);
            }
        }


        #endregion

    }
}
