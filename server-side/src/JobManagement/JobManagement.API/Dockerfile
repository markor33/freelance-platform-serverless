#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN useradd -m -s /bin/bash appuser
WORKDIR /app
USER appuser
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./src/JobManagement/JobManagement.API/JobManagement.API.csproj", "JobManagement/JobManagement.API/"]
COPY ["./src/JobManagement/JobManagement.Application/JobManagement.Application.Commands.csproj", "JobManagement/JobManagement.Application/"]
COPY ["./src/JobManagement/JobManagement.Domain/JobManagement.Domain.csproj", "JobManagement/JobManagement.Domain/"]
COPY ["./src/JobManagement/JobManagement.Infrastructure/JobManagement.Infrastructure.csproj", "JobManagement/JobManagement.Infrastructure/"]

RUN dotnet restore "JobManagement.API.csproj"

COPY . .

WORKDIR "/src/JobManagement/JobManagement.API/"

RUN dotnet build "JobManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "JobManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app
USER appuser
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobManagement.API.dll"]