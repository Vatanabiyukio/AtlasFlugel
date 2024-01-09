FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AtlasFlugel.Api/AtlasFlugel.Api.csproj", "AtlasFlugel.Api/"]
RUN dotnet restore "AtlasFlugel.Api/AtlasFlugel.Api.csproj"
COPY . .
WORKDIR "/src/AtlasFlugel.Api"
RUN dotnet build "AtlasFlugel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AtlasFlugel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AtlasFlugel.Api.dll"]
