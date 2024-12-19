using gestion.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using gestion.Data;

namespace gestion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Us()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Review()
        {
            var reviews = _context.Review.ToList(); 
            return View(reviews);
        }
        [HttpPost]
        public IActionResult AddReview(Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Review.Add(review);
                _context.SaveChanges();
                return RedirectToAction("Index"); 
            }

            return View("Index", _context.Review.ToList());
        }

        public IActionResult regfirm()
        {
            var goods = _context.Goods.ToList(); 
            return View(goods);
        }
        public IActionResult likfirm()
        {
            var goods = _context.Goods.ToList(); 
            return View(goods);
        }
        public IActionResult Go()
        {
            return View();
        }
        public IActionResult bug()
        {
            var goods = _context.Goods.ToList(); 
            return View(goods);
        }

        public IActionResult Cart()
        {
            var goods = _context.Goods.ToList(); 
            return View(goods);
        }
        public IActionResult Exit()
        {
            Aut.In = 0;
            return RedirectToAction("Index");
        }

        public IActionResult advoc()
        {
            var goods = _context.Goods.ToList(); 
            return View(goods);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
      
       
        [HttpPost]
        public IActionResult Add(int id)
        {
            var item = _context.Goods.FirstOrDefault(g => g.Id == id);

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                TempData["ErrorMessage"] = "Пожалуйста, войдите в систему, чтобы добавить товар в корзину.";
                return RedirectToAction("Login", "Account");
            }

            if (item != null)
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                item.Quantity += 1;
                var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

                if (user != null)
                {
                    var existingCartItem = _context.Cart
                        .FirstOrDefault(c => c.User == user.Id && c.Good == item.Id);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity += 1;
                    }
                    else
                    {
                       
                        var cartItem = new Cart
                        {
                            User = user.Id, 
                            Good = item.Id,   
                            Quantity = 1      
                        };

                        _context.Cart.Add(cartItem);
                    }

                    _context.SaveChanges();
                }
            }

            switch (item.Type)
            {
                case "Регистрация":
                    return RedirectToAction("regfirm", "Home");
                case "Ликвидация":
                    return RedirectToAction("likfirm", "Home");
                case "Бух":
                    return RedirectToAction("bug", "Home");
                case "Адвокат":
                    return RedirectToAction("advoc", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public IActionResult Remove(int id)
        {
            
            var item = _context.Goods.FirstOrDefault(g => g.Id == id);

            if (item != null && item.Quantity > 0)
            {
                
                item.Quantity -= 1;

                _context.SaveChanges();
            }
                //var type = _context.Goods.FirstOrDefault(g => g.);
                switch (item.Type)
            {
                case "Регистрация":
                    return RedirectToAction("regfirm", "Home");
                case "Ликвидация":
                    return RedirectToAction("likfirm", "Home");
                case "Бух":
                    return RedirectToAction("bug", "Home");
                case "Адвокат":
                    return RedirectToAction("advoc", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public IActionResult RemoveCart(int id)
        {

            var item = _context.Goods.FirstOrDefault(g => g.Id == id);

            if (item != null && item.Quantity > 0)
            {

                item.Quantity -= 1;

                _context.SaveChanges();
            }
            return RedirectToAction("Cart", "Home");

        }
        public IActionResult Details(int id)
        {
            
            var product = _context.Goods.FirstOrDefault(g => g.Id == id);

            if (product == null)
            {
                return NotFound(); 
            }

            return View(product); 
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            
            if (string.IsNullOrWhiteSpace(query))
            {
                return View("~/Views/Home/search.cshtml", Enumerable.Empty<Goods>());
            }

           
            var results = _context.Goods
                .Where(g => g.Name.Contains(query) || g.Short.Contains(query) || g.Long.Contains(query))
                .ToList();

            return View("~/Views/Home/search.cshtml", results);
        }

    }
}
