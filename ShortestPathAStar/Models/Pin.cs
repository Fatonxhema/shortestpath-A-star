namespace ShortestPathAStar.Models
{
    public class Pin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Map
    {
        public List<Pin> Pins { get; set; }

        public Map(List<Pin> pins)
        {
            Pins = pins;
        }
    }
}
