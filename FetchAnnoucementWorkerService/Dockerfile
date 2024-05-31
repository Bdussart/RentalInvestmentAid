#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FetchAnnoucementWorkerService/FetchAnnoucementWorkerService.csproj", "FetchAnnoucementWorkerService/"]
COPY ["RentalInvestmentAid.Core/RentalInvestmentAid.Core.csproj", "RentalInvestmentAid.Core/"]
COPY ["RentalInvestmentAid.Caching/RentalInvestmentAid.Caching.csproj", "RentalInvestmentAid.Caching/"]
COPY ["RentalInvestmentAid.Database/RentalInvestmentAid.Database.csproj", "RentalInvestmentAid.Database/"]
COPY ["RentalInvestmentAid.Logger/RentalInvestmentAid.Logger.csproj", "RentalInvestmentAid.Logger/"]
COPY ["RentalInvestmentAid.Models/RentalInvestmentAid.Models.csproj", "RentalInvestmentAid.Models/"]
COPY ["RentalInvestmentAid.Settings/RentalInvestmentAid.Settings.csproj", "RentalInvestmentAid.Settings/"]
COPY ["RentalInvestmentAid.Queue/RentalInvestmentAid.Queue.csproj", "RentalInvestmentAid.Queue/"]
RUN dotnet restore "./FetchAnnoucementWorkerService/./FetchAnnoucementWorkerService.csproj"
COPY . .
WORKDIR "/src/FetchAnnoucementWorkerService"
RUN dotnet build "./FetchAnnoucementWorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FetchAnnoucementWorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FetchAnnoucementWorkerService.dll"]