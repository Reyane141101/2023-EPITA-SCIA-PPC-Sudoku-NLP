using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph_Coloration_Solver
{
    public class DsatColoringSolver
    {
        private Dictionary<int, int> _nAdjacentUncolored = new Dictionary<int, int>();
        private Dictionary<int, HashSet<int>> _AdjacentColor = new Dictionary<int, HashSet<int>>();
        
        public DsatColoringSolver
        public Dictionary<int, int> Solve(int[][] adjacencyMatrix, int numColors, Dictionary<int, int> vertexColors)
        {
            var availableColors = Enumerable.Range(1, numColors).ToList();

            for (int vertex = 0; vertex < adjacencyMatrix.Length; vertex++)
            {
                // Find the colors of the adjacent vertices
                var adjacentColors = new HashSet<int>();
                for (int neighbor = 0; neighbor < adjacencyMatrix[vertex].Length; neighbor++)
                {
                    if (adjacencyMatrix[vertex][neighbor] == 1 && vertexColors.ContainsKey(neighbor))
                    {
                        adjacentColors.Add(vertexColors[neighbor]);
                    }
                }

                // Find the first available color for the vertex
                var color = availableColors.FirstOrDefault(c => !adjacentColors.Contains(c));
                if (color != 0)
                {
                    vertexColors[vertex] = color;
                }
                else
                {
                    // Backtrack if no color is available
                    vertexColors.Remove(vertex);
                    vertex--;
                    if (vertex < 0)
                    {
                        // No valid coloring exists
                        return null;
                    }
                }
            }

            return vertexColors;
        }
    }
}
