using TDA.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;

namespace TDA.Services
{
    public class BeersService : ControllerBase

    {
        private readonly IMongoCollection<Beers> _beersCollection;

        public BeersService(
            IOptions<BeersDBSettings> beersDBSettings)
        {

            var mongoClient = new MongoClient(
                beersDBSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                beersDBSettings.Value.DatabaseName);

            _beersCollection = mongoDatabase.GetCollection<Beers>(
                beersDBSettings.Value.BeersCollectionName);
        }
        public async Task<List<Beers>> GetAsync() =>
        await _beersCollection.Find(_ => true).ToListAsync();

        public async Task<Beers?> GetAsync(string id) =>
            await _beersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Beers newCombi) =>
            await _beersCollection.InsertOneAsync(newCombi);

        public async Task UpdateAsync(string id, Beers updatedCombi) =>
            await _beersCollection.ReplaceOneAsync(x => x.Id == id, updatedCombi);

        public async Task RemoveAsync(string id) =>
            await _beersCollection.DeleteOneAsync(x => x.Id == id);

    }
}
