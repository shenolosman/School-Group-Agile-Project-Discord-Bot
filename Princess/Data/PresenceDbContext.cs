using Microsoft.EntityFrameworkCore;

namespace Princess.Data
{
    public class PresenceDbContext : DbContext
    {
        public PresenceDbContext(DbContextOptions<PresenceDbContext> options) : base(options)
        {

        }
    }
}
