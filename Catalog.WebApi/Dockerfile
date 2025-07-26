FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Directory.Packages.props /
COPY ["Catalog.WebApi/Catalog.WebApi.csproj", "Catalog.WebApi/"]
COPY ["Catalog.Domain/Catalog.Domain.csproj", "Catalog.Domain/"]
COPY ["Catalog.Contracts/Catalog.Contracts.csproj", "Catalog.Contracts/"]
COPY ["Catalog.Client/Catalog.Client.csproj", "Catalog.Client/"]
COPY ["Catalog.Application/Catalog.Application.csproj", "Catalog.Application/"]
COPY ["Platform.Client/Platform.Client.csproj", "Platform.Client/"]
COPY ["Platform.Core/Platform.Core.csproj", "Platform.Core/"]
COPY ["Platform.Domain/Platform.Domain.csproj", "Platform.Domain/"]
COPY ["Platform.WebApi/Platform.WebApi.csproj", "Platform.WebApi/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]

RUN dotnet restore "Catalog.WebApi/Catalog.WebApi.csproj"

COPY ["Catalog.WebApi/", "Catalog.WebApi/"]
COPY ["Catalog.Domain/", "Catalog.Domain/"]
COPY ["Catalog.Contracts/", "Catalog.Contracts/"]
COPY ["Catalog.Client/", "Catalog.Client/"]
COPY ["Catalog.Application/", "Catalog.Application/"]
COPY ["Platform.Client/", "Platform.Client/"]
COPY ["Platform.Core/", "Platform.Core/"]
COPY ["Platform.Domain/", "Platform.Domain/"]
COPY ["Platform.WebApi/", "Platform.WebApi/"]
COPY ["Utilities/", "Utilities/"]

WORKDIR "/src/Catalog.WebApi"
RUN dotnet build "Catalog.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Catalog.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "Catalog.WebApi.dll"]
