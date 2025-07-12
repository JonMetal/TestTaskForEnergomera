using TestTaskForEnergomera.Kml;
using TestTaskForEnergomera.Models;
using TestTaskForEnergomera.Tools;

namespace TestTaskForEnergomera.Services
{
    public class FieldService(IConfiguration configuration) : IFieldService
    {
        private readonly KmlFilesContext _kmlFilesContext = new(configuration);
        public IList<Field> GetAllFields()
        {
            return _kmlFilesContext.Fields;
        }

        public double GetFieldSize(int id)
        {
            Field? field = _kmlFilesContext.Fields.FirstOrDefault(f => f.Id == id);
            ArgumentNullException.ThrowIfNull(field);
            return field.Size;
        }

        public double Distance(double Lat, double Lon, int id)
        {
            GeoPoint geoPoint = new(Lat, Lon);
            Field? field = _kmlFilesContext.Fields.FirstOrDefault(l => l.Id == id);
            ArgumentNullException.ThrowIfNull(field);
            return Area.Distance(geoPoint, field.Locations.Center);
        }

        public (int Id, string Name)? TryFindFieldByPoint(double Lat, double Lon)
        {
            GeoPoint geoPoint = new(Lat, Lon);
            foreach (Field field in _kmlFilesContext.Fields)
            {
                if (Area.IsPointInPolygon(geoPoint, field.Locations.Polygon))
                {
                    return (field.Id, field.Name);
                }
            }

            return null;
        }
    }
}
