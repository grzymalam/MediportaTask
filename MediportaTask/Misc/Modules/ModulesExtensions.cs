using System.Reflection;

namespace MediportaTask.Misc.Modules;

public static class ModulesExtensions
{
    public static IServiceCollection RegisterModules(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var modules = currentAssembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IModule)));
        var moduleInstancesAsObjects = modules.Where(m => !m.IsInterface).Select(Activator.CreateInstance);
        foreach (var moduleInstanceAsObject in moduleInstancesAsObjects)
        {
            var moduleInstance = (IModule)moduleInstanceAsObject;
            if(moduleInstance is null)
            {
                throw new ArgumentException($"The following module's services could not be registered in the DI container: {moduleInstanceAsObject?.GetType()}");
            }
            
            moduleInstance.Register(services);
        }

        return services;
    }

    public static IEndpointRouteBuilder AddEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var modules = currentAssembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IModule)));
        var moduleInstancesAsObjects = modules.Where(m => !m.IsInterface).Select(Activator.CreateInstance);
        foreach (var moduleInstanceAsObject in moduleInstancesAsObjects)
        {
            var moduleInstance = (IModule)moduleInstanceAsObject;
            if (moduleInstance is null)
            {
                throw new ArgumentException($"The following module's endpoints could not be registered in the DI container: {moduleInstanceAsObject?.GetType()}");
            }

            moduleInstance.Map(endpointRouteBuilder);
        }

        return endpointRouteBuilder;
    }
}

