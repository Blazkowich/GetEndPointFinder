using EndPointFinder.Repository.Implementation;
using EndPointFinder.Repository.Interfaces;

namespace EndPointFinder.Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private IEndpointFinder _endpointFinder;
    private IHelperMethods _helperMethods;
    private IApiFinder _apiFinder;
    private IMainMethods _mainMethods;

    public UnitOfWork()
    {
        _endpointFinder = new EndpointFinder();
        _helperMethods = new HelperMethods();
        _apiFinder = new ApiFinder();
        _mainMethods = new MainMethods(this);
    }

    public IEndpointFinder EndpointFinder => _endpointFinder;

    public IHelperMethods HelperMethods => _helperMethods;

    public IApiFinder ApiFinder => _apiFinder;

    public IMainMethods MainMethods => _mainMethods;
}
