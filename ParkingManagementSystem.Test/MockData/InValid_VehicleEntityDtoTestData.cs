using ParkingManagementSystem.Models.Dtos;
using ParkingManagementSystem.Models.Enums;
using System.Collections;
using System.Collections.Generic;

namespace ParkingManagementSystem.Test.MockData
{
    public class InValid_VehicleEntityDtoTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new VehicleEntityDto() { CarType = CarType.None , VehicleNumber = "MH21-BC1234" } };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}