# Multistage Dockerfile for Schooltrack-Finance-service linux-based image.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["FinanceSvc.API/FinanceSvc.API.csproj", "FinanceSvc.API/"]
COPY ["FinanceSvc.Core/FinanceSvc.Core.csproj", "FinanceSvc.Core/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "FinanceSvc.API/FinanceSvc.API.csproj"
COPY . .
WORKDIR "/src/FinanceSvc.API"
RUN dotnet build "FinanceSvc.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinanceSvc.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceSvc.API.dll"]