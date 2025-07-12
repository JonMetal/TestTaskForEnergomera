using System.Text.Json.Serialization;
using TestTaskForEnergomera.Models;

namespace TestTaskForEnergomera.Tools
{
    [JsonSerializable(typeof(Locations))]
    [JsonSerializable(typeof(GeoPoint))]
    [JsonSerializable(typeof(IList<Field>))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(FieldDetail))]
    public partial class AppJsonSerializerContext : JsonSerializerContext
    {
    }
}
