using System.Reflection;
using MediatVP.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MediatVP.Extensions;

public static class MediatVPExtensions
{
    public static IServiceCollection AddMediatVP(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddTransient<IMediator, Mediator>();

        var handlerTypeWithResponse = typeof(IHandlerCommand<,>);
        var handlerTypeWithoutResponse = typeof(IHandlerCommand<>);

        foreach (var assembly in assemblies)
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

        return services;
    }

}
