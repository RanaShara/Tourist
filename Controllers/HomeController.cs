 using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TouristP.Data;
using TouristP.Models;

namespace TouristP.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

   
        }

        public IActionResult Index()
        {
            // ��� ���� �������
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
                    CityName = City.Name
                }).ToList();

            // ��� ����� �����
            var Cities = _context.City.Select(c => new { c.Id, c.Name }).ToList();

            // ����� �������� ��� ��� View
            ViewBag.GetPackage = GetPackage;
            ViewBag.Cities = Cities;

            return View();
        }


        public IActionResult Details(int Id)
        {
            
            var packageDetails = _context.Package.Join(
                _context.City,
                package => package.CityId,
                city => city.Id,
                (package, city) => new
                {
                    Id = package.Id,
                    Name = package.Name,
                    Description = package.Description,
                    Price = package.Price,
                    Details = package.Details,
                    ImagePath = package.ImagePath,
                    CityName = city.Name,
                }
            ).FirstOrDefault(p => p.Id == Id); 

            if (packageDetails == null)
            {
                return NotFound(); 
            }

            return View(packageDetails);
        }

        public IActionResult Payment(int id)
        {
            var package = _context.Package.Join(
                _context.City,
                Package => Package.CityId,
                City => City.Id,
                (Package, City) => new
                {
                    Id = Package.Id,
                    Name = Package.Name,
                    Price = Package.Price,
                    CityName = City.Name,
                }).FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return NotFound();
            }

            ViewBag.Package = package;
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmPayment(int packageId, string cardNumber, string expiryDate, string cvv)
        {
            var package = _context.Package.FirstOrDefault(p => p.Id == packageId);

            if (package == null)
            {
                TempData["ErrorMessage"] = "��� ��� ����� ������ �����. ������ ��� ������.";
                return RedirectToAction("Index");
            }

            
            TempData["SuccessMessage"] = $"�� ����� ����� ������: {package.Name}. �����: {package.Price} ����.";
            return RedirectToAction("Index"); 
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
