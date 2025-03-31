using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers;

public class UserController : Controller
{
    private readonly DbContext _context;
    
    public UserController(DbContext context)
    {
        _context = context;
    }
    
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
}