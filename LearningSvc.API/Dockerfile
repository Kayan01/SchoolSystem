# Multistage Dockerfile for Schooltrack-learning-service linux-based image.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LearningSvc.API/LearningSvc.API.csproj", "LearningSvc.API/"]
COPY ["LearningSvc.Core/LearningSvc.Core.csproj", "LearningSvc.Core/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "LearningSvc.API/LearningSvc.API.csproj"
COPY . .
WORKDIR "/src/LearningSvc.API"
RUN dotnet build "LearningSvc.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearningSvc.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningSvc.API.dll"]