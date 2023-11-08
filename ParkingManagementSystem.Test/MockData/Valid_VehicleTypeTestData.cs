using ParkingManagementSystem.Models.Enums;
using System.Collections;
using System.Collections.Generic;

namespace ParkingManagementSystem.Test.MockData
{
    public class Valid_VehicleTypeTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { CarType.None };
            yield return new object[] { CarType.Hatchback };
            yield return new object[] { CarType.SedanOrCompactSUV };
            yield return new object[] { CarType.SUVOrLargeCars };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}