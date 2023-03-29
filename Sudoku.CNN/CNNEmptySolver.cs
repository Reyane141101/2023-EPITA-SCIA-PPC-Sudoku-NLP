using Sudoku.Shared;

namespace Sudoku.CNN;

public class CnnEmptySolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        Console.WriteLine(@"===== DlixLib Empty Solver =====");
        return s.CloneSudoku();
    }
}