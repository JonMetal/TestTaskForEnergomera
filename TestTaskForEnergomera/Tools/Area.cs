using TestTaskForEnergomera.Models;

namespace TestTaskForEnergomera.Tools
{
    public static class Area
    {
        public const double EarthRadius = 6376.5;

        public static double ToRadian(double x) => (Math.PI / 180.0) * x;

        public static double SizeFromLocations(Locations locs)
        {
            double area = 0;
            int n = locs.Polygon.Count;
            if (n < 3) return 0;

            for (int i = 0; i < n; i++)
            {
                GeoPoint firstPoint = locs.Polygon[i];
                GeoPoint secondPoint = locs.Polygon[(i + 1) % n];

                double lat1 = ToRadian(firstPoint.Lat);
                double lon1 = ToRadian(firstPoint.Lng);
                double lat2 = ToRadian(secondPoint.Lat);
                double lon2 = ToRadian(secondPoint.Lng);

                area += (lon2 - lon1) * (Math.Sin(lat1) + Math.Sin(lat2));
            }

            area = Math.Abs(area) * EarthRadius * EarthRadius / 2.0 / 10000.0;
            return area;
        }

        public static double Distance(GeoPoint firstGeoPoint, GeoPoint secondGeoPoint)
        {
            double dLat = ToRadian(secondGeoPoint.Lat - firstGeoPoint.Lat);
            double dLon = ToRadian(secondGeoPoint.Lng - firstGeoPoint.Lng);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                + Math.Cos(ToRadian(firstGeoPoint.Lat)) * Math.Cos(ToRadian(secondGeoPoint.Lat))
                * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = EarthRadius * c;

            return d * 1000;
        }

        public static bool IsPointInPolygon(GeoPoint point, IList<GeoPoint> polygon)
        {
            if (polygon == null || polygon.Count < 3)
                return false;

            bool isInside = false;
            for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                var xi = polygon[i].Lng;
                var yi = polygon[i].Lat;
                var xj = polygon[j].Lng;
                var yj = polygon[j].Lat;

                bool intersect = ((yi > point.Lat) != (yj > point.Lat)) &&
                                 (point.Lng < (xj - xi) * (point.Lat - yi) / (yj - yi) + xi);

                if (intersect)
                    isInside = !isInside;
            }

            return isInside;
        }
    }
}
