using TDA_WebApplication.Models;

namespace TDA_WebApplication.Services
{
    public interface IBeersService
    {
        Task<IEnumerable<Beers>> Find();
    }
}