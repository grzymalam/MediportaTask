namespace MediportaTask.Misc;

public interface IModule
{
    public IServiceCollection Register(IServiceCollection services);
    public IEndpointRouteBuilder Map(IEndpointRouteBuilder builder);
}
