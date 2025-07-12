namespace TestTaskForEnergomera.Models
{
    public class Locations
    {
        public GeoPoint Center { get; set; }
        public IList<GeoPoint> Polygon { get; set; } = [];
    }
}
