using System;
using System.Collections.Generic;
using C = System.Console;

namespace Console2048
{
    class Program
    {
        static int[,] Grid = new int[4, 4];
        static int Score = 0;
        static List<ConsoleKey> Konami = new List<ConsoleKey>();

        static void Intro()
        {
            C.WriteLine("#################################");
            C.WriteLine("           Console2048           ");
            C.WriteLine("#################################");
            C.WriteLine();
            C.WriteLine("Use the arrow keys to move around");
            C.WriteLine("        Type 'H' for help        ");
            C.WriteLine();
            C.WriteLine(" Combine numbers to get to 2048! ");
            C.WriteLine();
            C.WriteLine("#################################");
            C.WriteLine();
            C.Write("Press any key to start...");
            C.ReadKey();
            C.Clear();
        }

        static void Main(string[] args)
        {
            C.BackgroundColor = ConsoleColor.White;
            C.ForegroundColor = ConsoleColor.Black;
            C.Clear();

            Intro();

            ConsoleKey Input = ConsoleKey.A;
            bool DoAdd = true;

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
                    Konami.Add(Input);
                    CheckKonami();

                    switch (Input)
                    {
                        case ConsoleKey.LeftArrow:
                            MoveGrid();
                            break;

                        case ConsoleKey.RightArrow:
                            RotateGrid();
                            RotateGrid();
                            MoveGrid();
                            RotateGrid();
                            RotateGrid();
                            break;

                        case ConsoleKey.DownArrow:
                            RotateGrid();
                            MoveGrid();
                            RotateGrid();
                            RotateGrid();
                            RotateGrid();
                            break;

                        case ConsoleKey.UpArrow:
                            RotateGrid();
                            RotateGrid();
                            RotateGrid();
                            MoveGrid();
                            RotateGrid();
                            break;

                        case ConsoleKey.H:
                            C.WriteLine();
                            C.WriteLine("Console2048 Copyright 2014 Ian Wold");
                            C.WriteLine("Licensed under the MIT Open-Source License");
                            C.WriteLine();
                            C.WriteLine("Use the arrow keys on the keyboard to move");
                            DoAdd = false;
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

        private static void MoveGrid()
        {
            for (int row = 0; row < 4; row++) //rows
            {
                ShiftGrid(row);
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
                ShiftGrid(row);
            }
        }

        private static void RotateGrid()
        {
            int[,] newGrid = new int[4, 4];

            for (int i = 3; i >= 0; --i)
            {
                for (int ii = 0; ii < 4; ++ii)
                {
                    newGrid[ii, 3 - i] = Grid[i, ii];
                }
            }

            Grid = newGrid;
        }

        private static void ShiftGrid(int i)
        {
            for (int n = 3; n > 0; n--) //Shift rows three times
            {
                for (int col = n; col > 0; col--)
                {
                    if (Grid[i, col - 1] == 0)
                    {
                        Grid[i, col - 1] = Grid[i, col];
                        Grid[i, col] = 0;
                        if (Grid[i, col - 1] != 0) col++;
                    }
                }
            }
        }

        static void AddNumber()
        {
            bool isOpen = false;
            for (int i = 0; i < 4; i++)
                for (int ii = 0; ii < 4; ii++)
                    if (Grid[i, ii] == 0)
                        isOpen = true;

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

        private static void CheckKonami()
        {
            var n = Konami.Count;
            if (n > 9)
                if (Konami[n - 1] == ConsoleKey.A)
                    if (Konami[n - 2] == ConsoleKey.B)
                        if (Konami[n - 3] == ConsoleKey.RightArrow)
                            if (Konami[n - 4] == ConsoleKey.LeftArrow)
                                if (Konami[n - 5] == ConsoleKey.RightArrow)
                                    if (Konami[n - 6] == ConsoleKey.LeftArrow)
                                        if (Konami[n - 7] == ConsoleKey.DownArrow)
                                            if (Konami[n - 8] == ConsoleKey.DownArrow)
                                                if (Konami[n - 9] == ConsoleKey.UpArrow)
                                                    if (Konami[n - 10] == ConsoleKey.UpArrow)
                                                        throw new Exception("Open source easter eggs aren't as much fun.");
        }
    }
}
