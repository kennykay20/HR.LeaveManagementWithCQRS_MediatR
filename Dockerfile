# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# install curl inside my app image
RUN apt-get update && apt-get install -y curl

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081



# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HR_LeaveManagement.Api/HR_LeaveManagement.Api.csproj", "HR_LeaveManagement.Api/"]
COPY ["HR_LeaveManagement.Application/HR_LeaveManagement.Application.csproj", "HR_LeaveManagement.Application/"]
COPY ["HR_LeaveManagement.Domain/HR_LeaveManagement.Domain.csproj", "HR_LeaveManagement.Domain/"]
COPY ["HR_LeaveManagement.Infrastructure/HR_LeaveManagement.Infrastructure.csproj", "HR_LeaveManagement.Infrastructure/"]
COPY ["HR_LeaveManagement.Persistence/HR_LeaveManagement.Persistence.csproj", "HR_LeaveManagement.Persistence/"]
RUN dotnet restore "./HR_LeaveManagement.Api/HR_LeaveManagement.Api.csproj"
COPY . .
WORKDIR "/src/HR_LeaveManagement.Api"
RUN dotnet build "./HR_LeaveManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HR_LeaveManagement.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HR_LeaveManagement.Api.dll"]