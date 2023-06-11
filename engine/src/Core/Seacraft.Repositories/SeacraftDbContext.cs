using Microsoft.EntityFrameworkCore;

namespace Seacraft.Repositories
{
    public class SeacraftDbContext
        : DbContext
    {
        public SeacraftDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}