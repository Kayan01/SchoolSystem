# Multistage Dockerfile for Schooltrack-auth-service linux-based image.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Auth.API/Auth.API.csproj", "Auth.API/"]
RUN mkdir Auth.API/Filestore && touch Auth.API/Filestore/TempFile.txt
COPY ["Auth.Core/Auth.Core.csproj", "Auth.Core/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "Auth.API/Auth.API.csproj"

COPY . .
WORKDIR "/src/Auth.API"
RUN dotnet build "Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.API.dll"]
