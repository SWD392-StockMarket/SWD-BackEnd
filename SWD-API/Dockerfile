#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SWD-API/SWD.API.csproj", "SWD-API/"]
COPY ["SWD.Service/SWD.Service.csproj", "SWD.Service/"]
COPY ["SWD.Repository/SWD.Repository.csproj", "SWD.Repository/"]
COPY ["SWD.Data/SWD.Data.csproj", "SWD.Data/"]
RUN dotnet restore "./SWD-API/SWD.API.csproj"
COPY . .
WORKDIR "/src/SWD-API"
RUN dotnet build "./SWD.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SWD.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SWD.API.dll"]