using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Engine
{
    public class Engine
    {
        private bool[,] field;

        private readonly int _rows;
        private readonly int _cols;

        public Engine(int rows, int cols, int density)
        {
            _rows = rows;
            _cols = cols;
            
            field = CreateFirstGeneration(density);
        }
        public int CurrentGenerationNumber { get; private set; }
        public int GenerationNumber { get; private set; }
        public bool[,] Generation
        {
            get
            {
                bool[,] result = new bool[_cols, _rows];

                for (int x = 0; x < _cols; x++)
                    for (int y = 0; y < _rows; y++)
                        result[x, y] = field[x, y];
                return result;
            }
        }

        private bool [,] CreateFirstGeneration(int density)
        {
            Random random = new Random();
            
             for (int x = 0; x < _cols; x++)
                for (int y = 0; y < _rows; y++)
                    field[x, y] = random.Next(density) == 0;

            GenerationNumber = 1;
            return field;
        }

        public void CreateNextGeneration()
        {
            int minNeneighboursCount = 2;
            int maxNeighboursCount = 3;

            bool[,] newField = new bool[_cols, _rows];

            for (int x = 0; x < _cols; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    int neighboursCount = CalculateNeigboursCount(x, y);
                    bool hasLife = field[x, y];

                    if (!hasLife && neighboursCount == maxNeighboursCount)
                        newField[x, y] = true;
                    else if (neighboursCount < minNeneighboursCount || neighboursCount > maxNeighboursCount)
                        newField[x, y] = false;
                    else
                        newField[x, y] = field[x, y];
                }
            }
        }
          private int CalculateNeigboursCount(int entityX, int entityY)
          {
            int neigboursCount = 0;

            int startPosition = -1;
            int endPosition = 2;

            for (int i = startPosition; i < endPosition; i++)
            {
                for (int j = startPosition; j < endPosition; j++)
                {
                    int colIdx = (entityX + i + _cols) % _cols;
                    int rowIdx = (entityY + j + _rows) % _rows;

                    bool isSelfChecking = colIdx == entityX && rowIdx == entityY;
                    bool hasLife = field[colIdx, rowIdx];

                    if (hasLife && !isSelfChecking)
                        neigboursCount++;
                }
            }
            return neigboursCount;
           }

        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _cols && y < _rows;
        }

        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
                field[x, y] = state;
        }
        public void AddEntity(int x, int y)
        {
            UpdateCell(x, y, state: true);
        }
        public void killEntity(int x, int y)
        {
            UpdateCell(x, y, state: false);
        }

    }
}
