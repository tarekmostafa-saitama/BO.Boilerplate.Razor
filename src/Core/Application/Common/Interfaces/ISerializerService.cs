using Shared.ServiceContracts;

namespace Application.Common.Interfaces;

public interface ISerializerService : ISingletonService
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}