using System;
using System.Collections.Generic;
using System.IO;
using C = System.Console;

namespace Console2048
{
    class Program
    {
        static int[,] Grid = new int[4, 4];
        static int Score = 0;
        static int MaxInt = 2048;
        static int GridSize = 4;

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

            StreamReader reader = new StreamReader("Settings.txt");
            int _GridSize = Convert.ToInt32(reader.ReadLine());
            if (_GridSize > 1)
            {
                Grid = new int[_GridSize, _GridSize];
                GridSize = _GridSize;
            }

            int _MaxInt = Convert.ToInt32(reader.ReadLine());
            if (_MaxInt < 20)
                MaxInt = Convert.ToInt32(Math.Pow(2, _MaxInt));
            reader.Close();

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
            for (int row = 0; row < GridSize; row++) //rows
            {
                ShiftGrid(row);
                for (int col = 0; col < GridSize - 1; col++) //columns
                {
                    if (Grid[row, col] == Grid[row, col + 1])
                    {
                        var n = Grid[row, col + 1] * 2;
                        Grid[row, col] = n;
                        Grid[row, col + 1] = 0;

                        Score += n;
                        if (n == MaxInt) throw new Exception("You won the game!");
                    }
                }
                ShiftGrid(row);
            }
        }

        private static void RotateGrid()
        {
            int[,] newGrid = new int[GridSize, GridSize];

            for (int i = GridSize - 1; i >= 0; --i)
            {
                for (int ii = 0; ii < GridSize; ++ii)
                {
                    newGrid[ii, GridSize - (1 + i)] = Grid[i, ii];
                }
            }

            Grid = newGrid;
        }

        private static void ShiftGrid(int i)
        {
            for (int n = GridSize - 1; n > 0; n--) //Shift rows three times
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
            for (int i = 0; i < GridSize; i++)
                for (int ii = 0; ii < GridSize; ii++)
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
            int TotalSpaces = MaxInt.ToString().Length;

            for (int i = 0; i < GridSize; i++)
            {
                for (int ii = 0; ii < GridSize; ii++)
                {
                    C.Write(" | ");
                    if (Grid[i, ii] != 0)
                    {
                        WriteChar(' ', TotalSpaces - Grid[i, ii].ToString().Length);
                        WriteColor(Grid[i, ii], ConsoleColor.DarkGray);
                    }
                    else WriteChar(' ', TotalSpaces);
                }
                C.WriteLine();
                WriteChar('-', (3 + TotalSpaces) * GridSize);
                C.WriteLine();
            }
        }

        static void WriteChar(char c, int n)
        {
            for (int i = 0; i < n; i++)
            {
                C.Write(c);
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
