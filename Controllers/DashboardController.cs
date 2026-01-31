using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristP.Data;
using TouristP.Models;
using TouristP.Services;

namespace TouristP.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public DashboardController(
            DashboardContext context,
            CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // ================= Dashboard =================
        public IActionResult Index()
        {
            return View();
        }

        // ================= Packages =================
        public IActionResult Package()
        {
            ViewBag.getData = _context.City.ToList();

            var packages = _context.Package
                .Include(p => p.City)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Price,
                    p.Details,
                    p.ImagePath,
                    CityName = p.City.Name
                })
                .ToList();

            ViewBag.GetPackage = packages;
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult CreateNewPackage(Package package, IFormFile Photo)
        {
            if (Photo != null && Photo.Length > 0)
            {
                package.ImagePath = _cloudinaryService.UploadImage(Photo);
            }

            _context.Package.Add(package);
            _context.SaveChanges();
            return RedirectToAction("Package");
        }

        public IActionResult DeletePackage(int id)
        {
            var package = _context.Package.SingleOrDefault(p => p.Id == id);
            if (package != null)
            {
                _context.Package.Remove(package);
                _context.SaveChanges();
            }
            return RedirectToAction("Package");
        }

        public IActionResult EditPackage(int id)
        {
            ViewBag.getData = _context.City.ToList();
            var package = _context.Package.SingleOrDefault(p => p.Id == id);
            return View(package);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult UpdatePackage(Package package, IFormFile Photo)
        {
            var existingPackage = _context.Package.SingleOrDefault(p => p.Id == package.Id);
            if (existingPackage == null) return NotFound();

            existingPackage.Name = package.Name;
            existingPackage.Description = package.Description;
            existingPackage.Price = package.Price;
            existingPackage.Details = package.Details;
            existingPackage.CityId = package.CityId;

            if (Photo != null && Photo.Length > 0)
            {
                existingPackage.ImagePath = _cloudinaryService.UploadImage(Photo);
            }

            _context.SaveChanges();
            return RedirectToAction("Package");
        }

        // ================= Cities =================
        public IActionResult City()
        {
            return View(_context.City.ToList());
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult CreateNewCity(City city)
        {
            _context.City.Add(city);
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
            var city = _context.City.SingleOrDefault(c => c.Id == id);
            return View(city);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult UpdateCity(City city)
        {
            _context.City.Update(city);
            _context.SaveChanges();
            return RedirectToAction("City");
        }
    }
}
