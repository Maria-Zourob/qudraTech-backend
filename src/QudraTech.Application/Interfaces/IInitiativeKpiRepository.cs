using QudraTech.Domain.Entities;

namespace QudraTech.Application.Interfaces;

public interface IInitiativeKpiRepository : IRepository<InitiativeKpi>
{
    Task<IReadOnlyList<InitiativeKpi>> GetByInitiativeIdAsync(Guid initiativeId);
}