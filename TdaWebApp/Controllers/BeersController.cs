﻿using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using TdaWebApp.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using TdaWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace TdaWebApp.Controllers
{
    public class BeersController : Controller
    {

        private readonly BeersService beersService;

        public BeersController(BeersService beersService)
        {
            this.beersService = beersService;
        }


        [Authorize]
        public ActionResult Index(string searchTerm, string sortOrder, string sortBy, string tableFilter)
        {
            IEnumerable<Beers> beers;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search string to lowercase
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

            // Apply additional filtering based on the table filter
            if (!string.IsNullOrEmpty(tableFilter))
            {
                if (tableFilter == "Combination")
                {
                    // Filter records with multiple DrugIDs
                    beers = beers.Where(b => b.DrugID.Contains(","));
                }
                else
                {
                    // Filter records based on the specified table number
                    string tablePrefix = "b_t" + tableFilter + "_";
                    beers = beers.Where(b => b.DrugID.StartsWith(tablePrefix));
                }
            }

            // Apply sorting to the filtered data
            beers = ApplySorting(beers, sortOrder, sortBy);

            // Pass sorting and filtering information to the view
            ViewData["SortOrder"] = sortOrder;
            ViewData["SortBy"] = sortBy;
            ViewData["TableFilter"] = tableFilter;

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
        public ActionResult Details(string id)
         {
            if (id == null)
            {
                return NotFound();
            }

            var beer = beersService.Get(id);
            if (beer == null)
            {
                return NotFound();
            }
            return View(beer);
        }


        // GET: BeersController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.BeersList = beersService.Get(); // Assuming Get() returns a list of Beers
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Beers beers, string[] SelectedDrugs)
        {
            try
            {
                // Validate selected drug IDs
                if (SelectedDrugs == null || SelectedDrugs.Length < 2)
                {
                    ModelState.AddModelError(string.Empty, "Please select at least two drugs.");
                    ViewBag.BeersList = beersService.Get();
                    return View(beers);
                }

                // Check for duplicate record
                if (IsDuplicateRecord(beers))
                {
                    ModelState.AddModelError(string.Empty, "A record with the same criteria already exists. Please review your entries.");
                    ViewBag.BeersList = beersService.Get();
                    return View(beers);
                }

                if (ModelState.IsValid)
                {
                    // Assuming you have a method to retrieve a concatenated string of drug IDs
                    beers.DrugID = GetSelectedDrugIDs(SelectedDrugs);

                    // Combine selected drug names into the "Drug" field
                    beers.Drug = string.Join(",", SelectedDrugs.Select(drugId => GetDrugNameById(drugId)));

                    beersService.Create(beers);

                    // Set a success message in TempData
                    TempData["SuccessMessage"] = "Record created successfully.";

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.BeersList = beersService.Get();
                return View(beers);
            }
            catch (MongoWriteException ex)
            {
                // Handle MongoDB duplicate key error
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    ModelState.AddModelError(string.Empty, "A record with the same criteria already exists. Please review your entries.");
                    ViewBag.BeersList = beersService.Get();
                    return View(beers);
                }

                // Handle other MongoDB write errors if needed
                throw; // Re-throw the exception for other unexpected errors
            }
        }

        // Helper method to get drug name by ID
        private string GetDrugNameById(string drugId)
        {
            // Fetch the drug name from your data source using the drugId
            var drug = beersService.Get().FirstOrDefault(b => b.DrugID == drugId);
            return drug?.Drug ?? string.Empty;
        }




        //Helper method to concatenate selected drug IDs
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

            // Get the existing records with the same set of DrugIDs
            var existingRecordsWithSameDrugIDs = beersService.Get().Where(b => b.DrugID == beers.DrugID && b.Id != beers.Id);

            // If there are any existing records with the same set of DrugIDs, it means there is a duplicate record
            return existingRecordsWithSameDrugIDs.Any();
        }



        // GET: BeersController/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beer = beersService.Get(id);
            if (beer == null)
            {
                return NotFound();
            }

            ViewBag.BeersList = beersService.Get(); // Make sure to set ViewBag.BeersList
            return View(beer);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Beers updatedBeer, string[] SelectedDrugs)
        {
            try
            {
                if (id != updatedBeer.Id)
                {
                    return NotFound();
                }

                // Validate selected drug IDs
                if (SelectedDrugs == null || SelectedDrugs.Length < 2)
                {
                    ModelState.AddModelError(string.Empty, "Please select at least two drugs.");
                    ViewBag.BeersList = beersService.Get();
                    return View(updatedBeer);
                }

                // Check for duplicate record
                if (IsDuplicateRecord(updatedBeer))
                {
                    ModelState.AddModelError(string.Empty, "A record with the same criteria already exists. Please review your entries.");
                    ViewBag.BeersList = beersService.Get();
                    return View(updatedBeer);
                }

                if (ModelState.IsValid)
                {
                    // Assuming you have a method to retrieve a concatenated string of drug IDs
                    updatedBeer.DrugID = GetSelectedDrugIDs(SelectedDrugs);

                    // Combine selected drug names into the "Drug" field
                    updatedBeer.Drug = string.Join(",", SelectedDrugs.Select(drugId => GetDrugNameById(drugId)));

                    // Use the Update method to update the existing record
                    beersService.Update(id, updatedBeer);

                    // Set a success message in TempData
                    TempData["SuccessMessage"] = "Record updated successfully.";

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.BeersList = beersService.Get();
                return View(updatedBeer);
            }
            catch (MongoWriteException ex)
            {
                // Handle MongoDB duplicate key error
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    ModelState.AddModelError(string.Empty, "A record with the same criteria already exists. Please review your entries.");
                    ViewBag.BeersList = beersService.Get();
                    return View(updatedBeer);
                }

                // Handle other MongoDB write errors if needed
                throw; // Re-throw the exception for other unexpected errors
            }
        }



        // GET: BeersController/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beer = beersService.Get(id);
            if (beer == null)
            {
                return NotFound();
            }
            return View(beer);
        }


        // POST: BeersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                var beer = beersService.Get(id);

                if (beer == null)
                {
                    return NotFound();
                }

                beersService.Remove(beer.Id);

                // Store success message in TempData
                TempData["SuccessMessage"] = "Record deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [Authorize]
        public ActionResult UploadJSON()
        {
            return View();
        }



        // In BeersController.cs
        [HttpPost]
        public ActionResult UploadJSON(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    // Check if the uploaded file has a .json extension
                    if (!file.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        ViewBag.Message = "Please upload a valid JSON file.";
                        return View("UploadJSON");
                    }

                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        var jsonContent = reader.ReadToEnd();
                        var result = beersService.InsertFromJson(jsonContent);

                        ViewBag.Message = $"File uploaded successfully. {result.RecordsUpdated} records updated, {result.RecordsInserted} records inserted, {result.RecordsSkipped} records skipped due to the DrugID being empty.";
                    }
                }
                else
                {
                    ViewBag.Message = "Please select a file.";
                }

                return View("UploadJSON");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                return View("UploadJSON");
            }
        }





    }
}
