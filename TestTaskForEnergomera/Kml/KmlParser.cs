using System.Xml.Linq;
using TestTaskForEnergomera.Models;

namespace TestTaskForEnergomera.Kml
{
    public static class KmlParser
    {
        private const string KmlNamespace = "http://www.opengis.net/kml/2.2";

        public static List<Field> ParseFieldsAndCenters(string fieldsPath, string centroidsPath)
        {
            var fields = ParseFields(fieldsPath);
            fields.AddCenters(centroidsPath);
            return fields;
        }

        private static List<Field> ParseFields(string filePath)
        {
            List<Field> fields = [];

            XDocument doc = XDocument.Load(filePath);
            XNamespace ns = KmlNamespace;

            foreach (var placemark in doc.Descendants(ns + "Placemark"))
            {
                string? name = placemark.Element(ns + "name")?.Value;
                if (string.IsNullOrEmpty(name)) { name = "Unknown"; }
                XElement? fidElement = placemark.Descendants(ns + "SimpleData")
                    .FirstOrDefault(e => e.Attribute("name")?.Value == "fid");

                XElement? sizeElement = placemark.Descendants(ns + "SimpleData")
                    .FirstOrDefault(e => e.Attribute("name")?.Value == "size");

                if (!int.TryParse(fidElement?.Value, out int fid)) continue;
                if (!double.TryParse(sizeElement?.Value, out double size)) continue;

                XElement? polygonCoordinates = placemark.Descendants(ns + "coordinates").FirstOrDefault();

                IList<GeoPoint> points = ParseCoordinates(polygonCoordinates?.Value ?? "");

                fields.Add(new Field
                {
                    Id = fid,
                    Name = name,
                    Size = size,
                    Locations = new Locations
                    {
                        Polygon = points,
                        Center = new GeoPoint(0, 0)
                    }
                });
            }

            return fields;
        }

        private static void AddCenters(this ICollection<Field> fields, string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            XNamespace ns = KmlNamespace;

            foreach (var placemark in doc.Descendants(ns + "Placemark"))
            {
                XElement? fidElement = placemark.Descendants(ns + "SimpleData")
                    .FirstOrDefault(e => e.Attribute("name")?.Value == "fid");

                XElement? pointCoordinates = placemark.Descendants(ns + "coordinates")
                    .FirstOrDefault();

                if (fidElement == null) continue;

                if (!int.TryParse(fidElement.Value, out int fid)) continue;

                string[] coords = pointCoordinates?.Value.Trim().Split(',') ?? [];

                if (!double.TryParse(coords[0], out double lon) ||
                    !double.TryParse(coords[1], out double lat)) continue;
                Field? field = fields.FirstOrDefault(l => l.Id == fid);
                if (field == null) continue;
                field.Locations.Center = new GeoPoint(lon, lat);
            }
        }

        private static List<GeoPoint> ParseCoordinates(string coordinateString)
        {
            List<GeoPoint> points = [];

            var coords = coordinateString.Trim().Split(' ')
                .Select(p => p.Split(','))
                .Where(parts => parts.Length >= 2 &&
                                double.TryParse(parts[0], out _) &&
                                double.TryParse(parts[1], out _))
                .ToList();

            foreach (var part in coords)
            {
                double Lon = double.Parse(part[0]);
                double Lat = double.Parse(part[1]);
                points.Add(new GeoPoint(Lon, Lat));
            }

            return points;
        }
    }
}
