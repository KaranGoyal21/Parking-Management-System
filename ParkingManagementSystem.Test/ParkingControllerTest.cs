using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using ParkingManagementSystem.Business.Exceptions;
using ParkingManagementSystem.Business.Interfaces;
using ParkingManagementSystem.Business.Repository;
using ParkingManagementSystem.Business.Services;
using ParkingManagementSystem.Controllers;
using ParkingManagementSystem.Models.Dtos;
using ParkingManagementSystem.Models.Enums;
using ParkingManagementSystem.Test.MockData;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ParkingManagementSystem.Test
{
    public class ParkingControllerTest
    {
        private IPmsRepository _mockTestDb;

        public ParkingControllerTest()
        {
            var mockDbOptions = new DbContextOptionsBuilder<PmsDbContext>()
                                   .UseInMemoryDatabase("ParkingControllerTest")
                                   .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                                   .Options;

            var mockDbContext = new PmsDbContext(mockDbOptions);
            mockDbContext.Database.EnsureDeleted();
            mockDbContext.Database.EnsureCreated();

            mockDbContext.SeedDatabase();

            //_mockTestDb = new MockPmsRepository(new Mock<ILogger<MockPmsRepository>>().Object);
            _mockTestDb = new SqlPmsRepository(mockDbContext, new Mock<ILogger<SqlPmsRepository>>().Object);
        }

        [Fact]
        public async Task FetchAllParkingSlot_ValidData_Succeeds()
        {
            //Arrange
            var inputVehicleType = CarType.None;
            var mockParkingService = new Mock<IParkingService>();
            mockParkingService.Setup(service => service.GetParkingSlotsByTypeAsync(inputVehicleType))
                              .Returns(_mockTestDb.GetParkingSlotByTypeAsync(inputVehicleType));
            var parkingController = new ParkingController(mockParkingService.Object);

            //Act
            var expectedData = _mockTestDb.GetParkingSlotByTypeAsync(inputVehicleType).Result;
            var actualResponse = await parkingController.GetParkingSlotsByType(inputVehicleType);
            var actualResponseData = (actualResponse.Should().BeOfType<OkObjectResult>().Subject).Value as List<ParkingStatusDto>;

            //Assert
            Assert.NotNull(actualResponse);
            Assert.Equal(expectedData.Count, actualResponseData?.Count);
        }

        [Theory]
        [ClassData(typeof(Valid_VehicleTypeTestData))]
        public async Task FetchParkingSlotByType_ValidData_Succeeds(CarType carType)
        {
            //Arrange
            var mockParkingService = new Mock<IParkingService>();
            mockParkingService.Setup(service => service.GetParkingSlotsByTypeAsync(carType))
                              .Returns(_mockTestDb.GetParkingSlotByTypeAsync(carType));
            var parkingController = new ParkingController(mockParkingService.Object);

            //Act
            var expectedData = _mockTestDb.GetParkingSlotByTypeAsync(carType).Result;
            var actualResponse = await parkingController.GetParkingSlotsByType(carType);
            var actualResponseData = (actualResponse.Should().BeOfType<OkObjectResult>().Subject).Value as List<ParkingStatusDto>;

            //Assert
            Assert.NotNull(actualResponse);
            Assert.Equal(expectedData.Count, actualResponseData?.Count);
        }


        [Theory]
        [ClassData(typeof(Valid_VehicleTypeTestData))]
        public async Task FetchParkingNumberByType_ValidData_Succeeds(CarType carType)
        {
            //Arrange
            var mockParkingService = new Mock<IParkingService>();
            mockParkingService.Setup(service => service.GetParkingNumberByTypeAsync(carType))
                              .Returns(_mockTestDb.GetParkingNumberByTypeAsync(carType));
            var parkingController = new ParkingController(mockParkingService.Object);

            //Act
            var expectedData = await _mockTestDb.GetParkingNumberByTypeAsync(carType);
            var actualResponse = await parkingController.GetParkingNumberByType(carType);
            var actualResponseData = (int?)actualResponse.Should().BeOfType<OkObjectResult>().Subject.Value;

            //Assert
            Assert.NotNull(actualResponse);
            Assert.Equal(expectedData, actualResponseData);
        }

        [Theory]
        [ClassData(typeof(Valid_VehicleEntityDtoTestData))]
        public async Task AllocateParking_ValidData_Succeeds(VehicleEntityDto inputVehicleDto)
        {
            //Arrange
            var parkingService = new ParkingService(_mockTestDb, new Mock<ILogger<ParkingService>>().Object);
            var parkingController = new ParkingController(parkingService);

            //Act
            var expectedParkingNumber = await _mockTestDb.GetParkingNumberByTypeAsync(inputVehicleDto.CarType);
            var actualResponse = await parkingController.AllocateParking(inputVehicleDto);
            var actualResponseData = (int?)actualResponse.Should().BeOfType<OkObjectResult>().Subject.Value;

            //Assert
            Assert.NotNull(actualResponse);
            Assert.Equal(expectedParkingNumber, actualResponseData);
        }

        [Theory]
        [ClassData(typeof(InValid_VehicleEntityDtoTestData))]
        public async Task AllocateParking_InValidData_ShouldThrowException(VehicleEntityDto inputVehicleDto)
        {
            //Arrange
            var parkingService = new ParkingService(_mockTestDb, new Mock<ILogger<ParkingService>>().Object);
            var parkingController = new ParkingController(parkingService);
            var expectedException = "Invalid Car Type provided. Please provide valid input.";

            //Act
            var actualException = await Assert.ThrowsAsync<CustomException>(() => parkingController.AllocateParking(inputVehicleDto));

            //Assert
            Assert.Equal(expectedException, actualException.Message);
        }

        [Fact]
        public async Task DeallocateParking_ValidData_Succeeds()
        {
            //Arrange
            var inputVehicleDto = new VehicleEntityDto() { CarType = CarType.Hatchback, VehicleNumber = "MH12-BC1234" };
            var parkingService = new ParkingService(_mockTestDb, new Mock<ILogger<ParkingService>>().Object);
            var parkingController = new ParkingController(parkingService);

            //Act
            var expectedParkingNumber = 1;
            var actualResponse = await parkingController.DeallocateParking(inputVehicleDto);
            var actualResponseData = (int?)actualResponse.Should().BeOfType<OkObjectResult>().Subject.Value;

            //Assert
            Assert.NotNull(actualResponse);
            Assert.Equal(expectedParkingNumber, actualResponseData);
        }

        [Fact]
        public async Task DeallocateParking_InValidData_ShouldThrowException()
        {
            //Arrange
            var inputVehicleDto = new VehicleEntityDto() { CarType = CarType.Hatchback, VehicleNumber = "MH12-DF1234" };
            var parkingService = new ParkingService(_mockTestDb, new Mock<ILogger<ParkingService>>().Object);
            var parkingController = new ParkingController(parkingService);
            var expectedException = "Error while deallocating parking number.";
            
            //Act
            var actualException = await Assert.ThrowsAsync<CustomException>(() => parkingController.DeallocateParking(inputVehicleDto));

            //Assert
            Assert.Equal(expectedException, actualException.Message);            
        }
    }
}