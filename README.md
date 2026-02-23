# MediatVP

Uma implementação com fins de estudo **leve e moderna** de padrão Mediator para .NET, inspirada no MediatR, mas com foco em simplicidade e zero dependências pesadas.

MediatVP permite desacoplar completamente envio de comandos, queries e eventos da lógica de negócio, facilitando testes, manutenção e escalabilidade em aplicações .NET.


[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com)
[![NuGet](https://img.shields.io/nuget/v/MediatVP?color=green)](https://www.nuget.org/packages/MediatVP)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Recursos Principais

- Envio async de requests com e sem resposta (`SendAsync<TResponse>`)
- Suporte a CancellationToken nativo
- Registro automático via DI (Microsoft.Extensions.DependencyInjection)
- Exceções customizadas claras para falhas comuns

## Instalação

Instale via NuGet:

```bash
# Pacote principal (recomendado para uso completo)
dotnet add package MediatVP

# Apenas as abstrações (para domínio/testes sem dependência de implementação)
dotnet add package MediatVP.Abstractions