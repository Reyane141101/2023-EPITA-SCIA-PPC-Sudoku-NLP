using Sudoku.Shared;

namespace Sukoku.GraphColoration
{
    /// <summary>
    /// Represents a Sudoku solver that uses graph coloring algorithm to solve Sudoku puzzles.
    /// Implements the ISudokuSolver interface.
    /// </summary>
    public class ColorSolver: ISudokuSolver
    {
        private int backtracks_ = 0;
        
        /// <summary>
        /// Solves a Sudoku puzzle using a graph coloring algorithm.
        /// </summary>
        /// <param name="s">The SudokuGrid object representing the puzzle to solve.</param>
        /// <returns>The solved SudokuGrid object.</returns>
        public SudokuGrid Solve(SudokuGrid s)
        {
            Graph g = new Graph(s);
            DStatur(g);

            return g.toGrid();
        }

        /// <summary>
        /// Performs a recursive Depth-First Search (DS) based algorithm to color the vertices of a graph using the saturation degree heuristic.
        /// </summary>
        /// <param name="graph">The graph to be colored.</param>
        /// <returns>True if the graph can be colored successfully, False otherwise.</returns>
        private bool DStatur(Graph graph)
        {
            var uncolor = graph.GetUncolorVertices();
            if (uncolor.Count == 0)
                return true;

            Vertex v = graph.GetMostSaturatedVertex();
            if (v == null)
                return true;

            foreach (var color in graph.getPossibleColors(v))
            {
                v.Color = color;

                if (DStatur(graph))
                    return true;

                backtracks_++;
                v.Color = 0;
            }

            return false;
        }
    }
}
