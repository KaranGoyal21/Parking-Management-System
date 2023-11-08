using Microsoft.AspNetCore.Mvc;
using ParkingManagementSystem.Business.Interfaces;
using ParkingManagementSystem.Models.Dtos;
using ParkingManagementSystem.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace ParkingManagementSystem.Controllers
{
    [ApiController]
    [Route("pms")]
    public class ParkingController : Controller
    {
        private readonly IParkingService _service;

        public ParkingController(IParkingService service)
        {
            _service = service;
        }

        /// <summary>
        /// This will return parking slots by passing car type
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "This will return parking slots by passing car type")]
        [HttpGet("all-parking-status")]
        public async Task<IActionResult> GetParkingSlotsByType([FromQuery] CarType vehicleType)
        {
            return Ok(await _service.GetParkingSlotsByTypeAsync(vehicleType));
        }

        /// <summary>
        /// This will return parking slot number by passing car type
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "This will return parking slot number by passing car type")]
        [HttpGet("parking-number/{vehicleType}")]
        public async Task<IActionResult> GetParkingNumberByType(CarType vehicleType)
        {
            return Ok(await _service.GetParkingNumberByTypeAsync(vehicleType));
        }

        /// <summary>
        /// Allocate parking for new vehicle
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Allocate parking for new vehicle")]
        [HttpPost("allocate-parking")]
        public async Task<IActionResult> AllocateParking([FromBody] VehicleEntityDto vehicleDetails)
        {
            return Ok(await _service.AllocateParkingSlotAsync(vehicleDetails));
        }

        /// <summary>
        /// Deallocate parking for the parked vehicle
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Deallocate parking for the parked vehicle")]
        [HttpPost("deallocate-parking")]
        public async Task<IActionResult> DeallocateParking([FromBody] VehicleEntityDto vehicleDetails)
        {
            return Ok(await _service.DeallocateParkingSlotAsync(vehicleDetails.VehicleNumber));
        }
    }
}