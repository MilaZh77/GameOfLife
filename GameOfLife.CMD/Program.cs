
using GameOfLife.Engine;
using System;
using System.Runtime.InteropServices;

namespace GameOfLife.CMD
{
    internal class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdshow);

        private const int MAXIMIZE = 3;
        private const int DENSITY = 3;
        private const char CELL_WITH_ENTITY = '#';
        private const char EMPTY_CELL = ' ';

        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(GetConsoleWindow(), MAXIMIZE);
            Console.CursorVisible = false;

            Console.WriteLine("Для запуска нажмите Enter . . .");
            Console.ReadLine();
            Console.SetCursorPosition(0, 0);

            Game game = new Game
            (
                rowsCount: Console.WindowHeight - 1,
                colsCount: Console.WindowWidth - 1,
                density: DENSITY
            );

            while (true)
            {
                Console.Title = game.GenerationNumber.ToString();

                bool[,] generation = game.Generation;

                for (int y = 0; y < generation.GetLength(1); y++)
                {
                    char[] visibleLine = new char[generation.GetLength(0)];

                    for (int x = 0; x < generation.GetLength(0); x++)
                    {
                        if (generation[x, y])
                            visibleLine[x] = CELL_WITH_ENTITY;
                        else
                            visibleLine[x] = EMPTY_CELL;
                    }

                    Console.WriteLine(visibleLine);
                }

                Console.SetCursorPosition(0, 0);
                game.CreateNextGeneration();
            }
        }
    }
}