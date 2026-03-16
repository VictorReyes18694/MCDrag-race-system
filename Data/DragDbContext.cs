using Microsoft.EntityFrameworkCore;
using MCDrag.Data.Entities;

namespace MCDrag.Data
{
    public class DragDbContext : DbContext
    {
        public DragDbContext(DbContextOptions<DragDbContext> options) : base(options)
        {
        }
        
    public DbSet<RaceEntity> Races => Set<RaceEntity>();
        // Por ahora SIN tablas. Solo vamos a probar conexión.

    
    public DbSet<LaneResultEntity> LaneResults => Set<LaneResultEntity>(); 
    }

    
}