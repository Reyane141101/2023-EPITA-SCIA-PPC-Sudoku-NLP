using System;
using Google.OrTools.LinearSolver;
using Google.OrTools.Sat;
using Sudoku.Shared;

namespace Sudoku.OrTools;

public class Program
{
    public static void verify(SudokuGrid grid)
    {
        // Values
        for (int i = 0; i < 9; ++i)
        {
            for (int j = 0; j < 9; ++j)
            {
                if (grid.Cells[i][j] < 1 || grid.Cells[i][j] > 9)
                {
                    throw new Exception($"Invalid value at {i} {j}");
                }
            }
        }

        // Lines
        for (int i = 0; i < 9; ++i)
        {
            var used = new bool[9];
            for (int j = 0; j < 9; ++j) used[j] = false;
            
            for (int j = 0; j < 9; ++j)
            {
                used[grid.Cells[i][j] - 1] = true;
            }

            for (int j = 0; j < 9; ++j)
            {
                if (!used[j])
                {
                    throw new Exception($"Unused {j + 1}");
                }
            }
        }

        // Columns
        for (int i = 0; i < 9; ++i)
        {
            var used = new bool[9];
            for (int j = 0; j < 9; ++j) used[j] = false;
            
            for (int j = 0; j < 9; ++j)
            {
                used[grid.Cells[j][i] - 1] = true;
            }

            for (int j = 0; j < 9; ++j)
            {
                if (!used[j])
                {
                    throw new Exception($"Unused {j + 1}");
                }
            }
        }
    }
}