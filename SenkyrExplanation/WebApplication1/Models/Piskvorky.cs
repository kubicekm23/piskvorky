using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Piskvorky
{
    [Key]
    public int Id { get; set; }

    public string HerniPole { get; set; } = "---------";

    public char AktivniHrac { get; set; } = 'X';

    public string StavHry { get; set; } = "Hra probíhá";
    
    public int? Hrac1Id { get; set; }
    public int? Hrac2Id { get; set; }
}