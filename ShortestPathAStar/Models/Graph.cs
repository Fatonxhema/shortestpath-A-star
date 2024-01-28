namespace ShortestPathAStar.Models
{
    public class Graph
    {
        public List<Node> Nodes { get; set; }
        // Add any other properties as needed

        public Graph()
        {
            Nodes = new List<Node>();
        }

        public Node GetNode(string name)
        {
            return Nodes.Find(node => node.Name == name);
        }
    }
}
