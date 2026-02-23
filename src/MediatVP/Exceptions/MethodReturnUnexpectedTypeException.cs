using System;

namespace MediatVP.Exceptions;

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
