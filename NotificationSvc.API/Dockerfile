#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["NotificationSvc.API/NotificationSvc.API.csproj", "NotificationSvc.API/"]
RUN mkdir NotificationSvc.API/Filestore && touch NotificationSvc.API/Filestore/TempFile.txt
COPY ["NotificationSvc.Core/NotificationSvc.Core.csproj", "NotificationSvc.Core/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "NotificationSvc.API/NotificationSvc.API.csproj"
COPY . .
WORKDIR "/src/NotificationSvc.API"
RUN dotnet build "NotificationSvc.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationSvc.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationSvc.API.dll"]