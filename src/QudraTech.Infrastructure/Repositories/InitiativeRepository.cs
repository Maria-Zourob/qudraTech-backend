using Microsoft.EntityFrameworkCore;
using QudraTech.Application.Interfaces;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Persistence;

namespace QudraTech.Infrastructure.Repositories;

public class InitiativeRepository : IInitiativeRepository
{
    private readonly QudraTechDbContext _context;

    public InitiativeRepository(QudraTechDbContext context)
    {
        _context = context;
    }

    public async Task<Initiative?> GetByIdAsync(Guid id) =>
        await _context.Initiatives.FindAsync(id);

    public async Task<IReadOnlyList<Initiative>> GetAllAsync() =>
        await _context.Initiatives.ToListAsync();

    public async Task AddAsync(Initiative entity) =>
        await _context.Initiatives.AddAsync(entity);

    public void Update(Initiative entity) =>
        _context.Initiatives.Update(entity);

    public void Delete(Initiative entity) =>
        _context.Initiatives.Remove(entity);

    public async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task<Initiative?> GetBySlugAsync(string slug) =>
        await _context.Initiatives.FirstOrDefaultAsync(i => i.Slug == slug);

    public async Task<Initiative?> GetWithDetailsAsync(Guid id) =>
        await _context.Initiatives
            .Include(i => i.Objectives)
            .Include(i => i.Activities)
            .Include(i => i.Kpis)
            .FirstOrDefaultAsync(i => i.Id == id);

    public async Task<IReadOnlyList<Initiative>> GetPublicListAsync() =>
        await _context.Initiatives
            .Where(i => i.Status == Domain.Enums.InitiativeStatus.Active
                     || i.Status == Domain.Enums.InitiativeStatus.Completed)
            .ToListAsync();
}