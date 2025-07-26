FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Directory.Packages.props /
COPY ["Auth.WebApi/Auth.WebApi.csproj", "Auth.WebApi/"]
COPY ["Auth.Domain/Auth.Domain.csproj", "Auth.Domain/"]
COPY ["Auth.Contracts/Auth.Contracts.csproj", "Auth.Contracts/"]
COPY ["Auth.Client/Auth.Client.csproj", "Auth.Client/"]
COPY ["Auth.Application/Auth.Application.csproj", "Auth.Application/"]
COPY ["Platform.Client/Platform.Client.csproj", "Platform.Client/"]
COPY ["Platform.Core/Platform.Core.csproj", "Platform.Core/"]
COPY ["Platform.Domain/Platform.Domain.csproj", "Platform.Domain/"]
COPY ["Platform.WebApi/Platform.WebApi.csproj", "Platform.WebApi/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]

RUN dotnet restore "Auth.WebApi/Auth.WebApi.csproj"

COPY ["Auth.WebApi/", "Auth.WebApi/"]
COPY ["Auth.Domain/", "Auth.Domain/"]
COPY ["Auth.Contracts/", "Auth.Contracts/"]
COPY ["Auth.Client/", "Auth.Client/"]
COPY ["Auth.Application/", "Auth.Application/"]
COPY ["Platform.Client/", "Platform.Client/"]
COPY ["Platform.Core/", "Platform.Core/"]
COPY ["Platform.Domain/", "Platform.Domain/"]
COPY ["Platform.WebApi/", "Platform.WebApi/"]
COPY ["Utilities/", "Utilities/"]

WORKDIR "/src/Auth.WebApi"
RUN dotnet build "Auth.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Auth.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "Auth.WebApi.dll"]
