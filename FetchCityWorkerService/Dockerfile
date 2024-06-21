#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Use the .NET 8.0 runtime as a base image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app
USER app

# Use the .NET 8.0 SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

# Copy and restore dependencies
COPY ["FetchCityWorkerService/FetchCityWorkerService.csproj", "FetchCityWorkerService/"]
COPY ["RentalInvestmentAid.Core/RentalInvestmentAid.Core.csproj", "RentalInvestmentAid.Core/"]
COPY ["RentalInvestmentAid.Caching/RentalInvestmentAid.Caching.csproj", "RentalInvestmentAid.Caching/"]
COPY ["RentalInvestmentAid.Database/RentalInvestmentAid.Database.csproj", "RentalInvestmentAid.Database/"]
COPY ["RentalInvestmentAid.Logger/RentalInvestmentAid.Logger.csproj", "RentalInvestmentAid.Logger/"]
COPY ["RentalInvestmentAid.Models/RentalInvestmentAid.Models.csproj", "RentalInvestmentAid.Models/"]
COPY ["RentalInvestmentAid.Settings/RentalInvestmentAid.Settings.csproj", "RentalInvestmentAid.Settings/"]
COPY ["RentalInvestmentAid.GeminiAPICaller/RentalInvestmentAid.GeminiAPICaller.csproj", "RentalInvestmentAid.GeminiAPICaller/"]
COPY ["/RentalInvestmentAid.Queue/RentalInvestmentAid.Queue.csproj", "RentalInvestmentAid.Queue/"]
RUN dotnet restore "./FetchCityWorkerService/./FetchCityWorkerService.csproj"

# Copy the rest of the source code and build the application
COPY . .
WORKDIR "/src/FetchCityWorkerService"
RUN dotnet build "FetchCityWorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "FetchCityWorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FetchCityWorkerService.dll"]