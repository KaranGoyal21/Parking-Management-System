using ParkingManagementSystem.Models.Dtos;
using ParkingManagementSystem.Models.Entities;
using ParkingManagementSystem.Models.Enums;

namespace ParkingManagementSystem.Business.Interfaces
{
    public interface IParkingService
    {
        Task<List<ParkingStatusDto>> GetParkingSlotsByTypeAsync(CarType vehicleType);
        Task<int> GetParkingNumberByTypeAsync(CarType vehicleType);
        Task<int> AllocateParkingSlotAsync(VehicleEntityDto vehicleDetails);
        Task<int> DeallocateParkingSlotAsync(string vehicleNumber);
    }
}
