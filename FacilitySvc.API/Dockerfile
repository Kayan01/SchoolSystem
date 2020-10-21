# Multistage Dockerfile for Schooltrack-auth-service linux-based image.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FacilitySvc.API/FacilitySvc.API.csproj", "FacilitySvc.API/"]
RUN mkdir FacilitySvc.API/FileStore && touch FacilitySvc.API/FileStore/TempFile.txt
COPY ["FacilitySvc.Core/FacilitySvc.Core.csproj", "FacilitySvc.Core/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "FacilitySvc.API/FacilitySvc.API.csproj"
COPY . .
WORKDIR "/src/FacilitySvc.API"
RUN dotnet build "FacilitySvc.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FacilitySvc.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FacilitySvc.API.dll"]
