using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApi.Core;
using ShippingApi.Dtos.Driver;
using ShippingApi.Dtos.Mapping;

namespace ShippingApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/drivers")]
    public class DriversController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DriversController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverResponse>>> GetDrivers(CancellationToken cancellationToken)
        {
            var drivers = await _unitOfWork.Drivers.All(cancellationToken);
            return Ok(drivers.Select(d => d.ToResponse()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverResponse>> GetDriver(int id, CancellationToken cancellationToken)
        {
            var driver = await _unitOfWork.Drivers.FindById(id, cancellationToken);
            if (driver == null)
            {
                return NotFound();
            }
            return Ok(driver.ToResponse());
        }

        [HttpPost]
        public async Task<ActionResult<DriverResponse>> CreateDriver(CreateDriverRequest request, CancellationToken cancellationToken)
        {
            var driver = request.ToModel();
            await _unitOfWork.Drivers.Add(driver, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver.ToResponse());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(int id, UpdateDriverRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var existingDriver = await _unitOfWork.Drivers.FindById(id, cancellationToken);
            if (existingDriver == null)
            {
                return NotFound();
            }

            var driver = request.ToModel();
            await _unitOfWork.Drivers.Update(driver, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id, CancellationToken cancellationToken)
        {
            var driver = await _unitOfWork.Drivers.FindById(id, cancellationToken);
            if (driver == null)
            {
                return NotFound();
            }

            await _unitOfWork.Drivers.Delete(driver, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}

