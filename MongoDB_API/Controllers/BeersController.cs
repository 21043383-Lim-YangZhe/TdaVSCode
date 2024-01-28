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

        public BeersController(BeersService beersService) =>
            _beersService = beersService;

        [HttpGet]
        public async Task<List<Beers>> Get() =>
      await _beersService.GetAsync();


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
            var beers = await _beersService.GetAsync(id);

            if (beers is null)
            {
                return NotFound();
            }

            updatedBeers.Id = beers.Id;

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


        [HttpGet("recommendation-with-name")]
        public async Task<ActionResult<List<string>>> GetDrugRecommendationsWithName([FromQuery] string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return BadRequest("Please provide a comma-separated list of drug IDs.");
            }

            var idList = ids.Split(',');

            var recommendationsWithName = new List<string>();

            foreach (var id in idList)
            {
                var beers = await _beersService.GetAsync(id);

                if (beers != null)
                {
                    string recommendationWithName = $"{beers.Drug} - {beers.Recommendation}";
                    recommendationsWithName.Add(recommendationWithName);
                }
                else
                {
                    recommendationsWithName.Add($"No recommendation found for drug with ID: {id}");
                }
            }

            return recommendationsWithName;
        }


     

    }
}
