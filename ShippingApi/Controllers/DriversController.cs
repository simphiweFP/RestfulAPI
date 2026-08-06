using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApi.Core;
using ShippingApi.Dtos.Driver;
using ShippingApi.Dtos.Common;
using ShippingApi.Dtos.Mapping;

namespace ShippingApi.Controllers
{
    /// <summary>
    /// Manages driver records.
    /// </summary>
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

        /// <summary>Gets all drivers.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<DriverResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<DriverResponse>>> GetDrivers([FromQuery] DriverQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            var pagedDrivers = await _unitOfWork.Drivers.GetDriversAsync(queryParameters, cancellationToken);
            return Ok(new PagedResponse<DriverResponse>
            {
                Items = pagedDrivers.Items.Select(driver => driver.ToResponse()).ToList(),
                PageNumber = pagedDrivers.PageNumber,
                PageSize = pagedDrivers.PageSize,
                TotalCount = pagedDrivers.TotalCount,
                TotalPages = pagedDrivers.TotalPages
            });
        }

        /// <summary>Gets a driver by id.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DriverResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DriverResponse>> GetDriver(int id, CancellationToken cancellationToken)
        {
            var driver = await _unitOfWork.Drivers.FindById(id, cancellationToken);
            if (driver == null)
            {
                return NotFound();
            }
            return Ok(driver.ToResponse());
        }

        /// <summary>Creates a new driver.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(DriverResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<DriverResponse>> CreateDriver(CreateDriverRequest request, CancellationToken cancellationToken)
        {
            var driver = request.ToModel();
            await _unitOfWork.Drivers.Add(driver, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver.ToResponse());
        }

        /// <summary>Updates an existing driver.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            existingDriver.Name = request.Name;
            existingDriver.Email = request.Email;
            existingDriver.DriverNumber = request.DriverNumber;
            existingDriver.Team = request.Team;
            existingDriver.Address = request.Address?.ToModel();
            await _unitOfWork.Drivers.Update(existingDriver, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return NoContent();
        }

        /// <summary>Deletes a driver.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDriver(int id, CancellationToken cancellationToken)
        {
            var driver = await _unitOfWork.Drivers.FindById(id, cancellationToken);
            if (driver == null)
            {
                return NotFound();
            }

            await _unitOfWork.Drivers.Delete(driver, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return NoContent();
        }
    }
}

