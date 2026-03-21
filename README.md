# MediatVP

Uma implementação com fins de estudo **leve e moderna** de padrão Mediator para .NET, inspirada no MediatR, mas com foco em simplicidade e zero dependências pesadas.

MediatVP permite desacoplar completamente envio de comandos, queries e eventos da lógica de negócio, facilitando testes, manutenção e escalabilidade em aplicações .NET.

| Package | SDK | Version | Downloads | License |
| ------- | ----- | ----- | ----- | ----- |
| `Peixoto.MediatVP` | [![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com) | [![NuGet](https://img.shields.io/nuget/v/Peixoto.MediatVP?color=green)](https://www.nuget.org/packages/Peixoto.MediatVP) | [![Nuget](https://img.shields.io/nuget/dt/Peixoto.MediatVP.svg)](https://nuget.org/packages/Peixoto.MediatVP) | [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE) |

## Recursos Principais

- Envio async de requests com e sem resposta (`SendAsync<TResponse>`)
- Suporte a CancellationToken nativo
- Registro automático via DI (Microsoft.Extensions.DependencyInjection)
- Pipeline Behaviors com suporte a `IPipelineBehavior<TRequest, TResponse>`
- Suporte a requests sem retorno via `Unit`
- Exceções customizadas claras para falhas comuns

## Instalação

Instale via NuGet:

```bash
# Pacote principal (recomendado para uso completo)
dotnet add package Peixoto.MediatVP

# Apenas as abstrações (para domínio/testes sem dependência de implementação)
dotnet add package Peixoto.MediatVP.Abstractions
```

Versão atual da biblioteca: `2.0.0`.

## Uso Rápido

```csharp
using MediatVP.Abstractions;
using MediatVP.Extensions;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddMediatVP(config =>
{
	config.RegisterServicesFromAssembly(typeof(Program).Assembly);
	config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var mediator = services.BuildServiceProvider().GetRequiredService<IMediator>();
await mediator.SendAsync(new CreateUserCommand("Vitor", "vitor@gmail.com"));
```



## Testes

Para executar os testes:

```bash
dotnet test test/MediatVP.Tests/MediatVP.Tests.csproj
```

## Observações

- Este projeto foi criado para estudo e experimentação de arquitetura baseada em mediator.
- A API foi refinada para manter simplicidade no uso e previsibilidade no pipeline.