using System;

namespace GameOfLife.Engine
{
    public class Game
    {
        private readonly int _colsCount;
        private readonly int _rowsCount;

        private bool[,] _generation;

        public Game(int rowsCount, int colsCount, int density)
        {
            _rowsCount = rowsCount;
            _colsCount = colsCount;
            _generation = CreateFirstGeneration(density);
        }

        public uint GenerationNumber { get; private set; }

        public bool[,] Generation
        {
            get
            {
                bool[,] generation = new bool[_colsCount, _rowsCount];

                for (int x = 0; x < _colsCount; x++)
                    for (int y = 0; y < _rowsCount; y++)
                        generation[x, y] = _generation[x, y];

                return generation;
            }
        }

        private bool[,] CreateFirstGeneration(int density)
        {
            Random random = new Random();
            bool[,] generation = new bool[_colsCount, _rowsCount];

            GenerationNumber = 1;

            for (int x = 0; x < _colsCount; x++)
                for (int y = 0; y < _rowsCount; y++)
                    generation[x, y] = random.Next(density) == 0;

            return generation;
        }

        public void CreateNextGeneration()
        {
            int minNeighborsCount = 2;
            int maxNeighborsCount = 3;

            bool[,] newGeneration = new bool[_colsCount, _rowsCount];

            for (int x = 0; x < _colsCount; x++)
            {
                for (int y = 0; y < _rowsCount; y++)
                {
                    int neighboursCount = CalculateNeighborsCount(x, y);
                    bool isHasLife = _generation[x, y];

                    if (!isHasLife && neighboursCount == maxNeighborsCount)
                        newGeneration[x, y] = true;
                    else if (neighboursCount < minNeighborsCount || neighboursCount > maxNeighborsCount)
                        newGeneration[x, y] = false;
                    else
                        newGeneration[x, y] = _generation[x, y];
                }
            }

            _generation = newGeneration;
            GenerationNumber++;
        }

        private int CalculateNeighborsCount(int entityX, int entityY)
        {
            int startPosition = -1;
            int endPosition = 2;
            int neighborsCount = 0;

            for (int x = startPosition; x < endPosition; x++)
            {
                for (int y = startPosition; y < endPosition; y++)
                {
                    int colIdx = (entityX + x + _colsCount) % _colsCount;
                    int rowIdx = (entityY + y + _rowsCount) % _rowsCount;

                    bool isSelfChecking = colIdx == entityX && rowIdx == entityY;
                    bool isHasLife = _generation[colIdx, rowIdx];

                    if (isHasLife && !isSelfChecking)
                        neighborsCount++;
                }
            }

            return neighborsCount;
        }

        private bool ValidateCellPosition(int x, int y) => x >= 0 && y >= 0 && x < _colsCount && y < _rowsCount;

        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
                _generation[x, y] = state;
        }

        public void AddEntity(int entutyX, int entutyY) => UpdateCell(entutyX, entutyY, true);

        public void KillEntity(int entutyX, int entutyY) => UpdateCell(entutyX, entutyY, false);
    }
}