using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApi.Core;
using ShippingApi.Dtos.Address;
using ShippingApi.Dtos.Mapping;

namespace ShippingApi.Controllers
{
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressResponse>>> GetAddresses(CancellationToken cancellationToken)
        {
            var addresses = await _unitOfWork.Address.All(cancellationToken);
            return Ok(addresses.Select(a => a.ToResponse()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressResponse>> GetAddress(int id, CancellationToken cancellationToken)
        {
            var address = await _unitOfWork.Address.FindById(id, cancellationToken);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address.ToResponse());
        }

        [HttpPost]
        public async Task<ActionResult<AddressResponse>> CreateAddress(CreateAddressRequest request, CancellationToken cancellationToken)
        {
            var address = request.ToModel();
            await _unitOfWork.Address.Add(address, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address.ToResponse());
        }

        [HttpPut("{id}")]
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

            var address = request.ToModel();
            await _unitOfWork.Address.Update(address, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id, CancellationToken cancellationToken)
        {
            var address = await _unitOfWork.Address.FindById(id, cancellationToken);
            if (address == null)
            {
                return NotFound();
            }

            await _unitOfWork.Address.Delete(address, cancellationToken);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}

