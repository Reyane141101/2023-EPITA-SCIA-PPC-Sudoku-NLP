using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Shared;
using QuickGraph;
using GraphSharp;
using GraphSharp.Algorithms.Layout.Contextual;


namespace Graph_Coloration_Solver
{
	public class GraphColorationSolver : ISudokuSolver
	{

		public SudokuGrid Solve(SudokuGrid s)
		{
			var adjacencyMatrix = BuildAdjencyMatrix();
			var solver = new VertexColoringSolver();
			var vertexColors = SudokuToVertexColor(s);
			
			//launch the solver
			var colors = solver.Solve(adjacencyMatrix, s.Cells.Length, vertexColors);

			if (colors != null)
				foreach (KeyValuePair<int, int> color in colors)
					s.Cells[color.Key / 9][color.Key % 9] = color.Value;
			else
				Console.WriteLine("No valid coloring exists.");
			
			return s;
		}

		private Dictionary<int, int> SudokuToVertexColor(SudokuGrid sudoku)
		{
			var vertexColors = new Dictionary<int, int>();
			for (int row = 0; row < sudoku.Cells.Length; row++)
			{
				for (int col = 0; col < sudoku.Cells.Length; col++)
					if (sudoku.Cells[row][col] != 0)
						vertexColors[row + col * sudoku.Cells.Length] = sudoku.Cells[row][col];
			}

			return vertexColors;
		}

		private int[][] BuildAdjencyMatrix()
		{
			int[][] adjacencyMatrix = new int[81][];

			for (int i = 0; i < 81; i++)
			{
				adjacencyMatrix[i] = new int[81];
				for (int j = 0; j <= i; j++)
				{
					if (j / 9 == i / 9 || j % 9 == i % 9 || (j / 27 == i / 27 && j % 9 / 3 == i % 9 / 3))
					{
						adjacencyMatrix[i][j] = 1;
						adjacencyMatrix[j][i] = 1;
					}
					else
						adjacencyMatrix[i][j] = 0;
				}
			}

			return adjacencyMatrix;
		}
		
	}
}

