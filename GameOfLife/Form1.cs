using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private int currentGeneration = 0;
        private Graphics _graphis;
        private int _resolution;
        private bool[,] field;
        private int _rows;
        private int _cols;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer1.Enabled)
                return;


            currentGeneration = 0;
            Text = $"Generation: {currentGeneration}";
            nudResolution.Enabled = false;
            nudDensity.Enabled = false;

            _resolution = (int)nudResolution.Value;
            _rows = pictureBox1.Height / _resolution;
            _cols = pictureBox1.Width / _resolution;
            field = new bool[_cols, _rows];

            Random random = new Random();

            for (int x = 0; x < _cols; x++)
                for (int y = 0; y < _rows; y++)
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphis = Graphics.FromImage(pictureBox1.Image);

            timer1.Start();
        }

        private void NextGeneration()
        {
            _graphis.Clear(Color.Black);

            var newField = new bool[_cols, _rows];

            for (int x = 0; x < _cols; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    var neighboursCount = CountNeigbours(x, y);
                    var hasLife = field[x, y];

                    if (!hasLife && neighboursCount == 3)
                        newField[x, y] = true;
                    else if (neighboursCount < 2 || neighboursCount > 3)
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];

                    if (hasLife)
                        _graphis.FillRectangle(Brushes.Crimson, x * _resolution, y * _resolution, _resolution, _resolution);
                }
            }

            field = newField;
            pictureBox1.Refresh();
            Text = $"Generation: {++currentGeneration}";
        }

        private int CountNeigbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int col = (x + i + _cols) % _cols;
                    int row = (y + j + _rows) % _rows;

                    bool isSelfChecking = col == x && row == y;
                    bool hasLife = field[col, row];

                    if (hasLife && !isSelfChecking)
                        count++;
                }
            }

            return count;
        }

        private void StopGame()
        {
            if (!timer1.Enabled)
                return;

            timer1.Stop();

            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void timer1_Tick(object sender, System.EventArgs e) => NextGeneration();

        private void bStart_Click(object sender, System.EventArgs e) => StartGame();

        private void bStop_Click(object sender, System.EventArgs e) => StopGame();

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / _resolution;
                var y = e.Location.Y / _resolution;

                var validationPassed = ValidateMousePosition(x, y);
                if (validationPassed)
                    field[x, y] = true;
                 
             
            }

            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / _resolution;
                var y = e.Location.Y / _resolution;
                
                var validationPassed = ValidateMousePosition(x, y);
                if (validationPassed)
                    field[x, y] = false;

            }

        }

        private bool ValidateMousePosition(int x, int y)
        {
            return x>=0 && y >=0 && x< _cols && y < _rows;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation: {currentGeneration}";
        }
    }
}