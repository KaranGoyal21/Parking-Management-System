using Microsoft.Extensions.Logging;
using ParkingManagementSystem.Business.Exceptions;
using ParkingManagementSystem.Business.Helper;
using ParkingManagementSystem.Business.Interfaces;
using ParkingManagementSystem.Models.Dtos;
using ParkingManagementSystem.Models.Entities;
using ParkingManagementSystem.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingManagementSystem.Test.MockRepo
{
    public class MockPmsRepository : IPmsRepository
    {
        private readonly ILogger<MockPmsRepository> _logger;
        List<ParkingEntity> _masterParkingSlot = new List<ParkingEntity>();

        public MockPmsRepository(ILogger<MockPmsRepository> logger)
        {
            _masterParkingSlot.AddRange(DbHelper.DBFeeder());
            _logger = logger;
        }

        public async Task<List<ParkingStatusDto>> GetParkingSlotByTypeAsync(CarType vehicleType)
        {
            return await GetAllSlotsByTypeAsync(vehicleType);
        }

        public async Task<int> GetParkingNumberByTypeAsync(CarType vehicleType)
        {
            var parkingEntityCollection = await GetAllSlotsByTypeAsync(vehicleType);

            foreach (var ps in parkingEntityCollection)
            {
                var nearestParkingNumber = ps.AvailableParkingNumbers?.FirstOrDefault() ?? default;
                if (nearestParkingNumber > default(int))
                {
                    return nearestParkingNumber;
                }
            }

            throw new CustomException("Parking Full, unable to allocate any parking.");
        }

        public async Task<int> AllocateParkingSlotAsync(ParkingEntity vehicleDetails)
        {
            var parkingSlot = _masterParkingSlot.Where(x => x.ParkingNumber == vehicleDetails.ParkingNumber && x.ParkingStatus == ParkingStatus.Available)
                                                .FirstOrDefault();

            if (parkingSlot == null)
                throw new CustomException("Error while allocating parking number.");

            parkingSlot.ParkingStatus = vehicleDetails.ParkingStatus;
            parkingSlot.CarType = vehicleDetails.CarType;
            parkingSlot.ParkingType = vehicleDetails.ParkingType;

            return await Task.FromResult(vehicleDetails.ParkingNumber);
        }

        public async Task<int> DeallocateParkingSlotAsync(string vehicleNumber)
        {
            var parkingSlot = _masterParkingSlot.Where(x => x.VehicleNumber == vehicleNumber && x.ParkingStatus == ParkingStatus.Occupied)
                                                .FirstOrDefault();

            if (parkingSlot == null)
                throw new CustomException("Error while deallocating parking number.");

            parkingSlot.ParkingStatus = ParkingStatus.Available;
            parkingSlot.CarType = CarType.None;

            return await Task.FromResult(parkingSlot.ParkingNumber);
        }

        #region Private Methods

        private async Task<List<ParkingStatusDto>> GetAllSlotsAsync()
        {
            var availableParkingSlots = _masterParkingSlot.Where(x => x.ParkingStatus == ParkingStatus.Available).Select(y => y.ParkingNumber).OrderBy(i => i);
            var availableSmallParkingSlots = availableParkingSlots.Where(x => x <= 50).ToList();
            var availableMediumParkingSlots = availableParkingSlots.Where(x => x > 50 && x <= 80).ToList();
            var availableLargeParkingSlots = availableParkingSlots.Where(x => x > 80 && x <= 100).ToList();

            var occupiedAllParkingSlots = _masterParkingSlot.Where(x => x.ParkingStatus == ParkingStatus.Occupied).Select(y => y.ParkingNumber).OrderBy(i => i);
            var occupiedSmallParkingNumbers = occupiedAllParkingSlots.Where(x => x <= 50).ToList();
            var occupiedMediumParkingNumbers = occupiedAllParkingSlots.Where(x => x > 50 && x <= 80).ToList();
            var occupiedLargeParkingNumbers = occupiedAllParkingSlots.Where(x => x > 80 && x <= 100).ToList();

            return await Task.FromResult(new List<ParkingStatusDto>
            {
                new ParkingStatusDto()
                {
                    OccupiedParkingNumbers = occupiedSmallParkingNumbers,
                    AvailableParkingNumbers = availableSmallParkingSlots,
                    ParkingType = ParkingType.Small
                },
                new ParkingStatusDto()
                {
                    OccupiedParkingNumbers = occupiedMediumParkingNumbers,
                    AvailableParkingNumbers = availableMediumParkingSlots,
                    ParkingType = ParkingType.Medium
                },
                new ParkingStatusDto()
                {
                    OccupiedParkingNumbers = occupiedLargeParkingNumbers,
                    AvailableParkingNumbers = availableLargeParkingSlots,
                    ParkingType = ParkingType.Large
                }
            });
        }

        private async Task<List<ParkingStatusDto>> GetAllSlotsByTypeAsync(CarType vehicleType)
        {
            var parkingEntityCollection = await GetAllSlotsAsync();

            switch (vehicleType)
            {
                case CarType.SUVOrLargeCars:
                    return parkingEntityCollection.Where(x => x.ParkingType == ParkingType.Large).ToList();
                case CarType.SedanOrCompactSUV:
                    return parkingEntityCollection.Where(x => x.ParkingType == ParkingType.Medium || x.ParkingType == ParkingType.Large).ToList();
                case CarType.Hatchback:
                case CarType.None:
                default:
                    return parkingEntityCollection;
            }
        }
        #endregion
    }
}
