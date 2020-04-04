using System.Linq;
using System.Threading.Tasks;
using Banking.Customers.Controllers.Attributes;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Banking.Customers.Controllers.v2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private ICustomerService _dataService;

        public UsersController(ICustomerService dataRepositoryService)
        {
            _dataService = dataRepositoryService;
        }

        [HttpPost("Phone")]
        [ModelValidation]
        public IActionResult CreatePhoneRecord([FromBody] Phone phone)
        {

            return Ok(phone);

        }

        [HttpPost("Contact")]
        [ModelValidation]
        public IActionResult CreateContact([FromBody] CustomerCommunication communication)
        {

            return Ok(communication);

        }
    }
}
