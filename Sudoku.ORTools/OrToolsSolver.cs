using Google.OrTools.Sat;
using Sudoku.Shared;

namespace Sudoku.OrTools
{
	public class OrToolsSolver : ISudokuSolver
	{
		public SudokuGrid Solve(SudokuGrid s)
		{
			return s;
		}

		/**
		 * Initialize a model with n*n from a sudoku grid
		 */
		private CpModel InitModel(SudokuGrid s)
		{
			var model = new CpModel();
			
			const int size = 9;
			var cellVars = new IntVar[size, size];
			for (var i = 0; i < size; i++)
			{
				for (var j = 0; j < size; j++)
				{
					// TODO: Initialize the known values
					cellVars[i, j] = model.NewIntVar(1, size, $"x{i}{j}");
				}
			}
			
			return model;
		}
	}
}
