using EndPointFinder.Repository.Interfaces;

namespace EndPointFinder.Repository.UnitOfWork;

public interface IUnitOfWork
{
    IEndpointFinder EndpointFinder { get; }

    IHelperMethods HelperMethods { get; }

    IApiFinder ApiFinder { get; }

    IMainMethods MainMethods { get; }
}
