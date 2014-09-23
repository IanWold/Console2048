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
        static int[,] UndoGrid = new int[4, 4];
        static int UndoScore = 0;
        static bool HasUndone = true;
        static int MaxInt = 2048;
        static int GridSize = 4;
        static int MaxUndo = 1;

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

        static void ReadSettings()
        {
            try
            {
                StreamReader reader = new StreamReader("Settings.txt");

                int _GridSize = Convert.ToInt32(reader.ReadLine());
                if (_GridSize > 1)
                {
                    Grid = new int[_GridSize, _GridSize];
                    UndoGrid = new int[_GridSize, _GridSize];
                    GridSize = _GridSize;
                }

                int _MaxInt = Convert.ToInt32(reader.ReadLine());
                if (_MaxInt < 20)
                    MaxInt = Convert.ToInt32(Math.Pow(2, _MaxInt));

                MaxUndo = Convert.ToInt32(reader.ReadLine());

                reader.Close();
            }
            catch { }
        }

        static void Main(string[] args)
        {
            C.BackgroundColor = ConsoleColor.White;
            C.ForegroundColor = ConsoleColor.Black;
            C.Clear();

            Intro();
            ReadSettings();

            ConsoleKey Input = ConsoleKey.A;
            bool DoAdd = true;
            bool DoWrite = true;

            AddNumber();

            while (true)
            {
                try
                {
                    if (DoAdd) AddNumber();
                    else DoAdd = true;

                    if (DoWrite)
                    {
                        C.Clear();
                        C.WriteLine("Score: " + Score + " | Undos: " + MaxUndo);
                        WriteGrid();
                    }
                    else DoWrite = true;

                    C.WriteLine();
                    C.Write(">: ");
                    Input = C.ReadKey().Key;

                    switch (Input)
                    {
                        case ConsoleKey.LeftArrow:
                            MoveGrid(0, 0);
                            break;

                        case ConsoleKey.RightArrow:
                            MoveGrid(2, 2);
                            break;

                        case ConsoleKey.DownArrow:
                            MoveGrid(1, 3);
                            break;

                        case ConsoleKey.UpArrow:
                            MoveGrid(3, 1);
                            break;

                        case ConsoleKey.U:
                            C.WriteLine();

                            if (MaxUndo != 0)
                            {
                                if (!HasUndone)
                                {
                                    Grid = UndoGrid;
                                    Score = UndoScore;
                                    HasUndone = true;
                                    MaxUndo--;
                                    DoAdd = false;
                                }
                                else C.WriteLine("You may only undo once at a time.");
                            }
                            else
                            {
                                C.WriteLine("You may not undo anymore.");
                                DoAdd = DoWrite = false;
                            }
                            break;

                        case ConsoleKey.H:
                            C.WriteLine();
                            C.WriteLine("Console2048 Copyright 2014 Ian Wold");
                            C.WriteLine("Licensed under the MIT Open-Source License");
                            C.WriteLine();
                            C.WriteLine("Use the arrow keys on the keyboard to move");
                            DoAdd = DoWrite = false;
                            break;

                        default:
                            C.WriteLine();
                            C.WriteLine("That is not an acceptable input.");
                            C.WriteLine("Use the arrow keys to move around.");
                            DoAdd = DoWrite = false;
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

        private static void MoveGrid(int r1, int r2)
        {
            HasUndone = false;
            UndoGrid = Grid;
            UndoScore = Score;

            RotateGrid(r1);
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
            RotateGrid(r2);
        }

        private static void RotateGrid(int n)
        {
            for (int repeat = 0; repeat < n; repeat++)
            {
                int[,] newGrid = new int[GridSize, GridSize];

                for (int i = GridSize - 1; i >= 0; --i)
                    for (int ii = 0; ii < GridSize; ++ii)
                        newGrid[ii, GridSize - (1 + i)] = Grid[i, ii];

                Grid = newGrid;
            }
        }

        private static void ShiftGrid(int i)
        {
            for (int n = 0; n < GridSize; n++) //Shift rows GridSize - 1 times
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
                Tuple<int, int> place = new Tuple<int, int>(rnd.Next(0, GridSize), rnd.Next(0, GridSize));

                while (Grid[place.Item1, place.Item2] != 0)
                    place = new Tuple<int, int>(rnd.Next(0, GridSize), rnd.Next(0, GridSize));

                int[] array = {2,2,2,4};
                Grid[place.Item1, place.Item2] = array[rnd.Next(0, array.Length)];
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
                        WriteColor(Grid[i, ii], Grid[i, ii].ToString().Length);
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
            for (int i = 0; i < n; i++) C.Write(c);
        }

        static void WriteColor(int toWrite, int length)
        {
            switch (length)
            {
                case 1:
                    C.ForegroundColor = ConsoleColor.DarkGray;
                    break;

                case 2:
                    C.ForegroundColor = ConsoleColor.DarkRed;
                    break;

                case 3:
                    C.ForegroundColor = ConsoleColor.Red;
                    break;

                case 4:
                    C.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;

                default:
                    C.ForegroundColor = ConsoleColor.Magenta;
                    break;
            }
            C.Write(toWrite);
            C.ForegroundColor = ConsoleColor.Black;
        }
    }
}
