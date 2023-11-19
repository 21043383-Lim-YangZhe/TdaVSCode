using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public Beers Create(Beers beer)
        {
            beers.InsertOne(beer);
            return beer;
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
