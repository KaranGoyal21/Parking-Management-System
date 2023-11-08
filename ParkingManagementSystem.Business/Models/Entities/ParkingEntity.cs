using ParkingManagementSystem.Models.Enums;

namespace ParkingManagementSystem.Models.Entities
{
    public class ParkingEntity
    {
        public int id { get; set; }
        public int ParkingNumber { get; set; }
        public string VehicleNumber { get; set; }
        public ParkingType ParkingType { get; set; }
        public CarType CarType { get; set; }
        public ParkingStatus ParkingStatus { get; set; }
    }
}
