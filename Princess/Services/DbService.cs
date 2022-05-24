using Princess.Data;

namespace Princess.Services;

public class DbService
{
    private readonly PresenceDbContext _ctx;

    public DbService(PresenceDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task IsCreatedAsync()
    {
        await _ctx.Database.EnsureCreatedAsync();
    }

    public async Task RecreateAsync()
    {
        await _ctx.Database.EnsureDeletedAsync();
        await _ctx.Database.EnsureCreatedAsync();
    }
}