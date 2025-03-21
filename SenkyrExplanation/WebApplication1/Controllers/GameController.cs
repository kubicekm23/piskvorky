using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class GameController : Controller
{
    private readonly PiskvorkyContext _context;

    public GameController(PiskvorkyContext context)
    {
        _context = context;
        
        if (!_context.PiskvorkyModel.Any())
        {
            Console.WriteLine("Piskvorky is empty");
            _context.PiskvorkyModel.Add(new Piskvorky());
            _context.SaveChanges();
        }
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Piskvorky(int id)
    {
        var piskvorky = _context.PiskvorkyModel.First();
        
        return View(piskvorky);
    }
}