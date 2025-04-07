using System.Diagnostics;
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

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("prihlasenyUzivatel") != "x")
        {
            ViewData["Username"] = HttpContext.Session.GetString("prihlasenyUzivatel");
        }
        else ViewData["Username"] = "x";
        
        return View();
    }

    [HttpPost]
    public IActionResult Login(string? Username, string? Password)
    {
        Username = Username.Trim();
        Password = Password.Trim();
        
        UserModel? prihlasovanyUzivatel = _context.Users.FirstOrDefault(u => u.Username == Username);

        Console.WriteLine("BYL JSEM ZDE FANTOMAS");
        if (prihlasovanyUzivatel == null) return RedirectToAction("Login");
        Console.WriteLine("BYL JSEM ZDE ZAS FANTOMAS");
        
        if (!BCrypt.Net.BCrypt.Verify(Password, prihlasovanyUzivatel.Password)) return RedirectToAction("Login");
        
        HttpContext.Session.SetString("prihlasenyUzivatel", prihlasovanyUzivatel.Username);
        
        Console.WriteLine($" {HttpContext.Session.GetString("prihlasenyUzivatel")} ");
        
        return RedirectToAction("Index", "Piskvorky");
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