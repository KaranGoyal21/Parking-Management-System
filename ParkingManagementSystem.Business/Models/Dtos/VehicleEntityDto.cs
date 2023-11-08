using ParkingManagementSystem.Models.Enums;

namespace ParkingManagementSystem.Models.Dtos
{
    public class VehicleEntityDto
    {
        public CarType CarType { get; set; }
        public string VehicleNumber { get; set; }
    }
}
