#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#base image => sdk .net 6.0
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
#Container Port
EXPOSE 80 

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src # = cd
#copy from local machine directory to container directory 
COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"]
#Restore package used in application 
RUN dotnet restore "Services/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
#Switch to the directory where files copies in container
WORKDIR "/src/Services/Catalog/Catalog.API"
#Build Application  as release build 
# -c => configuration
# -o => output
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#application entry point
ENTRYPOINT ["dotnet", "Catalog.API.dll"]