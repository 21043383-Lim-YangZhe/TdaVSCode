using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using TdaWebApp.Models;

namespace TdaWebApp.Services
{
    public class BeersService
    {
        private readonly IMongoCollection<Beers> beers;

        public BeersService(IConfiguration config)
        {
            MongoClient client = new(config.GetConnectionString("mydb"));
            IMongoDatabase database = client.GetDatabase("mydb");
            beers = database.GetCollection<Beers>("beers");
        }

        public List<Beers> Get()
        {
            return beers.Find(beer => true).ToList();
        }

        public Beers Get(string id)
        {
            return beers.Find(beer => beer.Id == id).FirstOrDefault();
        }

        //public Beers Create(Beers beer)
        //{
        //    beers.InsertOne(beer);
        //    return beer;
        //}

        public Beers Create(Beers beer)
        {
            try
            {
                beers.InsertOne(beer);
                return beer;
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    // Handle duplicate key error, e.g, log it or return an error message
                    // You can also customize the error message based on your needs
                    throw new DuplicateKeyException("A record with the same criteria already exists.", ex);
                }

                // If it's not a duplicate key error, rethrow the exception
                throw;
            }
        }

        public void Update(string id, Beers beersIn)
        {
            beers.ReplaceOne(beer => beer.Id == id, beersIn);
        }

        public void Remove(Beers beersIn)
        {
            beers.DeleteOne(beer => beer.Id == beersIn.Id);
        }

        public void Remove(string id)
        {
            beers.DeleteOne(beer => beer.Id == id);
        }


    }
}
