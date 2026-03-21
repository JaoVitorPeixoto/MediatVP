using System;
using System.Reflection;
using MediatVP.Exceptions;

namespace MediatVP.Extensions;

/// <summary>
/// Defines MediatVP registration options used by dependency injection setup.
/// </summary>
public class MediatVPConfiguration
{
    internal List<Assembly> Assemblies { get; } = new();
    internal List<Type> OpenBehaviors { get; } = new ();

    /// <summary>
    /// Adds a single assembly for handler discovery.
    /// </summary>
    /// <param name="assembly">Assembly to scan for handler implementations.</param>
    /// <returns>The current configuration instance.</returns>
    public MediatVPConfiguration RegisterServicesFromAssembly(Assembly assembly)
    { 
        if (assembly == null)
            throw new ArgumentNullException(nameof(assembly));

        Assemblies.Add(assembly);
        return this;
    }

    /// <summary>
    /// Adds multiple assemblies for handler discovery.
    /// </summary>
    /// <param name="assemblies">Assemblies to scan for handler implementations.</param>
    /// <returns>The current configuration instance.</returns>
    public MediatVPConfiguration RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        if (assemblies == null)
            throw new ArgumentNullException(nameof(assemblies));

        Assemblies.AddRange(assemblies);
        return this;
    } 

    /// <summary>
    /// Registers an open generic pipeline behavior type.
    /// </summary>
    /// <param name="openBehaviorType">Open generic behavior type, such as SomeBehavior&lt;,&gt;.</param>
    /// <returns>The current configuration instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="openBehaviorType"/> is null.</exception>
    /// <exception cref="ClosedBehaviorException">Thrown when the behavior type is not an open generic definition.</exception>
    /// <exception cref="ArgumentException">Thrown when the behavior type does not have exactly two generic arguments.</exception>
    public MediatVPConfiguration AddOpenBehavior(Type openBehaviorType)
    {
        if (openBehaviorType == null)
            throw new ArgumentNullException(nameof(openBehaviorType));

        if (!openBehaviorType.IsGenericTypeDefinition)
            throw new ClosedBehaviorException(openBehaviorType);

        var genericArgsCount = openBehaviorType.GetGenericArguments().Length;

        if (genericArgsCount != 2)
            throw new ArgumentException(
            "AddOpenBehavior only accepts open generic types with exactly 2 type parameters " +
            "(for example, typeof(ValidationBehavior<,>) or typeof(LoggingBehavior<,>)).",
            nameof(openBehaviorType));

        OpenBehaviors.Add(openBehaviorType);
        return this;
    }
}
