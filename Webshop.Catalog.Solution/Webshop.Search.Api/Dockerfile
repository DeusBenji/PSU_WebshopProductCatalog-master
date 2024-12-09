# Se https://aka.ms/containerfastmode for at forstå, hvordan Visual Studio bruger denne Dockerfile til hurtigere debugging.

# Basisbillede for runtime-miljøet
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Basisbillede for build-miljøet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopier projektfiler
COPY ["Webshop.Search.Api/Webshop.Search.Api.csproj", "Webshop.Search.Api/"]
COPY ["Webshop.Search.Application/Webshop.Search.Application.csproj", "Webshop.Search.Application/"]
COPY ["Webshop.Search.Domain/Webshop.Search.Domain.csproj", "Webshop.Search.Domain/"]
COPY ["Webshop.Search.Persistence/Webshop.Search.Persistence.csproj", "Webshop.Search.Persistence/"]
COPY ["Webshop.Application/Webshop.Application.csproj", "Webshop.Application/"]
COPY ["Webshop.Domain/Webshop.Domain.csproj", "Webshop.Domain/"]

# Gendan afhængigheder
RUN dotnet restore "Webshop.Search.Api/Webshop.Search.Api.csproj"

# Kopier resten af filerne
COPY . .

# Skift arbejdsmappen til API-projektet
WORKDIR "/src/Webshop.Search.Api"

# Byg API-projektet
RUN dotnet build "Webshop.Search.Api.csproj" -c Release -o /app/build

# Publiser trin
FROM build AS publish
RUN dotnet publish "Webshop.Search.Api.csproj" -c Release -o /app/publish

# Runtime trin
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Angiv entrypoint
ENTRYPOINT ["dotnet", "Webshop.Search.Api.dll"]
