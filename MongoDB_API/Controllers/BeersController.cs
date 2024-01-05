using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using MongoDB_API.Models;
using MongoDB_API.Services;

namespace MongoDB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        private readonly BeersService _beersService;

        public BeersController(BeersService booksService) =>
            _beersService = booksService;

        [HttpGet]
        public async Task<List<Beers>> Get()
        {
            //return await _beersService.GetAsync();
            List<Beers> result = new List<Beers>();
            result.Add(new Beers() { Age=90});;
            return result;
        }
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Beers>> Get(string id)
        {
            var beers = await _beersService.GetAsync(id);

            if (beers is null)
            {
                return NotFound();
            }

            return beers;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Beers newBeers)
        {
            await _beersService.CreateAsync(newBeers);

            return CreatedAtAction(nameof(Get), new { id = newBeers.Id }, newBeers);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Beers updatedBeers)
        {
            var book = await _beersService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updatedBeers.Id = book.Id;

            await _beersService.UpdateAsync(id, updatedBeers);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var beers = await _beersService.GetAsync(id);

            if (beers is null)
            {
                return NotFound();
            }

            await _beersService.RemoveAsync(id);

            return NoContent();
        }
    }
}
