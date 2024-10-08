#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

ENV AWS_ACCESS_KEY_ID=AKIA47CRUUCC7ZR6ZTNX
ENV AWS_SECRET_ACCESS_KEY=Zq+q4K4tN41cwQ60xkog9lZgUMqOuiRSsWTRenQr
ENV AWS_REGION=eu-central-1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["server-side/src/JobManagement/JobManagement.API/JobManagement.API.csproj", "server-side/src/JobManagement/JobManagement.API/"]
COPY ["server-side/src/JobManagement/JobManagement.Application/JobManagement.Application.csproj", "server-side/src/JobManagement/JobManagement.Application/"]
COPY ["server-side/src/JobManagement/JobManagement.Domain/JobManagement.Domain.csproj", "server-side/src/JobManagement/JobManagement.Domain/"]
COPY ["server-side/src/JobManagement/JobManagement.Infrastructure/JobManagement.Infrastructure.csproj", "server-side/src/JobManagement/JobManagement.Infrastructure/"]
RUN dotnet restore "./server-side/src/JobManagement/JobManagement.API/JobManagement.API.csproj"
COPY . .
WORKDIR "/src/server-side/src/JobManagement/JobManagement.API"
RUN dotnet build "./JobManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JobManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobManagement.API.dll"]