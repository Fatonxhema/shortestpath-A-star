using ShortestPathAStar.Models;

namespace ShortestPathAStar.Services
{
    public interface IShortestPathAlgorithm
    {
        List<Node> AStarAlgorithm(Node start, Node goal);
        Graph ConstructMapGraph();
        //List<Pin> FindShortestPath(Map map, Pin startPin, Pin endPin);
    }
}