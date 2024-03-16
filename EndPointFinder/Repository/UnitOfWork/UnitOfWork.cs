using EndPointFinder.Repository.Implementation;
using EndPointFinder.Repository.Interfaces;

namespace EndPointFinder.Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private IEndpointFinder _endpointFinder;
    private IHelperMethods _helperMethods;

    public UnitOfWork()
    {
        _endpointFinder = new EndpointFinder();
        _helperMethods = new HelperMethods();
    }

    public IEndpointFinder EndpointFinder => _endpointFinder;
    public IHelperMethods HelperMethods => _helperMethods;
}
