using System.Text.Json.Serialization;

namespace ParkingManagementSystem.Models.Enums
{
    [JsonConverterAttribute(typeof(JsonStringEnumConverter))]
    public enum ParkingType
    {
        NA = 0,
        Small = 1,
        Medium = 2,
        Large = 3
    }
}
