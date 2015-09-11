using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {

        private const int GRID_OFFSET = 25; // Distance from upper-left side of window
        private const int GRID_LENGTH = 200; // Size in pixels of grid

        private int numCells = 3; // Number of cells in grid
        private int cellLength;

        private bool[,] grid; // Stores on/off state of cells in grid
        private Random rand; // Used to generate random numbers

        #region Constructors

        public MainForm()
        {
            InitializeComponent();
            rand = new Random(); // Initializes random number generator
            BuildBoard();
        }

        #endregion

        #region Other Events

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int r = 0; r < numCells; r++)
                for (int c = 0; c < numCells; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid section
                    Brush brush;
                    Pen pen;
                    if (grid[r, c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White; // On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black; // Off
                    }
                    // Determine (x,y) coord of row and col to draw rectangle
                    int x = c * cellLength + GRID_OFFSET;
                    int y = r * cellLength + GRID_OFFSET;
                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, cellLength, cellLength);

                    g.FillRectangle(brush, x + 1, y + 1, cellLength - 1, cellLength - 1);
                }
        }

        #endregion

        #region Click Events

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnNewGame_Click(sender, e);
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            // Fill grid with either white or black
            for (int r = 0; r < numCells; r++)
                for (int c = 0; c < numCells; c++)
                    grid[r, c] = rand.Next(2) == 1;
            // Redraw grid
            this.Invalidate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {

            // Make sure click was inside the grid
            if (e.X < GRID_OFFSET || e.X > cellLength * numCells + GRID_OFFSET ||
            e.Y < GRID_OFFSET || e.Y > cellLength * numCells + GRID_OFFSET)
                return;
            // Find row, col of mouse press
            int r = (e.Y - GRID_OFFSET) / cellLength;
            int c = (e.X - GRID_OFFSET) / cellLength;
            // Invert selected box and all surrounding boxes
            for (int i = r - 1; i <= r + 1; i++)
                for (int j = c - 1; j <= c + 1; j++)
                    if (i >= 0 && i < numCells && j >= 0 && j < numCells)
                        grid[i, j] = !grid[i, j];
            // Redraw grid
            this.Invalidate();
            // Check to see if puzzle has been solved
            if (PlayerWon())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numCells = 3;
            BuildBoard();
            this.Invalidate();
        }

        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numCells = 4;
            BuildBoard();
            this.Invalidate();
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numCells = 5;
            BuildBoard();
            this.Invalidate();
        }


        #endregion

        #region Helper Functions

        private void BuildBoard()
        {
            cellLength = GRID_LENGTH / numCells;

            grid = new bool[numCells, numCells];

            // Turn entire grid on
            for (int r = 0; r < numCells; r++)
            {
                for (int c = 0; c < numCells; c++)
                {
                    grid[r, c] = true;
                }
            }
        }

        private bool PlayerWon()
        {
            foreach(bool slot in grid)
            {
                if(slot)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}