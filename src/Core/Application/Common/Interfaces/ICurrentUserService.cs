using Shared.ServiceContracts;

namespace Application.Common.Interfaces;

public interface ICurrentUserService : IScopedService
{
    string Id { get; }
}