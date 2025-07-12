using TestTaskForEnergomera.Models;

namespace TestTaskForEnergomera.Kml
{
    public class KmlFilesContext
    {
        public IList<Field> Fields { get; set; }

        public KmlFilesContext(IConfiguration configuration)
        {
            Paths? paths = configuration.GetSection("Paths").Get<Paths>();
            ArgumentNullException.ThrowIfNull(paths);
            Fields = KmlParser.ParseFieldsAndCenters(paths.Fields, paths.Centroids);
        }
    }
}
