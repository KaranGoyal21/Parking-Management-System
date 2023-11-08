using ParkingManagementSystem.Models.Entities;
using ParkingManagementSystem.Models.Enums;

namespace ParkingManagementSystem.Business.Helper
{
    public static class DbHelper
    {
        public static List<ParkingEntity> DBFeeder()
        {
            var parkedVehicleCollection = GetDummyParkingData();
            List<ParkingEntity> dbSeedData = new List<ParkingEntity>();

            for (int pno = 1; pno <= 100; pno++)
            {
                var parkedVehicleData = parkedVehicleCollection.Where(x => x.ParkingNumber == pno).Select(y => y).FirstOrDefault();
                if (parkedVehicleData != null)
                {
                    dbSeedData.Add(parkedVehicleData);
                }
                else
                {
                    var ptype = pno <= 50 ? ParkingType.Small : pno > 50 && pno <= 80 ? ParkingType.Medium : pno > 80 && pno <= 100 ? ParkingType.Large : ParkingType.NA;

                    dbSeedData.Add(new ParkingEntity()
                    {
                        ParkingNumber = pno,
                        CarType = CarType.None,
                        ParkingType = ptype,
                        VehicleNumber = string.Empty,
                        ParkingStatus = ParkingStatus.Available
                    });
                }
            }

            return dbSeedData;
        }

        private static List<ParkingEntity> GetDummyParkingData()
        {
            return new List<ParkingEntity>() {
                new ParkingEntity() { ParkingNumber = 1, VehicleNumber="MH12-BC1234",  CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 2, VehicleNumber="MH13-BC1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 85, VehicleNumber="MH14-AC1234", CarType = CarType.SUVOrLargeCars, ParkingType = ParkingType.Large, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 3, VehicleNumber="MH12-GB1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 4, VehicleNumber="MH12-TH1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 5, VehicleNumber="MH12-CC1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 62, VehicleNumber="MH15-AC1234", CarType = CarType.SedanOrCompactSUV, ParkingType = ParkingType.Medium, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 6, VehicleNumber="MH16-XC1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 7, VehicleNumber="MH17-BC1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 98, VehicleNumber="MH18-BX1234", CarType = CarType.SUVOrLargeCars, ParkingType = ParkingType.Large, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 8, VehicleNumber="MH19-BC1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 9, VehicleNumber="MH20-GG1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 70, VehicleNumber="MH21-FF1234", CarType = CarType.SedanOrCompactSUV, ParkingType = ParkingType.Medium, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 10, VehicleNumber="MH22-FY1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 11, VehicleNumber="MH23-XD1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied},
                new ParkingEntity() { ParkingNumber = 12, VehicleNumber="MH24-XC1234", CarType = CarType.Hatchback, ParkingType = ParkingType.Small, ParkingStatus = ParkingStatus.Occupied}
            };
        }
    }
}
