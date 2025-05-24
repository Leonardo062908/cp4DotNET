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

## Docker

Para facilitar o uso em diferentes ambientes e simplificar o deploy, a API está preparada para rodar em container Docker. Siga os passos abaixo:

### 1. Construindo a imagem Docker

Na raiz do projeto (onde está o `Dockerfile`), execute:

```bash
docker build -t motoapi:1.0 .
```

### 2. Testando localmente

```bash
docker run -d --name motoapi -p 8080:80 \
  -e "ASPNETCORE_URLS=http://+:80" \
  -e "ConnectionStrings__DefaultConnection=User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=HOST:PORT/SERVICE_NAME" \
  motoapi:1.0
```

### 3. Enviando para o Docker Hub

```bash
docker tag motoapi:1.0 seuusuario/motoapi:1.0
docker push seuusuario/motoapi:1.0
```

### 4. Executando na VM

```bash
docker pull seuusuario/motoapi:1.0
docker run -d --name motoapi -p 80:80 \
  -e "ASPNETCORE_URLS=http://+:80" \
  -e "ASPNETCORE_ENVIRONMENT=Development" \
  -e "ConnectionStrings__DefaultConnection=User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=HOST:PORT/SERVICE_NAME" \
  seuusuario/motoapi:1.0
```

* **DTOs + AutoMapper** para desacoplar entidade do contrato da API.
* **Validação** com DataAnnotations ou FluentValidation.
* **Versionamento de API** (`v1`, `v2`).
* **Health Checks** em `/health`.
* **Logging estruturado** (Serilog, Seq, etc.).
* **Dockerfile** para containerização.
* **Testes automatizados** (xUnit, Moq, EF Core InMemory).

## Scripts de Provisionamento e Deploy

### Scripts Azure CLI para criação da VM

````powershell
# 1. Login na Azure
az login

# 2. Registro do provedor de rede (Microsoft.Network)
az provider register --namespace Microsoft.Network

# 3. Criação do Resource Group
az group create `
  --name rg-motoapi `
  --location brazilsouth

# 4. Criação da VM Ubuntu 22.04 com SSH automático e NSG
docker rm -f motoapi || true
az vm create `
  --resource-group rg-motoapi `
  --name vm-motoapi `
  --image Ubuntu2204 `
  --size Standard_B1ms `
  --admin-username azureuser `
  --generate-ssh-keys `
  --nsg nsg-motoapi

# 5. Abertura das portas HTTP e HTTPS
az network nsg rule create `
  --resource-group rg-motoapi `
  --nsg-name nsg-motoapi `
  --name Allow-HTTP `
  --protocol tcp `
  --priority 1001 `
  --destination-port-range 80

az network nsg rule create `
  --resource-group rg-motoapi `
  --nsg-name nsg-motoapi `
  --name Allow-HTTPS `
  --protocol tcp `
  --priority 1002 `
  --destination-port-range 443
```{}

### Dockerfile Multi-stage

```dockerfile
# Stage 1: build
group AS build
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY *.sln .
COPY MotoApi/*.csproj ./MotoApi/
RUN dotnet restore

COPY MotoApi/. ./MotoApi/
WORKDIR /src/MotoApi
RUN dotnet publish -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

# Forçar Kestrel a ouvir na porta 80
ENV ASPNETCORE_URLS=http://+:80

# Criar usuário não-root
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

COPY --from=build /app/publish ./
EXPOSE 80
ENTRYPOINT ["dotnet","MotoApi.dll"]
````

### Deploy no Docker Hub e Execução na VM

1. **Build e Push local**

   ```powershell
   docker build -t motoapi:1.0 .
   docker tag motoapi:1.0 gavmarques/motoapi:1.0
   docker push gavmarques/motoapi:1.0
   ```

2. **Pull e execução na VM**

   ```bash
   ssh azureuser@<IP_DA_VM>
   docker pull gavmarques/motoapi:1.0
   docker run -d \
     --name motoapi \
     -p 80:80 \
     -e "ASPNETCORE_ENVIRONMENT=Development" \
     -e "ConnectionStrings__DefaultConnection=User Id=rm554889;Password=100805;Data Source=oracle.fiap.com.br:1521/ORCL" \
     gavmarques/motoapi:1.0
   ```

3. **Testes**

   * API JSON: `http://<IP_DA_VM>/api/motos`
   * Swagger UI: `http://<IP_DA_VM>/swagger/index.html`

> *Gabriel Marques RM554889 - Desenvolvedor*
> *Leonardo Mateus RM556629 - Desenvolvedor*
> *Leonardo Ribas RM557908 - Desenvolvedor*
