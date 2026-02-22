using System;
using MediatVP.Abstraction;

namespace MediatVP.Exceptions;

internal class HandlerNotFoundException : Exception
{
    internal Type HandlerType;


    internal HandlerNotFoundException(Type handlerType) 
    : base($"Handler not found for {nameof(handlerType)}")
    {
        this.HandlerType = handlerType;
    }

    internal HandlerNotFoundException(string message, Type handlerType) 
    : base(message)
    {
        this.HandlerType = handlerType;
    }

}
