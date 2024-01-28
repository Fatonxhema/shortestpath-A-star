using Microsoft.AspNetCore.Mvc;
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
            var nodes = _shortestPathAlgorithm.ConstructMapGraph().Nodes;

            return View(nodes);
        }
        public IActionResult ShortesPathFineded()
        {
            List<Node> nodes = (List<Node>)TempData[key: "NodesData"];

            return View(nodes);
        }
        [HttpPost]
        public ActionResult FindShortestPath(string start, string end)
        {
            // Retrieve pins from the database or any other source
            var nodes = _shortestPathAlgorithm.ConstructMapGraph().Nodes;

            var startPin = nodes.FirstOrDefault(x=>x.Name == start);
            var endPin = nodes.FirstOrDefault(p => p.Name== end);

            if (startPin != null && endPin != null)
            {
                var shortestPath = _shortestPathAlgorithm.AStarAlgorithm(startPin, endPin);
                TempData["NodesData"] = shortestPath;
                return Redirect("ShortesPathFineded");
            }

            return Json(null);
        }
    }
}
