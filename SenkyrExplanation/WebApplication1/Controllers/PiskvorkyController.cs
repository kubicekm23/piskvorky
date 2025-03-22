using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class PiskvorkyController : Controller
{
    private readonly PiskvorkyContext _context;

    public PiskvorkyController(PiskvorkyContext context)
    {
        _context = context;
        
        if (!_context.PiskvorkyModel.Any())
        {
            _context.PiskvorkyModel.Add(new Piskvorky());
            _context.SaveChanges();
        }
    }

    public IActionResult Index()
    {
        var Piskvorky = _context.PiskvorkyModel.ToList();
        
        return View(Piskvorky);
    }

    public IActionResult Zobrazit(int id)
    {
        Console.WriteLine(id);
        var piskvorky = _context.PiskvorkyModel.Find(id);
        
        return View(piskvorky);
    }

    public IActionResult Tahnout(int policko, int id)
    {
        Piskvorky hra = _context.PiskvorkyModel.Find(id);
        char aktivniHrac = hra.AktivniHrac;

        if (hra.HerniPole[policko] == '-')
        {
            char[] herniPole = hra.HerniPole.ToCharArray();
            herniPole[policko] = aktivniHrac;
            hra.HerniPole = new string(herniPole);
            hra.AktivniHrac = (aktivniHrac == 'X') ? 'O' : 'X';

            _context.Update(hra);
            _context.SaveChanges();

        }
        
        return RedirectToAction("Zobrazit", new { id = id });
    }

    public IActionResult Reset(int id)
    {
        var hra = _context.PiskvorkyModel.First();
        hra.HerniPole = "---------";
        
        hra.AktivniHrac = 'X';
        
        _context.Update(hra);
        _context.SaveChanges();
        
        return RedirectToAction("Zobrazit", new { id = id });
    }

    public IActionResult Vytvorit()
    {
        Piskvorky novaHra = new Piskvorky();
        _context.PiskvorkyModel.Add(novaHra);
        _context.SaveChanges();
        int id = novaHra.Id;
        
        return RedirectToAction("Zobrazit", new { id = id });
    }

    public IActionResult Vyhodnotit(int id)
    {
        Piskvorky hra = _context.PiskvorkyModel.Find(id);
        
        return RedirectToAction("Zobrazit", new { id = hra.Id });
    }
}