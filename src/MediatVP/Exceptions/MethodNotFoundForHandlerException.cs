using System;

namespace MediatVP.Exceptions;

public class MethodNotFoundForHandlerException : Exception
{
    internal Type HandlerType;


    internal MethodNotFoundForHandlerException(Type handlerType) 
    : base($"Method not found for {handlerType}")
    {
        this.HandlerType = handlerType;
    }

    internal MethodNotFoundForHandlerException(string message, Type handlerType) 
    : base(message)
    {
        this.HandlerType = handlerType;
    }
}
