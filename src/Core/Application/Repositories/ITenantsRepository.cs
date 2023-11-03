using Domain.Entities;
using Shared.ServiceContracts;

namespace Application.Repositories;

public interface ITenantsRepository : IRepository<Tenant>, IScopedService
{
}