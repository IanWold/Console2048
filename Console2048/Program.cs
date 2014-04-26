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

        static void Main(string[] args)
        {
            ConsoleKey Input = ConsoleKey.A;
            AddNumber();

            while (true)
            {
                try
                {
                    AddNumber();
                    C.Clear();
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
                                        Grid[i, ii] = Grid[i, ii + 1] * 2;
                                        Grid[i, ii + 1] = 0;

                                        if (Grid[i, ii + 1] * 2 == 2048) throw new Exception("You won the game!");
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
                                        Grid[i, ii] = Grid[i, ii - 1] * 2;
                                        Grid[i, ii - 1] = 0;

                                        if (Grid[i, ii - 1] * 2 == 2048) throw new Exception("You won the game!");
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
                                        Grid[ii, i] = Grid[ii - 1, i] * 2;
                                        Grid[ii - 1, i] = 0;

                                        if (Grid[ii - 1, i] * 2 == 2048) throw new Exception("You won the game!");
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
                                        Grid[ii, i] = Grid[ii + 1, i] * 2;
                                        Grid[ii + 1, i] = 0;

                                        if (Grid[ii + 1, i] * 2 == 2048) throw new Exception("You won the game!");
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
                                C.Write(" |    {0}", Grid[i, ii].ToString());
                                break;

                            case 2:
                                C.Write(" |   {0}", Grid[i, ii].ToString());
                                break;

                            case 3:
                                C.Write(" |  {0}", Grid[i, ii].ToString());
                                break;

                            case 4:
                                C.Write(" | {0}", Grid[i, ii].ToString());
                                break;
                        }
                    }
                    else C.Write(" |     ");
                }
                C.WriteLine();
                C.WriteLine("----------------------------");
            }
        }
    }
}
