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

        // رابط الصورة الافتراضية عند عدم رفع صورة
        private const string DefaultImage = "https://res.cloudinary.com/demo/image/upload/v123456/default.png";

        public DashboardController(DashboardContext context, CloudinaryService cloudinaryService)
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
            var cities = _context.City.ToList();
            ViewBag.getData = cities;

            var packages = _context.Package
                .Join(
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
                    }
                ).ToList();

            ViewBag.GetPackage = packages;
            return View();
        }

[HttpPost]
[IgnoreAntiforgeryToken]
public IActionResult CreateNewPackage(Package package, IFormFile Photo)
{
    if (Photo != null && Photo.Length > 0)
    {
        // اسم فريد للصورة
        var fileName = Guid.NewGuid() + Path.GetExtension(Photo.FileName);

        // المسار داخل wwwroot/uploads
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

        // لو المجلد مو موجود
        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }

        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            Photo.CopyTo(stream);
        }

        // المسار اللي ينحفظ بالـ DB
        package.ImagePath = "/uploads/" + fileName;
    }
    else
    {
        package.ImagePath = "/uploads/default.png"; // اختياري
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
        [HttpPost]
[IgnoreAntiforgeryToken]
public IActionResult UpdatePackage(Package package, IFormFile Photo)
{
    var existingPackage = _context.Package.FirstOrDefault(p => p.Id == package.Id);
    if (existingPackage == null) return NotFound();

    existingPackage.Name = package.Name;
    existingPackage.Description = package.Description;
    existingPackage.Price = package.Price;
    existingPackage.Details = package.Details;
    existingPackage.CityId = package.CityId;

    if (Photo != null && Photo.Length > 0)
    {
        var fileName = Guid.NewGuid() + Path.GetExtension(Photo.FileName);
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            Photo.CopyTo(stream);
        }

        existingPackage.ImagePath = "/uploads/" + fileName;
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
