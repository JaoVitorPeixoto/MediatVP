using System;

namespace MediatVP.Exceptions;

/// <summary>
/// Exception thrown when a reflected handler method returns an unexpected runtime type.
/// </summary>
public class MethodReturnUnexpectedTypeException : Exception
{
    internal object? Result;


    internal MethodReturnUnexpectedTypeException(object? result) 
    : base($"Method returned unexpected type {result}")
    {
        this.Result = result;
    }

    internal MethodReturnUnexpectedTypeException(string message, object? result) 
    : base(message)
    {
        this.Result = result;
    }
}
