FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Directory.Packages.props /
COPY ["PizzeriaAccounting.WebApi/PizzeriaAccounting.WebApi.csproj", "PizzeriaAccounting.WebApi/"]
COPY ["PizzeriaAccounting.Domain/PizzeriaAccounting.Domain.csproj", "PizzeriaAccounting.Domain/"]
COPY ["PizzeriaAccounting.Contracts/PizzeriaAccounting.Contracts.csproj", "PizzeriaAccounting.Contracts/"]
COPY ["PizzeriaAccounting.Client/PizzeriaAccounting.Client.csproj", "PizzeriaAccounting.Client/"]
COPY ["PizzeriaAccounting.Application/PizzeriaAccounting.Application.csproj", "PizzeriaAccounting.Application/"]
COPY ["Auth.Client/Auth.Client.csproj", "Auth.Client/"]
COPY ["Auth.Contracts/Auth.Contracts.csproj", "Auth.Contracts/"]
COPY ["Platform.Client/Platform.Client.csproj", "Platform.Client/"]
COPY ["Platform.Core/Platform.Core.csproj", "Platform.Core/"]
COPY ["Platform.Domain/Platform.Domain.csproj", "Platform.Domain/"]
COPY ["Platform.WebApi/Platform.WebApi.csproj", "Platform.WebApi/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]

RUN dotnet restore "PizzeriaAccounting.WebApi/PizzeriaAccounting.WebApi.csproj"

COPY ["PizzeriaAccounting.WebApi/", "PizzeriaAccounting.WebApi/"]
COPY ["PizzeriaAccounting.Domain/", "PizzeriaAccounting.Domain/"]
COPY ["PizzeriaAccounting.Contracts/", "PizzeriaAccounting.Contracts/"]
COPY ["PizzeriaAccounting.Client/", "PizzeriaAccounting.Client/"]
COPY ["PizzeriaAccounting.Application/", "PizzeriaAccounting.Application/"]
COPY ["Auth.Contracts/", "Auth.Contracts/"]
COPY ["Auth.Client/", "Auth.Client/"]
COPY ["Platform.Client/", "Platform.Client/"]
COPY ["Platform.Core/", "Platform.Core/"]
COPY ["Platform.Domain/", "Platform.Domain/"]
COPY ["Platform.WebApi/", "Platform.WebApi/"]
COPY ["Utilities/", "Utilities/"]

WORKDIR "/src/PizzeriaAccounting.WebApi"
RUN dotnet build "PizzeriaAccounting.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PizzeriaAccounting.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "PizzeriaAccounting.WebApi.dll"]
