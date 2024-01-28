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
            Node prishtina = new Node("Prishtina", 42.6629, 21.1655);
            Node prizren = new Node("Prizren", 42.2139, 20.7397);
            Node gjilan = new Node("Gjilan", 42.4604, 21.4697);
            Node peja = new Node("Peja", 42.6609, 20.2894);
            Node mitrovica = new Node("Mitrovica", 42.8833, 20.8667);
            Node ferizaj = new Node("Ferizaj", 42.3709, 21.1557);

            // Define edges representing connections between locations
            prishtina.AdjacentEdges.Add(new Edge(prizren, 80)); // Example: Distance between Prishtina and Prizren is 80 km
            prishtina.AdjacentEdges.Add(new Edge(gjilan, 50));  // Example: Distance between Prishtina and Gjilan is 50 km
            prishtina.AdjacentEdges.Add(new Edge(peja, 90));    // Example: Distance between Prishtina and Peja is 90 km
            prishtina.AdjacentEdges.Add(new Edge(ferizaj, 30)); // Example: Distance between Prishtina and Ferizaj is 30 km

            prizren.AdjacentEdges.Add(new Edge(peja, 100));      // Example: Distance between Prizren and Peja is 100 km
            prizren.AdjacentEdges.Add(new Edge(gjilan, 120));     // Example: Distance between Prizren and Gjilan is 120 km
            prizren.AdjacentEdges.Add(new Edge(ferizaj, 100));     // Example: Distance between Prizren and Gjilan is 120 km

            gjilan.AdjacentEdges.Add(new Edge(peja, 60));        // Example: Distance between Gjilan and Peja is 60 km
            gjilan.AdjacentEdges.Add(new Edge(ferizaj, 40));     // Example: Distance between Gjilan and Ferizaj is 40 km
            gjilan.AdjacentEdges.Add(new Edge(prishtina, 100));     // Example: Distance between Gjilan and Ferizaj is 40 km

            peja.AdjacentEdges.Add(new Edge(ferizaj, 70));       // Example: Distance between Peja and Ferizaj is 70 km
            peja.AdjacentEdges.Add(new Edge(mitrovica, 80));     // Example: Distance between Peja and Mitrovica is 80 km
            peja.AdjacentEdges.Add(new Edge(gjilan, 90));     // Example: Distance between Peja and Mitrovica is 80 km

            mitrovica.AdjacentEdges.Add(new Edge(ferizaj, 90));  // Example: Distance between Mitrovica and Ferizaj is 60 km
            mitrovica.AdjacentEdges.Add(new Edge(prishtina, 60));  // Example: Distance between Mitrovica and Ferizaj is 60 km
            mitrovica.AdjacentEdges.Add(new Edge(gjilan, 120));  // Example: Distance between Mitrovica and Ferizaj is 60 km

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
            Graph graph = this.ConstructMapGraph();
            // Initialize open and closed lists
            HashSet<Node> closedSet = new HashSet<Node>();
            HashSet<Node> openSet = new HashSet<Node> { start };

            // Dictionary to keep track of parent nodes for reconstructing the path
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

            // Cost from start along best known path
            Dictionary<Node, double> gScore = new Dictionary<Node, double>();
            foreach (Node node in graph.Nodes)
            {
                gScore[node] = double.PositiveInfinity;
            }
            gScore[start] = 0;

            // Estimated total cost from start to goal through y
            Dictionary<Node, double> fScore = new Dictionary<Node, double>();
            foreach (Node node in graph.Nodes)
            {
                fScore[node] = double.PositiveInfinity;
            }
            fScore[start] = EuclideanDistance(start, goal);

            while (openSet.Count > 0)
            {
                Node current = GetNodeWithLowestFScore(openSet, fScore);
                if (current == goal)
                {
                    // Reconstruct path
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Edge neighborEdge in current.AdjacentEdges)
                {
                    Node neighbor = neighborEdge.TargetNode;
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
                    fScore[neighbor] = gScore[neighbor] + EuclideanDistance(neighbor, goal);
                }
            }

            // No path found
            return new List<Node>();
        }

        // Heuristic cost estimate function (replace with actual heuristic)
        private static double EuclideanDistance(Node from, Node to)
        {
            double dx = Math.Pow(to.X - from.X, 2);
            double dy = Math.Pow(to.Y - from.Y, 2);
            return Math.Sqrt(dx + dy);
        }
        public static double ManhattanDistance(Node from, Node to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }
        public static double ChebyshevDistance(Node from, Node to)
        {
            return Math.Max(Math.Abs(from.X - to.X), Math.Abs(from.Y - to.Y));
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
            List<Node> path = new List<Node> { current };
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
