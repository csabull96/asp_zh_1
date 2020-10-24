using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheAdvertiser.Extensions;
using TheAdvertiser.Models;

namespace TheAdvertiser.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAdvertiserRepository advertiserRepository;

        public HomeController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IAdvertiserRepository advertiserRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.advertiserRepository = advertiserRepository;
        }

        [Authorize]
        public async Task<IActionResult> Init()
        {
            var first = userManager.Users.First();
            IdentityRole adminRole = new IdentityRole("Admin");
            if (await roleManager.FindByNameAsync(adminRole.Name) == null)
                await roleManager.CreateAsync(adminRole);
            if (!await userManager.IsInRoleAsync(first, adminRole.Name))
                await userManager.AddToRoleAsync(first, adminRole.Name);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            var ads = await advertiserRepository.GetAllAds();
            var user = await userManager.GetUserAsync(User);
            if (user != null && user.IsStudent)
                ads = ads.ApplyDiscount(10);

            return View(ads);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Advertisement ad)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                ad.UID = Guid.NewGuid().ToString();
                ad.Email = user.Email;
                ad.Created = DateTime.Now;
                await advertiserRepository.PutAd(ad);
            }

            return RedirectToAction(nameof(Index));
        }

        
        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
