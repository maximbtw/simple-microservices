FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Directory.Packages.props /
COPY ["PizzeriaApi.WebApi/PizzeriaApi.WebApi.csproj", "PizzeriaApi.WebApi/"]
COPY ["PizzeriaApi.Contracts/PizzeriaApi.Contracts.csproj", "PizzeriaApi.Contracts/"]
COPY ["PizzeriaApi.Application/PizzeriaApi.Application.csproj", "PizzeriaApi.Application/"]
COPY ["Media.Contracts/Media.Contracts.csproj", "Media.Contracts/"]
COPY ["Media.Client/Media.Client.csproj", "Media.Client/"]
COPY ["Catalog.Contracts/Catalog.Contracts.csproj", "Catalog.Contracts/"]
COPY ["Catalog.Client/Catalog.Client.csproj", "Catalog.Client/"]
COPY ["PizzeriaAccounting.Contracts/PizzeriaAccounting.Contracts.csproj", "PizzeriaAccounting.Contracts/"]
COPY ["PizzeriaAccounting.Client/PizzeriaAccounting.Client.csproj", "PizzeriaAccounting.Client/"]
COPY ["Platform.Client/Platform.Client.csproj", "Platform.Client/"]
COPY ["Platform.Core/Platform.Core.csproj", "Platform.Core/"]
COPY ["Platform.Domain/Platform.Domain.csproj", "Platform.Domain/"]
COPY ["Platform.WebApi/Platform.WebApi.csproj", "Platform.WebApi/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]

RUN dotnet restore "PizzeriaApi.WebApi/PizzeriaApi.WebApi.csproj"

COPY ["PizzeriaApi.WebApi/", "PizzeriaApi.WebApi/"]
COPY ["PizzeriaApi.Contracts/", "PizzeriaApi.Contracts/"]
COPY ["PizzeriaApi.Application/", "PizzeriaApi.Application/"]
COPY ["Media.Contracts/", "Media.Contracts/"]
COPY ["Media.Client/", "Media.Client/"]
COPY ["Catalog.Contracts/", "Catalog.Contracts/"]
COPY ["Catalog.Client/", "Catalog.Client/"]
COPY ["PizzeriaAccounting.Contracts/", "PizzeriaAccounting.Contracts/"]
COPY ["PizzeriaAccounting.Client/", "PizzeriaAccounting.Client/"]
COPY ["Platform.Client/", "Platform.Client/"]
COPY ["Platform.Core/", "Platform.Core/"]
COPY ["Platform.Domain/", "Platform.Domain/"]
COPY ["Platform.WebApi/", "Platform.WebApi/"]
COPY ["Utilities/", "Utilities/"]

WORKDIR "/src/PizzeriaApi.WebApi"
RUN dotnet build "PizzeriaApi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PizzeriaApi.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "PizzeriaApi.WebApi.dll"]
