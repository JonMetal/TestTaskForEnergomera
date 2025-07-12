namespace TestTaskForEnergomera.Models
{
    public class Field
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Size { get; set; }

        public Locations Locations { get; set; } = null!;
    }
}
