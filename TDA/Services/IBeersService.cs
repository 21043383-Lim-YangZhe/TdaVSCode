using TDA.Models;

namespace TDA.Services
{
    public interface IBeersService
    {
        public Task<List<Beers>> GetAsync();
        public Task<Beers> GetAsync(string id);
        public Task CreateAsync(Beers newCombi);
        public Task UpdateAsync(string id, Beers updatedCombi);
        public Task RemoveAsync(string id);
    }
}
