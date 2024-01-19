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

        //[HttpGet]
        //public async Task<List<Beers>> Get()
        //{
        //    //return await _beersService.GetAsync();
        //    List<Beers> result = new List<Beers>();
        //    result.Add(new Beers() {Id="655df74ea7e3f76d0a818073", DrugID = "b_t3_r8", Drug= "Chlorpromazine", 
        //        DrugClass= "Antipsychotics", Crcl= "", Disease= "Syncope", Recommendation= "Avoid",
        //        Rationale= "Increased risk of orthostatic hypotension",
        //        StrengthRecommendation ="Weak", QualityEvidence= "High"});;
        //    return result;
        //}



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


        [HttpGet("recommendation/{id:length(24)}")]
        public async Task<ActionResult<string>> GetDrugRecommendation(string id)
        {
            var beers = await _beersService.GetAsync(id);

            if (beers is null)
            {
                return NotFound();
            }

            // Assuming you have a property in your Beers class that contains the drug recommendation.
            string drugRecommendation = beers.Recommendation;

            return drugRecommendation;
        }


        [HttpGet("recommendation")]
        public async Task<ActionResult<List<string>>> GetDrugRecommendations([FromQuery] string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return BadRequest("Please provide a comma-separated list of drug IDs.");
            }

            var idList = ids.Split(',');

            var recommendations = new List<string>();

            foreach (var id in idList)
            {
                var beers = await _beersService.GetAsync(id);

                if (beers != null)
                {
                    recommendations.Add(beers.Recommendation);
                }
                else
                {
                    recommendations.Add($"No recommendation found for drug with ID: {id}");
                }
            }

            return recommendations;
        }



    }
}
