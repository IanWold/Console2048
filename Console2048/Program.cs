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
                    AddNumber();
                    C.Clear();
                    C.WriteLine("Score: " + Score);
                    WriteGrid();
                    C.WriteLine();
                    C.Write(">: ");

                    Input = C.ReadKey().Key;

                    switch (Input)
                    {
                        case ConsoleKey.LeftArrow:
                            for (int i = 0; i < 4; i++) //rows
                            {
                                for (int ii = 0; ii < 3; ii++) //collumns
                                {
                                    if (Grid[i, ii] == 0)
                                    {
                                        Grid[i, ii] = Grid[i, ii + 1];
                                        Grid[i, ii + 1] = 0;
                                    }
                                    else if (Grid[i, ii] == Grid[i, ii + 1])
                                    {
                                        var n = Grid[i, ii + 1] * 2;
                                        Grid[i, ii] = n;
                                        Grid[i, ii + 1] = 0;

                                        AddScore(n);
                                        if (n == 2048) throw new Exception("You won the game!");
                                    }
                                }
                            }

                            break;

                        case ConsoleKey.RightArrow:
                            for (int i = 0; i < 4; i++) //rows
                            {
                                for (int ii = 3; ii > 0; ii--) //collumns
                                {
                                    if (Grid[i, ii] == 0)
                                    {
                                        Grid[i, ii] = Grid[i, ii - 1];
                                        Grid[i, ii - 1] = 0;
                                    }
                                    else if (Grid[i, ii] == Grid[i, ii - 1])
                                    {
                                        var n = Grid[i, ii - 1] * 2;
                                        Grid[i, ii] = n;
                                        Grid[i, ii - 1] = 0;

                                        AddScore(n);
                                        if (n == 2048) throw new Exception("You won the game!");
                                    }
                                }
                            }

                            break;

                        case ConsoleKey.DownArrow:
                            for (int i = 0; i < 4; i++) //columns
                            {
                                for (int ii = 3; ii > 0; ii--) //rows
                                {
                                    if (Grid[ii, i] == 0)
                                    {
                                        Grid[ii, i] = Grid[ii - 1, i];
                                        Grid[ii - 1, i] = 0;
                                    }
                                    else if (Grid[ii, i] == Grid[ii - 1, i])
                                    {
                                        var n = Grid[ii - 1, i] * 2;
                                        Grid[ii, i] = n;
                                        Grid[ii - 1, i] = 0;

                                        AddScore(n);
                                        if (n == 2048) throw new Exception("You won the game!");
                                    }
                                }
                            }

                            break;

                        case ConsoleKey.UpArrow:
                            for (int i = 0; i < 4; i++) //columns
                            {
                                for (int ii = 0; ii < 3; ii++) //rows
                                {
                                    if (Grid[ii, i] == 0)
                                    {
                                        Grid[ii, i] = Grid[ii + 1, i];
                                        Grid[ii + 1, i] = 0;
                                    }
                                    else if (Grid[ii, i] == Grid[ii + 1, i])
                                    {
                                        var n = Grid[ii + 1, i] * 2;
                                        Grid[ii, i] = n;
                                        Grid[ii + 1, i] = 0;

                                        AddScore(n);
                                        if (n == 2048) throw new Exception("You won the game!");
                                    }
                                }
                            }

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
                {
                    place = new Tuple<int, int>(rnd.Next(0, 4), rnd.Next(0, 4));
                }

                Grid[place.Item1, place.Item2] = rnd.Next(1, 3) * 2;
            }
            else throw new Exception("The game is over.");
        }

        static void AddScore(int toAdd)
        {
            switch (toAdd)
            {
                case 4:
                    Score += 4;
                    break;

                case 8:
                    Score += 16;
                    break;

                case 16:
                    Score += 48;
                    break;

                case 32:
                    Score += 128;
                    break;

                case 64:
                    Score += 320;
                    break;

                case 128:
                    Score += 768;
                    break;

                case 256:
                    Score += 1792;
                    break;

                case 512:
                    Score += 4096;
                    break;

                case 1024:
                    Score += 9216;
                    break;

                case 2048:
                    Score += 20480;
                    break;
            }
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
                                break;

                            case 2:
                                C.Write(" |   ");
                                break;

                            case 3:
                                C.Write(" |  ");
                                break;

                            case 4:
                                C.Write(" | ");
                                break;
                        }

                        WriteColor(Grid[i, ii]);
                    }
                    else C.Write(" |     ");
                }
                C.WriteLine();
                C.WriteLine("----------------------------");
            }
        }

        static void WriteColor(int toWrite)
        {
            switch (toWrite)
            {
                case 2:
                    C.ForegroundColor = ConsoleColor.DarkGray;
                    C.Write(toWrite);
                    break;

                case 4:
                    C.ForegroundColor = ConsoleColor.DarkGray;
                    C.Write(toWrite);
                    break;

                case 8:
                    C.ForegroundColor = ConsoleColor.DarkYellow;
                    C.Write(toWrite);
                    break;

                case 16:
                    C.ForegroundColor = ConsoleColor.DarkYellow;
                    C.Write(toWrite);
                    break;

                case 32:
                    C.ForegroundColor = ConsoleColor.DarkRed;
                    C.Write(toWrite);
                    break;

                case 64:
                    C.ForegroundColor = ConsoleColor.DarkRed;
                    C.Write(toWrite);
                    break;

                case 128:
                    C.ForegroundColor = ConsoleColor.Red;
                    C.Write(toWrite);
                    break;

                case 256:
                    C.ForegroundColor = ConsoleColor.Red;
                    C.Write(toWrite);
                    break;

                case 512:
                    C.ForegroundColor = ConsoleColor.DarkMagenta;
                    C.Write(toWrite);
                    break;

                case 1024:
                    C.ForegroundColor = ConsoleColor.DarkMagenta;
                    C.Write(toWrite);
                    break;

                case 2048:
                    C.ForegroundColor = ConsoleColor.Magenta;
                    C.Write(toWrite);
                    break;
            }

            C.ForegroundColor = ConsoleColor.Black;
        }
    }
}
