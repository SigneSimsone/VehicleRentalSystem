﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;


namespace VehicleRentalSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminDataManager _adminDataManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotyfService _notyfService;

        public AdminController(AdminDataManager adminDataManager, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager, INotyfService notyfService)
        {
            _adminDataManager = adminDataManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _notyfService = notyfService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            UserModel[] users = _adminDataManager.GetUsers();

            AdminViewModel viewModel = new AdminViewModel();
            viewModel.Users = users.Select(x => new UserViewModel()
            {
                Reservations = x.Reservations,
                Feedbacks = x.Feedbacks,
                UserName = x.UserName,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                Name = x.Name,
                Surname = x.Surname,
                Roles = _userManager.GetRolesAsync(x).Result
            }).ToArray();


            var userList = users.Select(x => x.Email).ToList();
            viewModel.UserDropdown = new SelectList(userList, "Email");

            var rolesList = _roleManager.Roles.Select(x => x.Name).ToList();
            viewModel.RoleDropdown = new SelectList(rolesList, "Name");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(AdminViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewRole))
            {
                return RedirectToAction(nameof(Index));
            }

            //Creating new role
            bool roleExists = await _roleManager.RoleExistsAsync(model.NewRole);
            if (!roleExists)
            {
                var role = new IdentityRole();
                role.Name = model.NewRole;
                await _roleManager.CreateAsync(role);
                _notyfService.Success("Role created successfully!");
            }
            else
            {//check if role already exists
                _notyfService.Error("This role already exists in the system!");
                return RedirectToAction(nameof(Index), model);
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> AddUserToRole(AdminViewModel model)
        {//select user and add him to a selected role
            if (string.IsNullOrWhiteSpace(model.SelectedRole) || string.IsNullOrWhiteSpace(model.SelectedUser))
            {
                return RedirectToAction(nameof(Index));
            }

            var user = _adminDataManager.GetOneUserByEmail(model.SelectedUser);

            bool roleExists = await _roleManager.RoleExistsAsync(model.SelectedRole);

            if (roleExists)
            {
                if (!await _userManager.IsInRoleAsync(user, model.SelectedRole))
                {
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    _notyfService.Success("Role assigned successfully!");
                }
                else
                {//check if user is not in the role
                    _notyfService.Error("This user already has this role!");
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
