using System.Net;
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
        if (HttpContext.Session.GetString("prihlasenyUzivatel") == "x")
        {
            ViewData["Username"] = HttpContext.Session.GetString("prihlasenyUzivatel");
        }
        else ViewData["Username"] = "x";
        
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Username, string Password)
    {
        Username = Username.Trim();
        Password = Password.Trim();

        UserModel? prihlasovanyUzivatel = _context.Users.Where(u => u.Username == Username).FirstOrDefault();
        
        if (prihlasovanyUzivatel == null) return RedirectToAction("Login");
        
        if (BCrypt.Net.BCrypt.Verify(Password, prihlasovanyUzivatel.Password)) return RedirectToAction("Login");
        
        HttpContext.Session.SetString("prihlasenyUzivatel", prihlasovanyUzivatel.Username);
        
        return RedirectToAction("Zobrazit", "Piskvorky");
    }
    
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string? username, string? password, string? passwordConfirm)
    {
        if (username == null || username.Trim().Length < 1) return RedirectToAction("Register", "User");
        if (password == null || password.Trim().Length <= 1) return RedirectToAction("Register", "User");
        if (passwordConfirm == null || passwordConfirm.Trim().Length <= 1) return RedirectToAction("Register", "User");
        
        if (passwordConfirm.Trim() != password.Trim()) return RedirectToAction("Register", "User");
        if (_context.Users.Any(u => u.Username == username)) return RedirectToAction("Register", "User");
        
        username = username.Trim();
        password = password.Trim();
        
        UserModel registrovanyUzivatel = new UserModel()
        {
            Username = username, 
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };
        _context.Users.Add(registrovanyUzivatel);
        _context.SaveChanges();
        
        return RedirectToAction("Login");
    }
}