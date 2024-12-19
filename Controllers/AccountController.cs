using Microsoft.AspNetCore.Mvc;
using gestion.Models;
using gestion.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace gestion.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.Name);
                var cartItems = _context.Cart.Where(c => c.User == user.Id).ToList();

                foreach (var cartItem in cartItems)
                {
                    var good = _context.Goods.FirstOrDefault(g => g.Id == cartItem.Good);
                    if (good != null)
                    {
                        good.Quantity = cartItem.Quantity; 
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.ErrorMessage = "Неверный email или пароль.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string email, string password, string confirmPassword, string name, string fam, string fat, string phone)
        {
            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Пароли не совпадают.";
                return View();
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Пользователь с таким email уже существует.";
                return View();
            }

            var user = new Users
            {
                Email = email,
                Password = password,
                Name = name,
                Fam = fam,
                Fat = fat,
                Phone = phone
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            var allGoods = _context.Goods.ToList(); 

            foreach (var item in allGoods)
            {
                item.Quantity = 0; 
            }

            _context.SaveChanges();
            HttpContext.Session.Clear(); 
            return RedirectToAction("Index", "Home"); 

        }
    }
}
