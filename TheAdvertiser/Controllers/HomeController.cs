using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using TheAdvertiser.Extensions;
using TheAdvertiser.Models;

namespace TheAdvertiser.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment hostEnv;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAdvertiserRepository advertiserRepository;

        public JsonResult JSON { get; private set; }

        public HomeController(IWebHostEnvironment hostEnv,
            UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IAdvertiserRepository advertiserRepository)
        {
            this.hostEnv = hostEnv;
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

        public async Task<IActionResult> GetPhoto(string uid)
        {
            var ad = await advertiserRepository.GetAd(uid);

            try
            {
                return new FileContentResult(ad.Photo, ad.ContentType);
            }
            catch (Exception)
            {
                string path = Path.Combine(hostEnv.WebRootPath, "no_photo.png");
                byte[] defaultPhoto = System.IO.File.ReadAllBytes(path);
                string contentType = "image/png";

                return new FileContentResult(defaultPhoto, contentType);
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Advertisement ad)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                ad.UID = Guid.NewGuid().ToString();
                
                using (var stream = ad.PhotoData.OpenReadStream())
                {
                    ad.ContentType = ad.PhotoData.ContentType;
                    ad.Photo = new byte[ad.PhotoData.Length];
                    stream.Read(ad.Photo, 0, ad.Photo.Length);
                }

                ad.Email = user.Email;
                ad.Created = DateTime.Now;
                await advertiserRepository.PutAd(ad);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string uid)
        {
            var ad = await advertiserRepository.GetAd(uid); 
            return View(ad);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Advertisement ad)
        {
            await advertiserRepository.Update(ad);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string uid)
        {
            await advertiserRepository.Delete(uid);
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
