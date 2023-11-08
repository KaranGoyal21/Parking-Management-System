using ParkingManagementSystem.Models.Dtos;
using ParkingManagementSystem.Models.Entities;
using ParkingManagementSystem.Models.Enums;

namespace ParkingManagementSystem.Business.Interfaces
{
    public interface IPmsRepository
    {
        Task<List<ParkingStatusDto>> GetParkingSlotByTypeAsync(CarType vehicleType);
        Task<int> GetParkingNumberByTypeAsync(CarType vehicleType);
        Task<int> AllocateParkingSlotAsync(ParkingEntity vehicleDetails);
        Task<int> DeallocateParkingSlotAsync(string vehicleNumber);
    }
}
