FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_ENVIRONMENT=Development
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["netflix-back.Api/netflix-back.Api.csproj", "netflix-back.Api/"]
COPY ["netflix-back.Application/netflix-back.Application.csproj", "netflix-back.Application/"]
COPY ["netflix-back.Domain/netflix-back.Domain.csproj", "netflix-back.Domain/"]
COPY ["netflix-back.Infrastructure/netflix-back.Infrastructure.csproj", "netflix-back.Infrastructure/"]
RUN dotnet restore "netflix-back.Api/netflix-back.Api.csproj"
COPY . .
WORKDIR "/src/netflix-back.Api"
RUN dotnet build "./netflix-back.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./netflix-back.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "netflix-back.Api.dll"]
