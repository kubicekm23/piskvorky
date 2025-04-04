using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class PiskvorkyContext : DbContext
{
    public PiskvorkyContext(DbContextOptions<PiskvorkyContext> options) : base(options)
    {
    }
    
    public DbSet<Piskvorky> PiskvorkyModel { get; set; }
    public DbSet<UserModel> Users { get; set; }
}