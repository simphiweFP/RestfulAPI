using Microsoft.AspNetCore.Mvc;
using ShippingApi.Data;
using ShippingApi.Models;
using ShippingApi.Core;
using System.Runtime.CompilerServices;


namespace ShippingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : Controller
    {
      
        private readonly IUnitOfWork _unitOfWork;

        public DriversController(IUnitOfWork unitOfWork) //Contrator
        {
            _unitOfWork = unitOfWork;// Dependency
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _unitOfWork.Drivers.All());
        }

        [HttpGet]
        [Route(template: "GetById")]
        public async Task<IActionResult> Get(int id)
        {
            var driver = await _unitOfWork.Drivers.FindById(id);
            LogMessage($"Retrieving driver with ID {id}");
            return Ok(driver);
        }

        [HttpPost]
        [Route(template: "AddDriver")]
        public async Task<IActionResult> AddDriver(Driver driver)
        {
            await _unitOfWork.Drivers.Add(driver);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpDelete]
        [Route(template: "DeleteDriver")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var driver = await _unitOfWork.Drivers.FindById(id);
            if (driver != null)
            {
                await _unitOfWork.Drivers.Delete(driver);
                await _unitOfWork.CompleteAsync();
                return NoContent();

            }
            return NotFound();
        }

        [HttpPatch]
        [Route(template: "UpdateDriver")]
        public async Task<IActionResult> UpdateDriver(Driver driver)
        {
            var existDriver = await _unitOfWork.Drivers.FindById(driver.Id);

            if (existDriver != null)
            {

                await _unitOfWork.Drivers.Update(driver);
                await _unitOfWork.CompleteAsync();
                return Ok();
            }
            return NotFound();

        }
        private void LogMessage(string message,
                                [CallerMemberName] string methodName = "",
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
        {
            Console.WriteLine($"Method: {methodName}, File: {sourceFilePath}, Line: {sourceLineNumber}, Message: {message}");
        }
    }
    }


