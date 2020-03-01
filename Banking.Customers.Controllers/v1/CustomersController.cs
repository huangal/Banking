using System.Linq;
using System.Threading.Tasks;
using Banking.Customers.Controllers.Attributes;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Customers.Controllers.v1
{
    /// <summary>
    /// Customers Services provide customer information for banking
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
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
        /// <returns>List of Cutomer objects</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _dataService.GetCustomersAsync();
            if (!customers.Any()) return NotFound();
            else return Ok(customers);

        }

        /// <summary>
        /// Get a single customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Customers/25
        ///     
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await _dataService.GetCustomersAsync(id);
            return customer != null ? Ok(customer) : (IActionResult)NotFound();
        }


        /// <summary>
        /// Create a new customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Customers
        ///     {
        ///       "name": "Peter",
        ///       "last": "Doe",
        ///       "age": 45,
        ///       "email": "user@example.com",
        ///       "product": "VISA"
        ///      }
        ///
        /// </remarks>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelValidation]
        public async Task<IActionResult> Post([FromBody] CustomerModel customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _dataService.CreateCustomerAsync(customer);

            return Ok(result);
        }


        /// <summary>
        /// Update customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Customers/25
        ///     {
        ///       "id": 25,
        ///       "name": "Peter",
        ///       "last": "Doe",
        ///       "age": 45,
        ///       "email": "Peter.Dow@example.com",
        ///       "product": "VISA"
        ///      }
        ///
        /// </remarks>
        /// <param name="customer"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/Customers/25
        ///     
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Get a requested number of customer
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     GET api/Customers/List/10
        ///     
        /// </remarks>
        /// <param name="numberofrecords"></param>
        /// <returns></returns>
        [HttpGet("List/{numberofrecords}")]
        public async Task<IActionResult> GetList(int numberofrecords)
        {
            var customers = await _dataService.GetCustomerListAsync(numberofrecords);
            if (!customers.Any()) return NotFound();
            else return Ok(customers);

        }


        /// <summary>
        /// Get total number of customers
        /// </summary>
        /// <returns></returns>
        [HttpGet("Count")]
        public IActionResult GetCount()
        {
            var count = _dataService.GetCustomersCount();
            return Ok(count);
        }




    }
}
