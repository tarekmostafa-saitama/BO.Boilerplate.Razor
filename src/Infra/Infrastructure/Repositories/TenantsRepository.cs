using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class TenantsRepository : EfRepository<Tenant>, ITenantsRepository
{
    public TenantsRepository(ApplicationDbContext context) : base(context)
    {
    }
}