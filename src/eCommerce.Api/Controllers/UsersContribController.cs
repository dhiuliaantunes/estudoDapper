using eCommerce.Api.Models;
using eCommerce.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eCommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersContribController : ControllerBase
    {
        private readonly IUserContibRepository _repository;

        public UsersContribController(IUserContibRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _repository.Get();

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

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Usuario usuario)
        {
            var result = await _repository.Insert(usuario);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Usuario usuario)
        {
            var result = await _repository.Update(usuario);

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
