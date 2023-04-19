using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Shared;
using QuickGraph;
using QuickGraph.Algorithms.GraphColoring.VertexColoring;

namespace Graph_Coloration_Solver
{
	public class QuickGraphDSatColorationSolver : ISudokuSolver
	{

		public SudokuGrid Solve(SudokuGrid s)
		{
			
			UndirectedGraph<int, UndirectedEdge<int>> graph = BuildGraph();
			var vertexColors = SudokuToVertexColor(s);
			var solver = new DsatColoringSolver(graph, vertexColors);

			solver.Solve();
			
			foreach (KeyValuePair<int, int> color in solver.VertexColors)
			{
				s.Cells[color.Key % 9][color.Key / 9] = color.Value;
			}
			
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

		private UndirectedGraph<int, UndirectedEdge<int>> BuildGraph()
		{
			UndirectedGraph<int, UndirectedEdge<int>> graph = new UndirectedGraph<int, UndirectedEdge<int>>();
			
			graph.AddVertexRange(Enumerable.Range(0, 81).ToArray());

			int[][] adjacencyMatrix = new int[81][];

			for (int i = 0; i < 81; i++)
			{
				for (int j = 0; j <= i; j++)
				{
					if (j / 9 == i / 9 || j % 9 == i % 9 || (j / 27 == i / 27 && j % 9 / 3 == i % 9 / 3))
					{
						graph.AddEdge(new UndirectedEdge<int>(i, j));
					}
				}
			}
			return graph;
		}
		
	}
}

