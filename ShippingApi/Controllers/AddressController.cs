using Microsoft.AspNetCore.Mvc;
using ShippingApi.Data;
using ShippingApi.Models;
using ShippingApi.Core;


namespace ShippingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
      
        private readonly IUnitOfWork _unitOfWork;

        public AddressController(IUnitOfWork unitOfWork) //Contrator
        {
            _unitOfWork = unitOfWork;// Dependency
        }

        [HttpGet]
        [Route(template: "GetAddress")]
        public async Task<IActionResult> GetAddress()
        {
            return Ok(await _unitOfWork.Address.All());
        }

        [HttpGet]
        [Route(template: "GetAddressById")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var driver = await _unitOfWork.Address.FindById(id);
            return Ok(driver);
        }

        [HttpPost]
        [Route(template: "AddAddress")]
        public async Task<IActionResult> AddAddress(Address address)
        {
            await _unitOfWork.Address.Add(address);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        [Route(template: "DeleteAddress")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _unitOfWork.Address.FindById(id);
            if (address != null)
            {
                await _unitOfWork.Address.Delete(address);
                await _unitOfWork.CompleteAsync();
                return NoContent();

            }
            return NotFound();
        }

        [HttpPatch]
        [Route(template: "UpdateAddress")]
        public async Task<IActionResult> UpdateAddress(Address address)
        {
            var existDriver = await _unitOfWork.Address.FindById(address.Id);

            if (existDriver != null)
            {

                await _unitOfWork.Address.Update(address);
                await _unitOfWork.CompleteAsync();
                return Ok();
            }
            return NotFound();

        }
      }
    }


