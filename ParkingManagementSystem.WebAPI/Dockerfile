#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ParkingManagementSystem.WebAPI/ParkingManagementSystem.WebAPI.csproj", "ParkingManagementSystem.WebAPI/"]
COPY ["ParkingManagementSystem.Business/ParkingManagementSystem.Business.csproj", "ParkingManagementSystem.Business/"]
RUN dotnet restore "ParkingManagementSystem.WebAPI/ParkingManagementSystem.WebAPI.csproj"
COPY . .
WORKDIR "/src/ParkingManagementSystem.WebAPI"
RUN dotnet build "ParkingManagementSystem.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParkingManagementSystem.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParkingManagementSystem.WebAPI.dll"]