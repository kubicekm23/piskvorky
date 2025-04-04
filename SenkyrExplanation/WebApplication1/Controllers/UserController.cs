using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class UserController : Controller
{
    private readonly PiskvorkyContext _context;
    
    public UserController(PiskvorkyContext context)
    {
        _context = context;
    }
    
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Username, string Password)
    {
        Username = Username.Trim();
        Password = Password.Trim();

        UserModel prihlasovanyUzivatel = _context.Users.Where(u => u.Username == Username).FirstOrDefault();
        
        if (Password != prihlasovanyUzivatel.Password) return RedirectToAction("Login", "Home");
        
        HttpContext.Session.SetString("prihlasenyUzivatel", prihlasovanyUzivatel.Username);
        
        return RedirectToAction("Zobrazit", "Piskvorky");
    }
    
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string Username, string Password, string PasswordConfirm)
    {
        if (Username == null || Username.Trim().Length < 3) return RedirectToAction("Register", "Account");
        if (Password == null || Username.Trim().Length < 5) return RedirectToAction("Register", "Account");
        if (PasswordConfirm.Trim() != Password.Trim()) return RedirectToAction("Register", "Account");
        if (_context.Users.Where(u => u.Username == Username) == null) return RedirectToAction("Register", "Account");
        
        Username = Username.Trim();
        Password = Password.Trim();
        
        UserModel registrovanyUzivatel = new UserModel()
        {
            Username = Username, 
            Password = BCrypt.Net.BCrypt.HashPassword(Password)
        };
        _context.Add(registrovanyUzivatel);
        _context.SaveChanges();
        
        return RedirectToAction("Login");
    }
}