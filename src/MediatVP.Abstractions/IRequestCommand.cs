using System;

namespace MediatVP.Abstractions;

public interface IRequestCommand<out TResponse>;

public interface IRequestCommand;
