
namespace ShortestPathAStar.Models
{
    public class Node
    {
        public string Name { get; set; }
        public List<Edge> AdjacentEdges { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public Node(string name, double x, double y)
        {
            Name = name;
            AdjacentEdges = new List<Edge>();
            X = x;
            Y = y;
        }

        public Node()
        {
            AdjacentEdges = new List<Edge>();
        }
    }
}
