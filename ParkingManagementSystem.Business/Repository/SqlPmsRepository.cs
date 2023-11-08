using Microsoft.Extensions.Logging;
using ParkingManagementSystem.Business.Exceptions;
using ParkingManagementSystem.Business.Interfaces;
using ParkingManagementSystem.Models.Dtos;
using ParkingManagementSystem.Models.Entities;
using ParkingManagementSystem.Models.Enums;

namespace ParkingManagementSystem.Business.Repository
{
    public class SqlPmsRepository : IPmsRepository
    {
        private readonly PmsDbContext _dbContext;
        private readonly ILogger<SqlPmsRepository> _logger;

        public SqlPmsRepository(PmsDbContext dbContext, ILogger<SqlPmsRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            _dbContext.SeedDatabase();
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
            if (_dbContext.ParkingEntities.Any(x => x.VehicleNumber == vehicleDetails.VehicleNumber))
                throw new CustomException("Vehicle with same car number already exists.");

            if (string.IsNullOrEmpty(vehicleDetails.VehicleNumber))
                throw new CustomException("Invalid input, kindly provide valid vehicle number.");

            var parkingSlot = _dbContext.ParkingEntities.Where(x => x.ParkingNumber == vehicleDetails.ParkingNumber && x.ParkingStatus == ParkingStatus.Available)
                                                        .FirstOrDefault();

            if (parkingSlot == null)
                throw new CustomException("Error while allocating parking number.");

            parkingSlot.ParkingStatus = vehicleDetails.ParkingStatus;
            parkingSlot.CarType = vehicleDetails.CarType;

            _dbContext.Update(parkingSlot);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(vehicleDetails.ParkingNumber);
        }

        public async Task<int> DeallocateParkingSlotAsync(string vehicleNumber)
        {
            var parkingSlot = _dbContext.ParkingEntities.Where(x => x.VehicleNumber == vehicleNumber && x.ParkingStatus == ParkingStatus.Occupied)
                                                        .FirstOrDefault();

            if (parkingSlot == null)
                throw new CustomException("Error while deallocating parking number.");

            parkingSlot.VehicleNumber = string.Empty;
            parkingSlot.ParkingStatus = ParkingStatus.Available;
            parkingSlot.CarType = CarType.None;

            _dbContext.Update(parkingSlot);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(parkingSlot.ParkingNumber);
        }

        #region Private Methods
        private async Task<List<ParkingStatusDto>> GetAllSlotsAsync()
        {
            var availableParkingSlots = _dbContext.ParkingEntities.Where(x => x.ParkingStatus == ParkingStatus.Available).Select(y => y.ParkingNumber).OrderBy(i => i);
            var availableSmallParkingSlots = availableParkingSlots.Where(x => x <= 50).ToList();
            var availableMediumParkingSlots = availableParkingSlots.Where(x => x > 50 && x <= 80).ToList();
            var availableLargeParkingSlots = availableParkingSlots.Where(x => x > 80 && x <= 100).ToList();

            var occupiedAllParkingSlots = _dbContext.ParkingEntities.Where(x => x.ParkingStatus == ParkingStatus.Occupied).Select(y => y.ParkingNumber).OrderBy(i => i);
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
