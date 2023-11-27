using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
using TdaWebApp.Models;
using Newtonsoft.Json;

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


        // In BeersService.cs
        //public (int RecordsUpdated, int RecordsInserted) InsertFromJson(string jsonContent)
        //{
        //    var beersList = JsonConvert.DeserializeObject<List<Beers>>(jsonContent);

        //    int recordsUpdated = 0;
        //    int recordsInserted = 0;

        //    foreach (var beer in beersList)
        //    {
        //        // Check if the Id is a valid ObjectId; if not, generate a new one
        //        if (!ObjectId.TryParse(beer.Id, out _))
        //        {
        //            beer.Id = ObjectId.GenerateNewId().ToString();
        //        }

        //        // Check if a record with the same DrugID already exists
        //        var existingRecord = beers.Find(beerInDb => beerInDb.DrugID == beer.DrugID).FirstOrDefault();

        //        if (existingRecord != null)
        //        {
        //            // If a record with the same DrugID exists, update it
        //            beer.Id = existingRecord.Id; // Ensure the correct ID is set
        //            beers.ReplaceOne(beerInDb => beerInDb.Id == existingRecord.Id, beer);
        //            recordsUpdated++;
        //        }
        //        else
        //        {
        //            // If no record with the same DrugID exists, insert a new record
        //            beers.InsertOne(beer);
        //            recordsInserted++;
        //        }
        //    }

        //    return (recordsUpdated, recordsInserted);
        //}



        // In BeersService.cs
        public (int RecordsUpdated, int RecordsInserted, int RecordsSkipped) InsertFromJson(string jsonContent)
        {
            var beersList = JsonConvert.DeserializeObject<List<Beers>>(jsonContent);

            int recordsUpdated = 0;
            int recordsInserted = 0;
            int recordsSkipped = 0;

            foreach (var beer in beersList)
            {
                // Check if the DrugID is empty or contains only whitespace characters
                if (string.IsNullOrWhiteSpace(beer.DrugID))
                {
                    recordsSkipped++;
                    continue;
                }

                // Check if the Id is a valid ObjectId; if not, generate a new one
                if (!ObjectId.TryParse(beer.Id, out _))
                {
                    beer.Id = ObjectId.GenerateNewId().ToString();
                }

                // Check if a record with the same DrugID already exists
                var existingRecord = beers.Find(beerInDb => beerInDb.DrugID == beer.DrugID).FirstOrDefault();

                if (existingRecord != null)
                {
                    // If a record with the same DrugID exists, update it
                    beer.Id = existingRecord.Id; // Ensure the correct ID is set
                    beers.ReplaceOne(beerInDb => beerInDb.Id == existingRecord.Id, beer);
                    recordsUpdated++;
                }
                else
                {
                    // If no record with the same DrugID exists, insert a new record
                    beers.InsertOne(beer);
                    recordsInserted++;
                }
            }

            return (recordsUpdated, recordsInserted, recordsSkipped);
        }



    }
}
