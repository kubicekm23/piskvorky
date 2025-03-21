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
        return View();
    }

    public IActionResult Zobrazit(int id)
    {
        var piskvorky = _context.PiskvorkyModel.First();
        
        return View(piskvorky);
    }

    public IActionResult Tahnout(int policko)
    {
        Piskvorky hra = _context.PiskvorkyModel.First();
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
        return RedirectToAction("Zobrazit");
    }

    public IActionResult Reset(int id)
    {
        var hra = _context.PiskvorkyModel.First();
        hra.HerniPole = "---------";
        
        hra.AktivniHrac = 'X';
        
        _context.Update(hra);
        _context.SaveChanges();
        
        return RedirectToAction("Zobrazit", id);
    }
}