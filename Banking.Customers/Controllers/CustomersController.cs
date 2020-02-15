using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.Customers.Attributes;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking.Customers.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private ICustomerService _dataService;

        public CustomersController(ICustomerService dataRepositoryService)
        {
            _dataService = dataRepositoryService;
        }


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



        //    List<Customer> customers = _context.Customers.Take(3).ToList();
        //    return customers;
        //}



        //[Authorize(Policy = "ServiceAccess")]



        //[Authorize(Policy = "ServiceAccess")]
        [HttpGet("Count")]
        public IActionResult GetCount()
        {
            var count = _dataService.GetCustomersCountAsync();
            return Ok(count);
        }




    }


}
