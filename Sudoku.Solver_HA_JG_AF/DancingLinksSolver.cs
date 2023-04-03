using Sudoku.Shared;
using System.Runtime.InteropServices;
using DlxLib;
using System.Collections.Immutable;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sudoku.Solver_HA_JG_AF
{
    public class DancingLinksSolver : ISudokuSolver
    {

        /// <summary>
        /// Transform 9x9 sudoku to it's exact cover matrix represention and apply the solve method of the DLX library to find a solution
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public SudokuGrid Solve(SudokuGrid s)
        {
            var dlx = new Dlx();
            var exactCoverMatrix = ConvertToCovertExactMatrix(s.Cells);
            var solution = dlx.Solve(exactCoverMatrix).Take(1);
            
            var sudokuSolution = new SudokuGrid();
            sudokuSolution.Cells = ConvertToReadableSudoku(solution.ElementAt(0).RowIndexes);
            return sudokuSolution;
        }

        /// <summary>
        /// Convert a 9x9 sudoku to an exact cover matrix (grid of 1s and 0s where columns represent constraints and rows represent possibilities)
        /// </summary>
        /// <param name="sudoku"></param>
        /// <returns></returns>
        public int[,] ConvertToCovertExactMatrix(int[][] sudoku)
        {
            const int ROWS = 729; // 9 x 9 x 9 -> all the possibilities 
            const int COLS = 324; // 9 x (9 + 9 + 9 + 9) -> all the contraints

            int[,] matrix = new int[ROWS, COLS];

            for (int row = 0; row < 9; row++ )
            {
                for (int col=0; col < 9; col++)
                {
                    int currentNum = sudoku[row][col];

                    if (currentNum == 0) // Case not filled
                    {
                        // We add every possibilities 
                        for (int num = 1; num <= 9; num++)
                        {
                            int idx = (row * 9 + col) * 9 + (num - 1);

                            int rowConst = row * 9 + num - 1;
                            int colConst = col * 9 + num - 1;
                            int boxConst = (row / 3 * 3 + col / 3) * 9 + num - 1;

                            matrix[idx, row * 9 + col] = 1;
                            matrix[idx, 81 + rowConst] = 1;
                            matrix[idx, 162 + colConst] = 1;
                            matrix[idx, 243 + boxConst] = 1;
                        }
                    }
                    else
                    {
                        // We represent the state as a single row
                        int idx = (row * 9 + col) * 9 + (currentNum - 1);

                        int rowConst = row * 9 + currentNum - 1;
                        int colConst = col * 9 + currentNum - 1;
                        int boxConst = (row / 3 * 3 + col / 3) * 9 + currentNum - 1;

                        matrix[idx, row * 9 + col] = 1;
                        matrix[idx, 81 + rowConst] = 1;
                        matrix[idx, 162 + colConst] = 1;
                        matrix[idx, 243 + boxConst] = 1;
                    }
                }
            }
            return matrix;
        }

        /// <summary>
        /// Convert a solution sudoku from exact cover matrix (only 0 and 1) to a 'readable' 9x9 sudoku with numbers from 1 to 9
        /// </summary>
        /// <param name="solutionRowIndexes"></param>
        /// <returns></returns>
        public int[][] ConvertToReadableSudoku(IEnumerable<int> solutionRowIndexes)
        {
            int[,] sudoku = new int[9,9];

            // Convert exact cover matrix to a 9x9 sudoku
            foreach (var  solutionRowIndex in solutionRowIndexes)
            {
                int num = (solutionRowIndex % 9) + 1;
                int row = (solutionRowIndex / 9) % 9;
                int col = solutionRowIndex / 81;
                sudoku[col, row] = num;
            }

            // Convert int[,] to int[][]
            return Enumerable.Range(0, sudoku.GetLength(0))
                .Select(i => Enumerable.Range(0, sudoku.GetLength(1)).Select(j => sudoku[i, j]).ToArray())
                .ToArray();
        }
    }
}