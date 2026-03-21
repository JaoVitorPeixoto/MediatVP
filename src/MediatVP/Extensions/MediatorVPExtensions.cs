using System.Reflection;
using MediatVP.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MediatVP.Extensions;

/// <summary>
/// Provides dependency injection extensions for MediatVP.
/// </summary>
public static class MediatVPExtensions
{
    /// <summary>
    /// Registers MediatVP core services, handlers, and optional open generic pipeline behaviors.
    /// </summary>
    /// <param name="services">Service collection where MediatVP will be registered.</param>
    /// <param name="configure">Optional configuration callback for assemblies and pipeline behaviors.</param>
    /// <returns>The same service collection instance for fluent chaining.</returns>
    public static IServiceCollection AddMediatVP(
        this IServiceCollection services,
        Action<MediatVPConfiguration>? configure = null
    )
    {
        var config = new MediatVPConfiguration();
        configure?.Invoke(config);

        if (config.Assemblies.Any())
        {   
            var handlerTypeWithResponse = typeof(IHandlerCommand<,>);
            var handlerTypeWithoutResponse = typeof(IHandlerCommand<>);

            foreach (var assembly in config.Assemblies)
            {
                var handlers = assembly.GetTypes()
                    .Where(type => !type.IsAbstract &&  !type.IsInterface)
                    .SelectMany(x => x.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                    .Where(ti => ti.Interface.IsGenericType &&
                        (ti.Interface.GetGenericTypeDefinition() == handlerTypeWithResponse ||
                        ti.Interface.GetGenericTypeDefinition() == handlerTypeWithoutResponse));

                foreach (var handler in handlers)
                    services.AddTransient(handler.Interface, handler.Type);
            }
        }

        foreach (var behaviorType in config.OpenBehaviors)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), behaviorType);
        }

        services.AddTransient<IMediator, Mediator>();

        return services;
    }

}
