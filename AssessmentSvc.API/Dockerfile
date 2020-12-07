#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AssesmentSvc.API/AssesmentSvc.API.csproj", "AssesmentSvc.API/"]
RUN mkdir AssesmentSvc.API/FileStore && touch AssesmentSvc.API/FileStore/TempFile.txt
COPY ["AssesmentSvc.Core/AssesmentSvc.Core.csproj", "AssesmentSvc.Core/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "AssesmentSvc.API/AssesmentSvc.API.csproj"
COPY . .
WORKDIR "/src/AssesmentSvc.API"
RUN dotnet build "AssesmentSvc.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AssesmentSvc.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssesmentSvc.API.dll"]