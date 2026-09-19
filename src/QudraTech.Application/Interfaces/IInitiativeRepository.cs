using QudraTech.Domain.Entities;

namespace QudraTech.Application.Interfaces;

public interface IInitiativeRepository : IRepository<Initiative>
{
    Task<Initiative?> GetBySlugAsync(string slug);
    Task<Initiative?> GetWithDetailsAsync(Guid id); // مع Objectives/Activities/Kpis
    Task<IReadOnlyList<Initiative>> GetPublicListAsync(); // للموقع العام فقط
}