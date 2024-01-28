using Microsoft.AspNetCore.Mvc;
using ShortestPathAStar.Extensions;
using ShortestPathAStar.Models;
using ShortestPathAStar.Services;

namespace ShortestPathAStar.Controllers
{
    public class MapController : Controller
    {
        private readonly IShortestPathAlgorithm _shortestPathAlgorithm;

        public MapController(IShortestPathAlgorithm shortestPathAlgorithm)
        {
            _shortestPathAlgorithm = shortestPathAlgorithm;
        }

        public IActionResult Index()
        {
            List<Node> nodes = _shortestPathAlgorithm.ConstructMapGraph().Nodes;

            return View(nodes);
        }
        public IActionResult ShortesPathFineded()
        {
            List<Node> nodes = TempData.Get<List<Node>>("NodesData");

            return View(nodes);
        }
        [HttpPost]
        public ActionResult FindShortestPath(string start, string end)
        {
            // Retrieve pins from the database or any other source
            List<Node> nodes = _shortestPathAlgorithm.ConstructMapGraph().Nodes;

            Node? startPin = nodes.FirstOrDefault(x => x.Name == start);
            Node? endPin = nodes.FirstOrDefault(p => p.Name == end);
            if (startPin != null && endPin != null)
            {
                List<Node> shortestPath = _shortestPathAlgorithm.AStarAlgorithm(startPin, endPin);
                TempData.Put("NodesData", shortestPath);
                return Redirect("ShortesPathFineded");
            }

            return Json(null);
        }
    }
}
