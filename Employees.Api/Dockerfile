# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar tudo e restaurar as dependências
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar build da etapa anterior
COPY --from=build /app/publish .

# Expõe porta padrão HTTP
EXPOSE 80

# EntryPoint da aplicação
ENTRYPOINT ["dotnet", "Employees.Api.dll"]
