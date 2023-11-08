using Microsoft.Extensions.Logging;
using ParkingManagementSystem.Business.Exceptions;
using ParkingManagementSystem.Business.Interfaces;
using ParkingManagementSystem.Models.Dtos;
using ParkingManagementSystem.Models.Entities;
using ParkingManagementSystem.Models.Enums;

namespace ParkingManagementSystem.Business.Services
{
    public class ParkingService : IParkingService
    {
        private readonly ILogger<ParkingService> _logger;
        private readonly IPmsRepository _repository;

        public ParkingService(IPmsRepository repository, ILogger<ParkingService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<ParkingStatusDto>> GetParkingSlotsByTypeAsync(CarType vehicleType)
        {
            return await _repository.GetParkingSlotByTypeAsync(vehicleType);
        }

        public async Task<int> GetParkingNumberByTypeAsync(CarType vehicleType)
        {
            if (vehicleType == CarType.None)
                throw new CustomException("Invalid Car Type provided. Please provide valid input.");

            return await _repository.GetParkingNumberByTypeAsync(vehicleType);
        }

        public async Task<int> AllocateParkingSlotAsync(VehicleEntityDto vehicleDetails)
        {
            var nearestAvailableParkingNumber = await GetParkingNumberByTypeAsync(vehicleDetails.CarType);
            var parkingType = nearestAvailableParkingNumber <= 50 ? ParkingType.Small : nearestAvailableParkingNumber > 50 &&
                              nearestAvailableParkingNumber <= 80 ? ParkingType.Medium : nearestAvailableParkingNumber > 80 &&
                              nearestAvailableParkingNumber <= 100 ? ParkingType.Large : ParkingType.NA;

            return await _repository.AllocateParkingSlotAsync(new ParkingEntity
            {
                ParkingNumber = nearestAvailableParkingNumber,
                VehicleNumber = vehicleDetails.VehicleNumber,
                ParkingType = parkingType,
                CarType = vehicleDetails.CarType,
                ParkingStatus = ParkingStatus.Occupied
            });
        }

        public async Task<int> DeallocateParkingSlotAsync(string vehicleNumber)
        {
            return await _repository.DeallocateParkingSlotAsync(vehicleNumber);
        }
    }
}
