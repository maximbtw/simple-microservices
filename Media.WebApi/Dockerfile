FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Directory.Packages.props /
COPY ["Media.WebApi/Media.WebApi.csproj", "Media.WebApi/"]
COPY ["Media.Domain/Media.Domain.csproj", "Media.Domain/"]
COPY ["Media.Contracts/Media.Contracts.csproj", "Media.Contracts/"]
COPY ["Media.Client/Media.Client.csproj", "Media.Client/"]
COPY ["Media.Application/Media.Application.csproj", "Media.Application/"]
COPY ["Platform.Client/Platform.Client.csproj", "Platform.Client/"]
COPY ["Platform.Core/Platform.Core.csproj", "Platform.Core/"]
COPY ["Platform.Domain/Platform.Domain.csproj", "Platform.Domain/"]
COPY ["Platform.WebApi/Platform.WebApi.csproj", "Platform.WebApi/"]
COPY ["Utilities/Utilities.csproj", "Utilities/"]

RUN dotnet restore "Media.WebApi/Media.WebApi.csproj"

COPY ["Media.WebApi/", "Media.WebApi/"]
COPY ["Media.Domain/", "Media.Domain/"]
COPY ["Media.Contracts/", "Media.Contracts/"]
COPY ["Media.Client/", "Media.Client/"]
COPY ["Media.Application/", "Media.Application/"]
COPY ["Platform.Client/", "Platform.Client/"]
COPY ["Platform.Core/", "Platform.Core/"]
COPY ["Platform.Domain/", "Platform.Domain/"]
COPY ["Platform.WebApi/", "Platform.WebApi/"]
COPY ["Utilities/", "Utilities/"]

WORKDIR "/src/Media.WebApi"
RUN dotnet build "Media.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Media.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "Media.WebApi.dll"]
