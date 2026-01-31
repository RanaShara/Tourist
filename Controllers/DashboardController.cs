using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristP.Data;
using TouristP.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;

namespace TouristP.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardContext _context;
        private readonly Cloudinary _cloudinary;

        public DashboardController(DashboardContext context, IConfiguration config)
        {
            _context = context;

            // إعداد Cloudinary باستخدام Environment Variables
            var cloudName = config["Cloudinary:CloudName"];
            var apiKey = config["Cloudinary:ApiKey"];
            var apiSecret = config["Cloudinary:ApiSecret"];
            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Package()
        {
            var getData = _context.City.ToList();
            ViewBag.getData = getData;

            var GetPackage = _context.Package.Join(
                _context.City,
                p => p.CityId,
                c => c.Id,
                (p, c) => new
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Details = p.Details,
                    ImagePath = p.ImagePath,
                    CityName = c.Name
                }).ToList();

            ViewBag.GetPackage = GetPackage;
            return View();
        }

        public IActionResult CreateNewPackage(Package package, IFormFile Photo)
        {
            if (Photo != null && Photo.Length > 0)
            {
                // رفع الصورة إلى Cloudinary
                using var stream = Photo.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(Photo.FileName, stream)
                };
                var result = _cloudinary.Upload(uploadParams);

                // حفظ رابط الصورة في قاعدة البيانات
                package.ImagePath = result.SecureUrl.ToString();
            }

            _context.Add(package);
            _context.SaveChanges();

            return RedirectToAction("Package");
        }

        public IActionResult DeletePackage(int id)
        {
            var package = _context.Package.SingleOrDefault(c => c.Id == id);
            if (package != null)
            {
                _context.Package.Remove(package);
                _context.SaveChanges();
            }
            return RedirectToAction("Package");
        }

        public IActionResult EditPackage(int id)
        {
            var getData = _context.City.ToList();
            ViewBag.getData = getData;

            var edit_Package = _context.Package.SingleOrDefault(e => e.Id == id);
            return View(edit_Package);
        }

        public IActionResult UpdatePackage(Package package, IFormFile Photo)
        {
            var existingPackage = _context.Package.SingleOrDefault(p => p.Id == package.Id);
            if (existingPackage == null) return NotFound("الباقة غير موجودة");

            existingPackage.Name = package.Name;
            existingPackage.Description = package.Description;
            existingPackage.Price = package.Price;
            existingPackage.CityId = package.CityId;

            if (Photo != null && Photo.Length > 0)
            {
                // رفع الصورة الجديدة إلى Cloudinary
                using var stream = Photo.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(Photo.FileName, stream)
                };
                var result = _cloudinary.Upload(uploadParams);
                existingPackage.ImagePath = result.SecureUrl.ToString();
            }

            _context.Package.Update(existingPackage);
            _context.SaveChanges();

            return RedirectToAction("Package");
        }

        // === City Methods ===
        public IActionResult City()
        {
            var getData = _context.City.ToList();
            return View(getData);
        }

        public IActionResult CreateNewCity(City city)
        {
            _context.Add(city);
            _context.SaveChanges();
            return RedirectToAction("City");
        }

        public IActionResult DeleteCity(int id)
        {
            var city = _context.City.SingleOrDefault(c => c.Id == id);
            if (city != null)
            {
                _context.City.Remove(city);
                _context.SaveChanges();
            }
            return RedirectToAction("City");
        }

        public IActionResult EditCity(int id)
        {
            var edit_City = _context.City.SingleOrDefault(e => e.Id == id);
            return View(edit_City);
        }

        public IActionResult UpdateCity(City city)
        {
            _context.City.Update(city);
            _context.SaveChanges();
            return RedirectToAction("City");
        }
    }
}
