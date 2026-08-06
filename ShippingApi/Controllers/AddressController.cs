using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApi.Core;
using ShippingApi.Dtos.Address;
using ShippingApi.Dtos.Common;
using ShippingApi.Dtos.Mapping;

namespace ShippingApi.Controllers
{
    /// <summary>
    /// Manages address records.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>Gets all addresses.</summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of address records.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<AddressResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<AddressResponse>>> GetAddresses([FromQuery] AddressQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            var pagedAddresses = await _unitOfWork.Address.GetAddressesAsync(queryParameters, cancellationToken);
            return Ok(new PagedResponse<AddressResponse>
            {
                Items = pagedAddresses.Items.Select(address => address.ToResponse()).ToList(),
                PageNumber = pagedAddresses.PageNumber,
                PageSize = pagedAddresses.PageSize,
                TotalCount = pagedAddresses.TotalCount,
                TotalPages = pagedAddresses.TotalPages
            });
        }

        /// <summary>Gets a single address by id.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AddressResponse>> GetAddress(int id, CancellationToken cancellationToken)
        {
            var address = await _unitOfWork.Address.FindById(id, cancellationToken);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address.ToResponse());
        }

        /// <summary>Creates a new address.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<AddressResponse>> CreateAddress(CreateAddressRequest request, CancellationToken cancellationToken)
        {
            var address = request.ToModel();
            await _unitOfWork.Address.Add(address, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address.ToResponse());
        }

        /// <summary>Updates an existing address.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAddress(int id, UpdateAddressRequest request, CancellationToken cancellationToken)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var existingAddress = await _unitOfWork.Address.FindById(id, cancellationToken);
            if (existingAddress == null)
            {
                return NotFound();
            }

            existingAddress.Street = request.Street;
            existingAddress.City = request.City;
            existingAddress.ZipCode = request.ZipCode;
            await _unitOfWork.Address.Update(existingAddress, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return NoContent();
        }

        /// <summary>Deletes an address.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAddress(int id, CancellationToken cancellationToken)
        {
            var address = await _unitOfWork.Address.FindById(id, cancellationToken);
            if (address == null)
            {
                return NotFound();
            }

            await _unitOfWork.Address.Delete(address, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return NoContent();
        }
    }
}

