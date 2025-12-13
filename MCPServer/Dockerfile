# Usa a imagem oficial do .NET SDK como build environment
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Copia os arquivos .csproj de todos os projetos para restaurar as dependências
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["MCPServer/MCPServer.csproj", "MCPServer/"]

# Restaura as dependências
RUN dotnet restore ./MCPServer/MCPServer.csproj

# Copia todo o código restante para o contêiner
COPY . .

# Publica a aplicação (gera binários e dependências)
RUN dotnet publish ./MCPServer/MCPServer.csproj -c Release -o /app/out

# Usa a imagem oficial do .NET runtime como runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expõe a porta 2000 e inicia a aplicação
EXPOSE 2000
ENTRYPOINT ["dotnet", "MCPServer.dll"]
