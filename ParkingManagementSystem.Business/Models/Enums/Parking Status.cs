using System.Text.Json.Serialization;

namespace ParkingManagementSystem.Models.Enums
{
    [JsonConverterAttribute(typeof(JsonStringEnumConverter))]
    public enum ParkingStatus
    {
        Available = 1,
        Occupied = 2,
        OnHold = 3
    }
}
