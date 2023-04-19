using System.Data.SqlTypes;
using Sudoku.Shared;

namespace Sukoku.GraphColoration;

/// <summary>
/// Represents a graph data structure.
/// </summary>
public class Graph
{
    /// <summary>
    /// Represents the adjacency list of a graph, where each entry in the outer list represents
    /// the adjacency list of a vertex in the graph.
    /// </summary>
    private List<List<Vertex>> adjacency_;
   
    /// <summary>
    /// List of Vertex objects representing the nodes in the graph.
    /// </summary>
    private List<Vertex> nodes_;
    
    /// <summary>
    /// Initializes a new instance of the Graph class with a SudokuGrid object.
    /// </summary>
    /// <param name="grid">The SudokuGrid object to create the graph from.</param>
    public Graph(SudokuGrid grid)
    {
        nodes_ = new List<Vertex>();
        // Add all vertice in the graph
        for (int i = 0; i < 9 * 9; i++)
            nodes_.Add(new Vertex(i, 0));

        adjacency_ = new List<List<Vertex>>();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                HashSet<Vertex> subList = new HashSet<Vertex>();
                for (int element = 0; element < 9; element++)
                {
                    if (element != j)
                    {
                        nodes_[i * 9 + element].Color = grid.Cells[i][element];
                        subList.Add(nodes_[i * 9 + element]);
                    }

                    if (element != i)
                    {
                        nodes_[element * 9 + j].Color = grid.Cells[element][j];
                        subList.Add(nodes_[element * 9 + j]);
                    }
                }

                // Cell in same block
                int blockI = i / 3;
                int blockJ = j / 3;
                for (int a = 0; a < 3; a++)
                {
                    for (int b = 0; b < 3; b++)
                        if (!(blockI * 3 + a == i && blockJ * 3 + b == j))
                        {
                            nodes_[(blockI * 3 + a) * 9 + blockJ * 3 + b].Color =
                                grid.Cells[blockI * 3 + a][blockJ * 3 + b];
                            subList.Add(nodes_[(blockI * 3 + a) * 9 + blockJ * 3 + b]);
                        }
                }

                adjacency_.Add(subList.ToList());
            }
        }
    }
    
    /// <summary>
    /// Retrieves the neighbors of a given vertex in a graph.
    /// </summary>
    /// <param name="v">The vertex for which to retrieve neighbors.</param>
    /// <returns>A List of Vertex objects representing the neighbors of the given vertex.</returns>
    public List<Vertex> getNeighbor(Vertex v)
    {
        return adjacency_[v.Id];
    }
    

    /// <summary>
    /// Retrieves the uncolored vertices in a graph.
    /// </summary>
    /// <returns>A HashSet of Vertex objects representing the uncolored vertices.</returns>
    public HashSet<Vertex> GetUncolorVertices()
    {
        var unColor = new HashSet<Vertex>();
        foreach (var v in nodes_)
            if (v.Color == 0)
                unColor.Add(v);

        return unColor;
    }

    /// <summary>
    /// Calculates the saturation degree of a vertex in a graph, which is the count of its colored neighbors.
    /// </summary>
    /// <param name="v">The vertex for which to calculate the saturation degree.</param>
    /// <returns>An integer representing the saturation degree of the given vertex.</returns>
    public int GetSaturationDegree(Vertex v)
    {
        int sat = 0;
        foreach (var neighbor in getNeighbor(v))
            if (v.Color != 0)
                sat++;

        return sat;
    }


    /// <summary>
    /// Retrieves the most saturated vertex (uncolored vertex with the highest saturation degree) in a graph.
    /// </summary>
    /// <returns>The most saturated vertex in the graph, or null if all vertices are colored.</returns>
    public Vertex GetMostSaturatedVertex()
    {
        var vertices = GetUncolorVertices();
        if (vertices.Count == 0)
            return null;

        var mostVertex = vertices.First();
        int maxSat = -1;

        foreach (var v in vertices)
        {
            var sat = GetSaturationDegree(v);
            if (sat > maxSat)
            {
                maxSat = sat;
                mostVertex = v;
            }
        }

        return mostVertex;
    }

    /// <summary>
    /// Converts the colored vertices of a Sudoku graph into a Sudoku grid representation.
    /// </summary>
    /// <returns>A SudokuGrid object representing the Sudoku grid.</returns>
    public SudokuGrid toGrid()
    {
        var result = new SudokuGrid();
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
                result.Cells[i][j] = nodes_[i * 9 + j].Color;
        }

        return result;
    }

    /// <summary>
    /// Retrieves the list of possible colors for a given vertex based on its neighbors' colors.
    /// </summary>
    /// <param name="v">The vertex for which to retrieve possible colors.</param>
    /// <returns>A List of integers representing the possible colors for the given vertex.</returns>
    public List<int> getPossibleColors(Vertex v)
    {
        int color = 1;
        var neighbors = getNeighbor(v);

        var possibles = new List<int>();
        while (color <= 9)
        {
            if (neighbors.All(n => n.Color != color))
                possibles.Add(color);

            color++;
        }

        return possibles;
    }
}