using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FSharp.Control;
using QuickGraph;

namespace Graph_Coloration_Solver
{
    public class DsatColoringSolver
    {
        private Dictionary<int, int> _nAdjacentUncolored = new Dictionary<int, int>();
        private Dictionary<int, HashSet<int>> _AvailibleColor = new Dictionary<int, HashSet<int>>();
        private UndirectedGraph<int, UndirectedEdge<int>> _graph; 
        public Dictionary<int, int> VertexColors;
        public DsatColoringSolver(UndirectedGraph<int, UndirectedEdge<int>> graph, Dictionary<int, int> vertexColors)
        {
            this._graph = graph;
            this.VertexColors = vertexColors;
            for (int i = 0; i < 81; i++)
            {
                _nAdjacentUncolored.Add(i, 20);
                _AvailibleColor.Add(i, new HashSet<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9});
            }
            
            // Handling edge case ?!
            _AvailibleColor.Add(-1, new HashSet<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10});
            _nAdjacentUncolored.Add(-1, -1);

            foreach (int vertex in vertexColors.Keys)
                foreach (int adjVertex in _graph.AdjacentVertices(vertex))
                {
                    _nAdjacentUncolored[adjVertex]--;
                    _AvailibleColor[adjVertex].Remove(vertexColors[vertex]);
                }
        }
        
        public void Solve()
        {
            RecSolve();
        }

        private bool RecSolve()
        {
            // chose the next vertecise :
            int vertex = ChooseNextVertex();

            // stop case
            if (vertex == -1)
                return true;
            
            if (_AvailibleColor[vertex].Count() == 0)
                return false;

            foreach (int color in _AvailibleColor[vertex].ToList())
            {
                bool[] modified = UpdateDictionaries(vertex, color);
                if (RecSolve())
                    return true;
                
                // bactrack
                CancelUpdateDictionaries(vertex, color, modified);
            }

            return false;
        }
        
        private bool[] UpdateDictionaries(int vertex, int color)
        {
            bool[] modified = new bool[81];
            VertexColors.Add(vertex, color);
            foreach (int adjVertex in _graph.AdjacentVertices(vertex))
            {
                _nAdjacentUncolored[adjVertex]--;
                modified[adjVertex] = _AvailibleColor[adjVertex].Contains(color);
                _AvailibleColor[adjVertex].Remove(color);
            }

            return modified;
        }

        private void CancelUpdateDictionaries(int vertex, int color, bool[] modified)
        {
            VertexColors.Remove(vertex);
            foreach (int adjVertex in _graph.AdjacentVertices(vertex))
            {
                _nAdjacentUncolored[adjVertex]++;
                if (modified[adjVertex])
                    _AvailibleColor[adjVertex].Add(color);
            }
        }

        private int ChooseNextVertex()
        {
            int minAvailibleColorVertex = -1;

            for (int vertex = 0; vertex < 81; vertex++)
            {
                if (!VertexColors.ContainsKey(vertex) && _AvailibleColor[minAvailibleColorVertex].Count() >
                    _AvailibleColor[vertex].Count())
                    minAvailibleColorVertex = vertex;

            }
            
            int maxAdjacentUncoloredVertex = -1;

            for (int vertex = 0; vertex < 81; vertex++)
            {
                if (!VertexColors.ContainsKey(vertex) &&
                    _AvailibleColor[minAvailibleColorVertex].Count() == _AvailibleColor[vertex].Count() &&
                    _nAdjacentUncolored[maxAdjacentUncoloredVertex] < _nAdjacentUncolored[vertex])
                    maxAdjacentUncoloredVertex = vertex;
            }
            
            return maxAdjacentUncoloredVertex;
        }
    }
}
