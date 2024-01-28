namespace ShortestPathAStar.Models
{
    public class Edge
    {
        public Node TargetNode { get; set; }
        public int Weight { get; set; }
        // Add any other properties as needed

        public Edge(Node targetNode, int weight)
        {
            TargetNode = targetNode;
            Weight = weight;
        }
    }
}