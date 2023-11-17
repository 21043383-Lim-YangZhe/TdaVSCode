using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using TdaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using TdaWebApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace TdaWebApp.Controllers
{
    public class BeersController : Controller
    {

        private readonly BeersService beersService;

        public BeersController(BeersService beersService)
        {
            this.beersService = beersService;
        }

        /*// GET: BeersController
        public ActionResult Index()
        {
            return View(beersService.Get());
        }*/
        /* public ActionResult Index(string searchTerm)
         {
             IEnumerable<Beers> beers;

             if (!string.IsNullOrEmpty(searchTerm))
             {
                 // Filter records based on the search term
                 beers = beersService.Get().Where(b =>
                     b.DrugID?.Contains(searchTerm) == true ||
                     b.Drug?.Contains(searchTerm) == true ||
                     b.DrugClass?.Contains(searchTerm) == true ||
                     b.Crcl?.Contains(searchTerm) == true ||
                     b.Disease?.Contains(searchTerm) == true ||
                     b.Recommendation?.Contains(searchTerm) == true ||
                     b.Rationale?.Contains(searchTerm) == true ||
                     b.QualityEvidence?.Contains(searchTerm) == true ||
                     b.StrengthRecommendation?.Contains(searchTerm) == true ||
                     b.Condition?.Contains(searchTerm) == true ||
                     b.Age?.Contains(searchTerm) == true ||
                     b.InteractingDrugOrClass?.Contains(searchTerm) == true ||
                     b.Dosage?.Contains(searchTerm) == true
                 );
             }
             else
             {
                 // If no search term provided, get all records
                 beers = beersService.Get();
             }

             return View(beers);
         }*/
        [Authorize]
        public ActionResult Index(string searchTerm, string sortOrder, string sortBy)
        {
            IEnumerable<Beers> beers;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Filter records based on the search term
                beers = beersService.Get().Where(b =>
                     b.DrugID?.Contains(searchTerm) == true ||
                     b.Drug?.Contains(searchTerm) == true ||
                     b.DrugClass?.Contains(searchTerm) == true ||
                     b.Crcl?.Contains(searchTerm) == true ||
                     b.Disease?.Contains(searchTerm) == true ||
                     b.Recommendation?.Contains(searchTerm) == true ||
                     b.Rationale?.Contains(searchTerm) == true ||
                     b.QualityEvidence?.Contains(searchTerm) == true ||
                     b.StrengthRecommendation?.Contains(searchTerm) == true ||
                     b.Condition?.Contains(searchTerm) == true ||
                     b.Age?.Contains(searchTerm) == true ||
                     b.InteractingDrugOrClass?.Contains(searchTerm) == true ||
                     b.Dosage?.Contains(searchTerm) == true
                );

                // Apply sorting to the filtered data
                beers = ApplySorting(beers, sortOrder, sortBy);
            }
            else
            {
                // If no search term provided, get all records
                beers = beersService.Get();

                // Apply sorting to all records
                beers = ApplySorting(beers, sortOrder, sortBy);
            }

            // Pass sorting information to the view
            ViewData["SortOrder"] = sortOrder;
            ViewData["SortBy"] = sortBy;

            return View(beers);
        }

        private IEnumerable<Beers> ApplySorting(IEnumerable<Beers> data, string sortOrder, string sortBy)
        {
            switch (sortBy)
            {
                case "DrugID":
                    return sortOrder == "asc" ? data.OrderBy(b => b.DrugID) : data.OrderByDescending(b => b.DrugID);
                case "Drug":
                    return sortOrder == "asc" ? data.OrderBy(b => b.Drug) : data.OrderByDescending(b => b.Drug);
                case "DrugClass":
                    return sortOrder == "asc" ? data.OrderBy(b => b.DrugClass) : data.OrderByDescending(b => b.DrugClass);
                // Add cases for other columns if needed
                default:
                    // Default sorting (e.g., by DrugID ascending)
                    return data.OrderBy(b => b.DrugID);
            }
        }


        // GET: BeersController/Details/5
        public ActionResult Details(int id)
         {
             return View();
         }


        // GET: BeersController/Create
        public ActionResult Create()
        {
            ViewBag.BeersList = beersService.Get(); // Assuming Get() returns a list of Beers
            return View();
        }

        // POST: BeersController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Beers beers, string[] SelectedDrugs)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        beersService.Create(beers);
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewBag.BeersList = beersService.Get(); //
        //    return View(beers);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Beers beers, string[] SelectedDrugs)
        {
            if (ModelState.IsValid)
            {
                // Assuming you have a method to check for duplicate records
                if (IsDuplicateRecord(beers))
                {
                    ModelState.AddModelError("DuplicateRecord", "A record with the same criteria already exists.");
                }
                else
                {
                    // Assuming you have a method to retrieve a concatenated string of drug IDs
                    beers.DrugID = GetSelectedDrugIDs(SelectedDrugs);
                    beersService.Create(beers);
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.BeersList = beersService.Get();
            return View(beers);
        }

        // Helper method to concatenate selected drug IDs
        private string GetSelectedDrugIDs(string[] selectedDrugs)
        {
            // Concatenate selected drug IDs with a comma separator
            return string.Join(",", selectedDrugs);
        }

        // Helper method to check for duplicate records
        private bool IsDuplicateRecord(Beers beers)
        {
            // Implement logic to check for duplicate records based on criteria
            // For example, check if a record with the same criteria already exists in the database
            // You may need to modify this logic based on your data model and database structure

            var existingRecord = beersService.Get().FirstOrDefault(b =>
                b.DrugID == beers.DrugID
                // Add additional conditions for other criteria if needed
            );

            return existingRecord != null;
        }




        // GET: BeersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BeersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BeersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BeersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


    }
}
