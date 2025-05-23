# Moto API

> **API RESTful para gerenciamento de motos em banco Oracle via ASP.NET Core e EF Core**

![.NET](https://img.shields.io/badge/.NET-8.0-blue) ![Oracle](https://img.shields.io/badge/Oracle-EF%20Core-red) ![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-green)

## Descrição

Este projeto implementa uma API Web em **ASP.NET Core 8** para CRUD completo da entidade **Moto**, integrando com **Oracle Database** através do **Entity Framework Core**. A API oferece:

* **GET**: múltiplas rotas (todos, por ID, filtros por `statusMotoId`, busca por `modelo`).
* **POST**: criação de novas motos.
* **PUT**: atualização de motos existentes.
* **DELETE**: remoção de motos.
* **OpenAPI/Swagger**: documentação automática das rotas.

## Tecnologias

* .NET SDK 8.0
* Oracle.EntityFrameworkCore 9.23.80
* Microsoft.EntityFrameworkCore.Design 9.0.5
* Microsoft.EntityFrameworkCore.Tools 9.0.5
* Swashbuckle.AspNetCore 6.4.0

## Pré-requisitos

* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
* Oracle Database acessível (versão 12c+)
* Credenciais e string de conexão para Oracle
* Visual Studio 2022 ou VS Code

## Instalação

1. **Clone o repositório**

   ```bash
   git clone https://github.com/seu-usuario/moto-api.git
   cd moto-api
   ```

2. **Configure a string de conexão**

   * Abra `appsettings.json` e ajuste:

     ```json
     "ConnectionStrings": {
       "DefaultConnection": "User Id=SEU_USUARIO;Password=SUASENHA;Data Source=HOST:PORT/SERVICE_NAME"
     }
     ```

3. **Restaure pacotes**

   ```bash
   dotnet restore
   ```

4. **Gerar e aplicar migrations**

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Execute a API**

   ```bash
   dotnet run
   ```

6. **Acesse Swagger UI**

   * Abra no navegador: `https://localhost:5001/swagger`

## Rotas da API

| Método | Endpoint                           | Descrição                                          |
| ------ | ---------------------------------- | -------------------------------------------------- |
| GET    | `/api/motos`                       | Lista todas as motos (opcional: `?statusMotoId=1`) |
| GET    | `/api/motos/{id}`                  | Retorna a moto de ID especificado                  |
| GET    | `/api/motos?statusMotoId={status}` | Filtra motos por `statusMotoId`                    |
| GET    | `/api/motos/search?modelo={texto}` | Busca motos cujo `modelo` contém `{texto}`         |
| POST   | `/api/motos`                       | Cria uma nova moto                                 |
| PUT    | `/api/motos/{id}`                  | Atualiza a moto de ID especificado                 |
| DELETE | `/api/motos/{id}`                  | Remove a moto de ID especificado                   |

## Boas práticas & melhorias

* **DTOs + AutoMapper** para desacoplar entidade do contrato da API.
* **Validação** com DataAnnotations ou FluentValidation.
* **Versionamento de API** (`v1`, `v2`).
* **Health Checks** em `/health`.
* **Logging estruturado** (Serilog, Seq, etc.).
* **Dockerfile** para containerização.
* **Testes automatizados** (xUnit, Moq, EF Core InMemory).

> *Gabriel Marques RM554889 - Desenvolvedor*
> *Leonardo Mateus RM556629 - Desenvolvedor*
> *Leonardo Ribas RM557908 - Desenvolvedor*
