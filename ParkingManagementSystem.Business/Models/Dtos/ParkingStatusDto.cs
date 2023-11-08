using ParkingManagementSystem.Models.Enums;

namespace ParkingManagementSystem.Models.Dtos
{
    public class ParkingStatusDto
    {
        public List<int> AvailableParkingNumbers { get; set; }
        public List<int> OccupiedParkingNumbers { get; set; }
        public ParkingType ParkingType { get; set; }
        public int AvailableCount => AvailableParkingNumbers.Count;
        public int OccupiedCount => OccupiedParkingNumbers.Count;
        public int TotalCount => AvailableCount + OccupiedCount;
    }
}
