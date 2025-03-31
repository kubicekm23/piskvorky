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
        ViewBag.Piskvorky = Piskvorky;
        
        return View();
    }

    public IActionResult Zobrazit(int id)
    {
        Console.WriteLine(id);
        var piskvorky = _context.PiskvorkyModel.Find(id);
        
        return View(piskvorky);
    }
    
    public IActionResult Vytvorit()
    {
        Piskvorky novaHra = new Piskvorky();
        _context.PiskvorkyModel.Add(novaHra);
        _context.SaveChanges();
        int id = novaHra.Id;
        
        return RedirectToAction("Zobrazit", new { id = id });
    }

    public IActionResult Smazat(int id)
    {
        var piskvorky = _context.PiskvorkyModel.Find(id);
        _context.PiskvorkyModel.Remove(piskvorky);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Tahnout(int policko, int id)
    {
        Piskvorky hra = _context.PiskvorkyModel.Find(id);
        char aktivniHrac = hra.AktivniHrac;

        if (hra.HerniPole[policko] == '-' && hra.StavHry == "Hra probíhá")
        {
            char[] herniPole = hra.HerniPole.ToCharArray();
            herniPole[policko] = aktivniHrac;
            hra.HerniPole = new string(herniPole);

            Vyhodnotit(hra.Id);
            
            hra.AktivniHrac = (aktivniHrac == 'X') ? 'O' : 'X';
            
            _context.Update(hra);
            _context.SaveChanges();

        }
        
        return RedirectToAction("Zobrazit", new { id = id });
    }
    [HttpPost]
    public IActionResult Reset(int id)
    {
        var hra = _context.PiskvorkyModel.Find(id);
        hra.HerniPole = "---------";
        
        hra.AktivniHrac = 'X';
        hra.StavHry = "Hra probíhá";
        
        _context.Update(hra);
        _context.SaveChanges();
        
        return RedirectToAction("Zobrazit", new { id = id });
    }

    public IActionResult Vyhodnotit(int id)
    {
        Piskvorky hra = _context.PiskvorkyModel.Find(id);

        bool gameOver = KonecHryPiskvorek(hra);
        
        if (gameOver)
        {
            if (hra.AktivniHrac == 'X') hra.StavHry = "Vyhrál hráč X";
            else hra.StavHry = "Vyhrál hráč O";
        }
        else
        {
            if (KonecHryPiskvorekRemiza(hra))
            {
                hra.StavHry = "Remíza!";
            }
        }
        
        _context.Update(hra);
        _context.SaveChanges();
        
        return RedirectToAction("Zobrazit", new { id = hra.Id });
    }

    private bool KonecHryPiskvorek(Piskvorky hra)
    {
        char[] herniPole = hra.HerniPole.ToCharArray();
        
        // horizontalní
        if (herniPole[0] == herniPole[1] && herniPole[1] == herniPole[2] && herniPole[0] != '-') return true;
        if (herniPole[3] == herniPole[4] && herniPole[4] == herniPole[5] && herniPole[3] != '-') return true;
        if (herniPole[6] == herniPole[7] && herniPole[7] == herniPole[8] && herniPole[6] != '-') return true;
        
        // křížem
        if (herniPole[0] == herniPole[3] && herniPole[3] == herniPole[8] && herniPole[8] != '-') return true;
        if (herniPole[2] == herniPole[4] && herniPole[4] == herniPole[6]  && herniPole[6] != '-') return true;
        
        // vertikální
        if (herniPole[0] == herniPole[3] && herniPole[3] == herniPole[6]  && herniPole[6] != '-') return true;
        if (herniPole[1] == herniPole[4] && herniPole[4] == herniPole[7]  && herniPole[7] != '-') return true;
        if (herniPole[2] == herniPole[5] && herniPole[5] == herniPole[8]  && herniPole[8] != '-') return true;
        
        return false;
    }
    
    private bool KonecHryPiskvorekRemiza(Piskvorky hra)
    {
        char[] herniPole = hra.HerniPole.ToCharArray();

        if (!herniPole.Contains('-')) return true;
        
        return false;
    }
}