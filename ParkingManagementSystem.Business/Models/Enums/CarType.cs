using System.Text.Json.Serialization;

namespace ParkingManagementSystem.Models.Enums
{
    [JsonConverterAttribute(typeof(JsonStringEnumConverter))]
    public enum CarType
    {
        None = 0,
        Hatchback = 1,
        SedanOrCompactSUV = 2,
        SUVOrLargeCars = 3        
    }
}
