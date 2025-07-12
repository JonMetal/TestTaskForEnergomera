using TestTaskForEnergomera.Models;

namespace TestTaskForEnergomera.Services
{
    public interface IFieldService
    {
        double Distance(double Lat, double Lon, int id);
        (int Id, string Name)? TryFindFieldByPoint(double Lat, double Lon);
        IList<Field> GetAllFields();
        double GetFieldSize(int id);
    }
}