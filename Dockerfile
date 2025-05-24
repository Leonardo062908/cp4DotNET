########################################
# STAGE 1: build da aplicação
########################################
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
ENV ASPNETCORE_URLS=http://+:80

# Copia solução e restaura pacotes
COPY *.sln .
COPY MotoApi/*.csproj ./MotoApi/
RUN dotnet restore

# Copia código e publica em Release
COPY MotoApi/. ./MotoApi/
WORKDIR /src/MotoApi
RUN dotnet publish -c Release -o /app/publish

########################################
# STAGE 2: imagem final de runtime
########################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

# Cria usuário sem privilégios root
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

# Copia binários publicados
COPY --from=build /app/publish ./

# Expõe porta padrão (ajuste se necessário)
EXPOSE 80

ENTRYPOINT ["dotnet", "MotoApi.dll"]
