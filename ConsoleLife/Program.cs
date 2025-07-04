using GameOfLife;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleLife
{
    internal class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWind, int nCmdShow);
        private const int MAXIMAZE = 3;

        static void Main(string[] args)
        {
            
            Console.ReadLine();
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(GetConsoleWindow(), MAXIMAZE);
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            var gameEngine = new GameEngine
            (
                 rows: Console.WindowHeight - 2,
                 cols: Console.WindowWidth - 2,
                 density: 2
            );

            while (true)
            {
                Console.Title = gameEngine.CurrentGeneration.ToString();

                var field = gameEngine.GetCurrentGeneration();

                for (int y = 0; y < field.GetLength(1); y++)
                {
                    var str = new char[field.GetLength(0)];

                    for (int x = 0; x < field.GetLength(0); x++)
                    {
                        if (field[x, y])
                            str[x] = '#';
                        else
                            str[x] = ' ';
                    }
                    Console.WriteLine(str);
                }
                Console.SetCursorPosition(0, 0);
                gameEngine.NextGeneration();

            }
        }
    }
}
