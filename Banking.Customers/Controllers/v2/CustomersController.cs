using System.Linq;
using System.Threading.Tasks;
using Banking.Customers.Attributes;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Customers.Controllers.v2
{
    /// <summary>
    /// Customers Services provide demographic information
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomersController : ControllerBase
    {
        private ICustomerService _dataService;

        public CustomersController(ICustomerService dataRepositoryService)
        {
            _dataService = dataRepositoryService;
        }

        /// <summary>
        /// Get List of Customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _dataService.GetCustomersAsync();
            if (!customers.Any()) return NotFound();
            else return Ok(customers);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await _dataService.GetCustomersAsync(id);
            return customer != null ? Ok(customer) : (IActionResult)NotFound();
        }


        [HttpPost]
        [ModelValidation]
        public async Task<IActionResult> Post([FromBody] CustomerModel customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _dataService.CreateCustomerAsync(customer);

            return Ok(result);
        }


        [HttpPut("{id}")]
        [ModelValidation]
        public async Task<IActionResult> Put([FromBody] CustomerModel customer, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (customer.Id != id) return BadRequest();
            var response = await _dataService.GetCustomersAsync(id);
            if (response == null) return NotFound();

            var result = _dataService.UpdateCustomerAsync(customer);

            return Ok(result.Result);
        }

        [HttpDelete("{id}")]
        [ModelValidation]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _dataService.GetCustomersAsync(id);
            if (response == null) return NotFound();

            var result = await _dataService.DeleteCustomerAsync(id);
            if (!result) return Content(StatusCodes.Status406NotAcceptable.ToString(), "Unable to process your request.");

            return NoContent();
        }


        [HttpGet("List/{numberofrecords}")]
        public async Task<IActionResult> GetList(int numberofrecords)
        {
            var customers = await _dataService.GetCustomerListAsync(numberofrecords);
            if (!customers.Any()) return NotFound();
            else return Ok(customers);

        }


        [HttpGet("Count")]
        public IActionResult GetCount()
        {
            var count = _dataService.GetCustomersCount();
            return Ok(count);
        }




    }


}
