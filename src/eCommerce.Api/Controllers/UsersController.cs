using eCommerce.Api.Models;
using eCommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eCommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;


        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _repository.Get();

            return Ok(users);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] int page, int quantityPerPage)
        {
            var users = await _repository.GetPaginated(page, quantityPerPage);

            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repository.Get(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("Multi/{id:int}")]
        public async Task<IActionResult> GetMultiple(int id)
        {
            var user = await _repository.GetMultiple(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("SP/GetUsers")]
        public async Task<IActionResult> SP_Get()
        {
            var result = await _repository.SP_Get();

            return Ok(result);
        }


        [HttpGet("SP/GetUser/{id}")]
        public async Task<IActionResult> SP_Get(int id)
        {
            var result = await _repository.SP_Get(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Usuario user)
        {
            var result = await _repository.Insert(user);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Usuario user)
        {
            var result = await _repository.Update(user);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.Delete(id);

            if (result == false)
                return NotFound();

            return Ok("User removed successfully.");
        }
    }
}
