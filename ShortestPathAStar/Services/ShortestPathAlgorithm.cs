using ShortestPathAStar.Models;

namespace ShortestPathAStar.Services
{
    public class ShortestPathAlgorithm : IShortestPathAlgorithm
    {
        // Construct the map graph (replace with actual implementation)
        public Graph ConstructMapGraph()
        {
            // Initialize a new graph
            Graph kosovoMap = new Graph();

            // Define nodes representing locations in Kosovo
            var prishtina = new Node("Prishtina", 42.6629, 21.1655);
            var prizren = new Node("Prizren", 42.2139, 20.7397);
            var gjilan = new Node("Gjilan", 42.4604, 21.4697);
            var peja = new Node("Peja", 42.6609, 20.2894);
            var mitrovica = new Node("Mitrovica", 42.8833, 20.8667);
            var ferizaj = new Node("Ferizaj", 42.3709, 21.1557);

            // Define edges representing connections between locations
            prishtina.AdjacentEdges.Add(new Edge(prizren, 80)); // Example: Distance between Prishtina and Prizren is 80 km
            prishtina.AdjacentEdges.Add(new Edge(gjilan, 50));  // Example: Distance between Prishtina and Gjilan is 50 km
            prishtina.AdjacentEdges.Add(new Edge(peja, 90));    // Example: Distance between Prishtina and Peja is 90 km
            prishtina.AdjacentEdges.Add(new Edge(ferizaj, 30)); // Example: Distance between Prishtina and Ferizaj is 30 km

            prizren.AdjacentEdges.Add(new Edge(peja, 100));      // Example: Distance between Prizren and Peja is 100 km
            prizren.AdjacentEdges.Add(new Edge(gjilan, 120));     // Example: Distance between Prizren and Gjilan is 120 km

            gjilan.AdjacentEdges.Add(new Edge(peja, 60));        // Example: Distance between Gjilan and Peja is 60 km
            gjilan.AdjacentEdges.Add(new Edge(ferizaj, 40));     // Example: Distance between Gjilan and Ferizaj is 40 km

            peja.AdjacentEdges.Add(new Edge(ferizaj, 70));       // Example: Distance between Peja and Ferizaj is 70 km
            peja.AdjacentEdges.Add(new Edge(mitrovica, 80));     // Example: Distance between Peja and Mitrovica is 80 km

            mitrovica.AdjacentEdges.Add(new Edge(ferizaj, 60));  // Example: Distance between Mitrovica and Ferizaj is 60 km

            // Add nodes to the graph
            kosovoMap.Nodes.Add(prishtina);
            kosovoMap.Nodes.Add(prizren);
            kosovoMap.Nodes.Add(gjilan);
            kosovoMap.Nodes.Add(peja);
            kosovoMap.Nodes.Add(mitrovica);
            kosovoMap.Nodes.Add(ferizaj);

            return kosovoMap;
        }

        // A* Algorithm implementation
        public List<Node> AStarAlgorithm(Node start, Node goal)
        {
            var graph = this.ConstructMapGraph();
            // Initialize open and closed lists
            var closedSet = new HashSet<Node>();
            var openSet = new HashSet<Node> { start };

            // Dictionary to keep track of parent nodes for reconstructing the path
            var cameFrom = new Dictionary<Node, Node>();

            // Cost from start along best known path
            var gScore = new Dictionary<Node, double>();
            foreach (Node node in graph.Nodes)
            {
                gScore[node] = double.PositiveInfinity;
            }
            gScore[start] = 0;

            // Estimated total cost from start to goal through y
            var fScore = new Dictionary<Node, double>();
            foreach (Node node in graph.Nodes)
            {
                fScore[node] = double.PositiveInfinity;
            }
            fScore[start] = HeuristicCostEstimate(start, goal);

            while (openSet.Count > 0)
            {
                var current = GetNodeWithLowestFScore(openSet, fScore);
                if (current == goal)
                {
                    // Reconstruct path
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Edge neighborEdge in current.AdjacentEdges)
                {
                    var neighbor = neighborEdge.TargetNode;
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    double tentativeGScore = gScore[current] + neighborEdge.Weight;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                    else if (tentativeGScore >= gScore[neighbor])
                    {
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);
                }
            }

            // No path found
            return new List<Node>();
        }

        // Heuristic cost estimate function (replace with actual heuristic)
        private static double HeuristicCostEstimate(Node from, Node to)
        {
            // Implement your heuristic function (e.g., Euclidean distance, Manhattan distance, etc.)
            // Here's a simple example using Euclidean distance
            double dx = Math.Abs(from.X - to.X);
            double dy = Math.Abs(from.Y - to.Y);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        // Get the node with the lowest f-score from the open set
        private static Node GetNodeWithLowestFScore(HashSet<Node> openSet, Dictionary<Node, double> fScore)
        {
            Node lowestNode = null;
            double lowestScore = double.PositiveInfinity;
            foreach (Node node in openSet)
            {
                if (fScore[node] < lowestScore)
                {
                    lowestNode = node;
                    lowestScore = fScore[node];
                }
            }
            return lowestNode;
        }

        // Reconstruct the path from start to goal
        private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
        {
            var path = new List<Node> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }

    }
}
