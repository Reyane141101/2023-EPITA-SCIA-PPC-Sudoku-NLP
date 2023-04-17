namespace Sukoku.GraphColoration;

/// <summary>
/// Represents a vertex in a graph.
/// </summary>
public class Vertex
{
    /// <summary>
    /// The ID of the vertex.
    /// </summary>
    public int Id;

    /// <summary>
    /// The color of the vertex.
    /// </summary>
    public int Color;

    /// <summary>
    /// Initializes a new instance of the <see cref="Vertex"/> class.
    /// </summary>
    /// <param name="id">The ID of the vertex.</param>
    /// <param name="color">The color of the vertex.</param>
    public Vertex(int id, int color)
    {
        Id = id;
        Color = color;
    }
}