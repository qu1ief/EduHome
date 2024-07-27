using EduHome.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Services;

public class LayoutService : ILayoutService
{
    private readonly AppDbContext _context;

    public LayoutService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Dictionary<string, string>> GetSettingsAsync()
    {
        var settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);

        return settings;
    }
}


public interface ILayoutService
{
    Task<Dictionary<string, string>> GetSettingsAsync();
}
