using TDA_WebApplication.Models;

using TDA_WebApplication.Helpers;

namespace TDA_WebApplication.Services
{
    public class BeersService : IBeersService
    {
        private readonly HttpClient _client;
        public const string BasePath = "/api/find";

        public BeersService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<Beers>> Find()
        {
            var response = await _client.GetAsync(BasePath);

            return await response.ReadContentAsync<List<Beers>>();
        }
    }
}