using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristP.Data;
using TouristP.Models;

namespace TouristP.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        public DashboardController(DashboardContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;

            _webHostEnvironment = webHostEnvironment;
        }
       

        public IActionResult Package()
        {
            var getData = _context.City.ToList();
            ViewBag.getData = getData;
            var Package = _context.Package.ToList();
            var GetPackage = _context.Package.Join(
                 _context.City,
                 Package => Package.CityId,
                 City => City.Id,

                (Package, City) => new
                {
                    Id = Package.Id,
                    Name = Package.Name,
                    Description = Package.Description,
                    Price = Package.Price,
                    Details = Package.Details,
                    ImagePath = Package.ImagePath,
                    CityName = City.Name,

                }).ToList();
            ViewBag.GetPackage = GetPackage;
            return View();



        }
        public IActionResult CreateNewPackage(Package Package, IFormFile Photo)
        {
            if (Photo == null || Photo.Length == 0)
            {
                return Content("No File Selected");
            }
            //wwwroot/images/p.png
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", Photo.FileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                Photo.CopyTo(stream);
                stream.Close();

            }

            Package.ImagePath = Photo.FileName;
            _context.Add(Package);
            _context.SaveChanges();
            return RedirectToAction("Package");
        }
        public IActionResult DeletePackage(int id)
        {
            var Package = _context.Package.SingleOrDefault(c => c.Id == id);

            if (Package != null)
            {
                _context.Package.Remove(Package);
                _context.SaveChanges();
            }


            var catogries = _context.Package.ToList();

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

            if (existingPackage == null)
            {
                return NotFound("الباقة غير موجودة");
            }

            
            existingPackage.Name = package.Name;
            existingPackage.Description = package.Description;
            existingPackage.Price = package.Price;
            existingPackage.CityId = package.CityId; 
           
            if (Photo != null && Photo.Length > 0)
            {
                
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Photo.CopyTo(stream);
                }

                
                existingPackage.ImagePath = "images/" + fileName;
            }

          
            _context.Package.Update(existingPackage);
            _context.SaveChanges();

            return RedirectToAction("Package");
        }


        public IActionResult City()
        {
            var getData = _context.City.ToList();

            return View(getData);

        }
        public IActionResult CreateNewCity(City City)
        {
            _context.Add(City);
            _context.SaveChanges();
            return RedirectToAction("City");
        }
        public IActionResult DeleteCity(int id)
        {
            var City = _context.City.SingleOrDefault(c => c.Id == id);

            if (City != null)
            {
                _context.City.Remove(City);
                _context.SaveChanges();
            }


            var catogries = _context.City.ToList();

            return RedirectToAction("City");


        }
        public IActionResult EditCity(int id)
        {
            var edit_City = _context.City.SingleOrDefault(e => e.Id == id);
            return View(edit_City);
        }

        public IActionResult UpdateCity(City City)
        {
            _context.City.Update(City);
            _context.SaveChanges();

            return RedirectToAction("City");

        }
    }
}
