using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = System.Console;

namespace Console2048
{
    class Program
    {
        static int[,] Grid = new int[4, 4];
        static int Score = 0;

        static bool DoAdd = true;

        static void Main(string[] args)
        {
            C.BackgroundColor = ConsoleColor.White;
            C.ForegroundColor = ConsoleColor.Black;

            ConsoleKey Input = ConsoleKey.A;
            AddNumber();

            while (true)
            {
                try
                {
                    if (DoAdd)
                    {
                        AddNumber();
                        C.Clear();
                        C.WriteLine("Score: " + Score);
                        WriteGrid();
                        C.WriteLine();
                    }
                    else DoAdd = true;
                    C.Write(">: ");

                    Input = C.ReadKey().Key;

                    switch (Input)
                    {
                        case ConsoleKey.LeftArrow:
                            for (int row = 0; row < 4; row++) //rows
                            {
                                ShiftGrid(Input, row);
                                for (int col = 0; col < 3; col++) //columns
                                {
                                    if (Grid[row, col] == Grid[row, col + 1])
                                    {
                                        var n = Grid[row, col + 1] * 2;
                                        Grid[row, col] = n;
                                        Grid[row, col + 1] = 0;

                                        Score += n;
                                        if (n == 2048) throw new Exception("You won the game!");
                                    }
                                }
                                ShiftGrid(Input, row);
                            }

                            break;

                        case ConsoleKey.RightArrow:
                            for (int row = 0; row < 4; row++) //rows
                            {
                                ShiftGrid(Input, row);
                                for (int col = 3; col > 0; col--) //columns
                                {
                                    if (Grid[row, col] == Grid[row, col - 1])
                                    {
                                        var n = Grid[row, col - 1] * 2;
                                        Grid[row, col] = n;
                                        Grid[row, col - 1] = 0;

                                        Score += n;
                                        if (n == 2048) throw new Exception("You won the game!");
                                    }
                                }
                                ShiftGrid(Input, row);
                            }

                            break;

                        case ConsoleKey.DownArrow:
                            for (int col = 0; col < 4; col++) //columns
                            {
                                ShiftGrid(Input, col);
                                for (int row = 3; row > 0; row--) //rows
                                {
                                    if (Grid[row, col] == Grid[row - 1, col])
                                    {
                                        var n = Grid[row - 1, col] * 2;
                                        Grid[row, col] = n;
                                        Grid[row - 1, col] = 0;

                                        Score += n;
                                        if (n == 2048) throw new Exception("You won the game!");
                                    }
                                }
                                ShiftGrid(Input, col);
                            }

                            break;

                        case ConsoleKey.UpArrow:
                            for (int col = 0; col < 4; col++) //columns
                            {
                                ShiftGrid(Input, col);
                                for (int row = 0; row < 3; row++) //rows
                                {
                                    if (Grid[row, col] == Grid[row + 1, col])
                                    {
                                        var n = Grid[row + 1, col] * 2;
                                        Grid[row, col] = n;
                                        Grid[row + 1, col] = 0;

                                        Score += n;
                                        if (n == 2048) throw new Exception("You won the game!");
                                    }
                                }
                                ShiftGrid(Input, col);
                            }

                            break;

                        default:
                            C.WriteLine();
                            C.WriteLine("That is not an acceptable input.");
                            C.WriteLine("Use the arrow keys to move around.");
                            DoAdd = false;
                            break;

                    }
                }
                catch (Exception ex)
                {
                    C.Clear();
                    C.WriteLine(ex.Message);
                    C.WriteLine("Your score was " + Score);
                    break;
                }
            }

            C.WriteLine("Press any key to continue.");
            C.ReadKey();
        }

        private static void ShiftGrid(ConsoleKey Input, int i)
        {
            switch (Input)
            {
                case ConsoleKey.LeftArrow:
                    for (int col = 3; col > 0; col--)
                    {
                        if (Grid[i, col - 1] == 0)
                        {
                            Grid[i, col - 1] = Grid[i, col];
                            Grid[i, col] = 0;
                            if (Grid[i, col - 1] != 0) col++;
                        }
                    }
                    break;

                case ConsoleKey.RightArrow:
                    for (int col = 0; col < 3; col++)
                    {
                        if (Grid[i, col + 1] == 0)
                        {
                            Grid[i, col + 1] = Grid[i, col];
                            Grid[i, col] = 0;
                            if (Grid[i, col + 1] != 0) col--;
                        }
                    }
                    break;

                case ConsoleKey.DownArrow:
                    for (int row = 0; row < 3; row++) // shift rows over
                    {
                        if (Grid[row + 1, i] == 0)
                        {
                            Grid[row + 1, i] = Grid[row, i];
                            Grid[row, i] = 0;
                            if (Grid[row + 1, i] != 0) row--;
                        }
                    }
                    break;

                case ConsoleKey.UpArrow:
                    for (int row = 3; row > 0; row--) // shift rows over
                    {
                        if (Grid[row - 1, i] == 0)
                        {
                            Grid[row - 1, i] = Grid[row, i];
                            Grid[row, i] = 0;
                            if (Grid[row - 1, i] != 0) row++;
                        }
                    }
                    break;
            }
        }

        static void AddNumber()
        {
            bool isOpen = false;
            for (int i = 0; i < 4; i++)
                for (int ii = 0; ii < 4; ii++)
                    if (Grid[i, ii] == 0) isOpen = true;

            if (isOpen)
            {
                Random rnd = new Random();
                Tuple<int, int> place = new Tuple<int, int>(rnd.Next(0, 4), rnd.Next(0, 4));

                while (Grid[place.Item1, place.Item2] != 0)
                    place = new Tuple<int, int>(rnd.Next(0, 4), rnd.Next(0, 4));

                Grid[place.Item1, place.Item2] = rnd.Next(1, 3) * 2;
            }
            else throw new Exception("The game is over.");
        }

        static void WriteGrid()
        {
            C.WriteLine();

            for (int i = 0; i < 4; i++ )
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    if (Grid[i, ii] != 0)
                    {
                        switch (Grid[i, ii].ToString().Length)
                        {
                            case 1:
                                C.Write(" |    ");
                                WriteColor(Grid[i, ii], ConsoleColor.DarkGray);
                                break;

                            case 2:
                                C.Write(" |   ");
                                WriteColor(Grid[i, ii], ConsoleColor.DarkRed);
                                break;

                            case 3:
                                C.Write(" |  ");
                                WriteColor(Grid[i, ii], ConsoleColor.Red);
                                break;

                            case 4:
                                C.Write(" | ");
                                WriteColor(Grid[i, ii], ConsoleColor.DarkMagenta);
                                break;
                        }
                    }
                    else C.Write(" |     ");
                }
                C.WriteLine();
                C.WriteLine("----------------------------");
            }
        }

        static void WriteColor(int toWrite, ConsoleColor color)
        {
            C.ForegroundColor = color;
            C.Write(toWrite);
            C.ForegroundColor = ConsoleColor.Black;
        }
    }
}
