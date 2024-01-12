
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data;
using TdaWebApp.Models;

namespace TdaWebApp.Controllers
{
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;
        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        //[Authorize(Roles = "SuperAdmin")]
        public ViewResult Create() => View();

        //[HttpPost]
        //public async Task<IActionResult> Create(User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser appUser = new ApplicationUser
        //        {
        //            UserName = user.Name,
        //            Email = user.Email
        //        };

        //        IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

        //        // Add Role
        //        await userManager.AddToRoleAsync(appUser, "Admin");

        //        if (result.Succeeded)
        //            ViewBag.Message = "User Created Successfully";
        //        else
        //        {
        //            foreach (IdentityError error in result.Errors)
        //                ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View(user);
        //}

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                var existingUser = await userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email address is already in use.");
                    return View(user);
                }

                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = user.Name,
                    Email = user.Email
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                {
                    // Retrieve the user with security stamp from the database
                    appUser = await userManager.FindByEmailAsync(user.Email);

                    // Add Role
                    await userManager.AddToRoleAsync(appUser, "SuperAdmin");

                    ViewBag.Message = "User Created Successfully";
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            return View(user);
        }


        [Authorize(Roles = "SuperAdmin")]
        public IActionResult CreateRole() => View();

        [HttpPost]
        public async Task<IActionResult> CreateRole(UserRole userRole)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new ApplicationRole() { Name = userRole.RoleName });
                if (result.Succeeded)
                    ViewBag.Message = "Role Created Successfully";
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }


        //
        //
        public IActionResult ManageUsers()
        {
            var users = userManager.Users.ToList();
            var usersWithRoles = new List<dynamic>();

            foreach (var user in users)
            {
                var roles = userManager.GetRolesAsync(user).Result;

                usersWithRoles.Add(new
                {
                    User = user,
                    Roles = roles
                });
            }

            return View(usersWithRoles);
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await userManager.GetRolesAsync(user);

            var model = new EditUserRolesViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                Roles = roles.ToList(),
                AllRoles = roleManager.Roles.Select(r => r.Name).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> EditUserRoles(EditUserRolesViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var selectedRoles = model.SelectedRoles ?? new List<string>();

            var rolesToRemove = userRoles.Except(selectedRoles);
            var rolesToAdd = selectedRoles.Except(userRoles);

            await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await userManager.AddToRolesAsync(user, rolesToAdd);

            TempData["SuccessMessage"] = "User roles updated successfully.";

            return RedirectToAction("ManageUsers");
        }


        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                // Optionally, you can add a success message to ViewBag
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            else
            {
                // Handle errors and add them to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("ManageUsers");
        }


    }

}

