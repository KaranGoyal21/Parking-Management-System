using ParkingManagementSystem.Models.Dtos;
using ParkingManagementSystem.Models.Enums;
using System.Collections;
using System.Collections.Generic;

namespace ParkingManagementSystem.Test.MockData
{
    public class Valid_VehicleEntityDtoTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.Hatchback, VehicleNumber = "MH22-XC1234" } };
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.Hatchback, VehicleNumber = "MH13-YC1234" } };
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.Hatchback, VehicleNumber = "MH14-ZC1234" } };
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.Hatchback, VehicleNumber = "MH15-OC1234" } };
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.SedanOrCompactSUV, VehicleNumber = "MH16-PC1234" } };
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.SedanOrCompactSUV, VehicleNumber = "MH17-BQ1234" } };
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.SedanOrCompactSUV, VehicleNumber = "MH18-BR1234" } };
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.SUVOrLargeCars, VehicleNumber = "MH19-BS1234" } };
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.SUVOrLargeCars, VehicleNumber = "MH20-KC1234" } };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}