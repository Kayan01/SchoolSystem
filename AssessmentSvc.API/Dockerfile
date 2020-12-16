#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AssessmentSvc.API/AssessmentSvc.API.csproj", "AssessmentSvc.API/"]
RUN mkdir AssessmentSvc.API/FileStore && touch AssessmentSvc.API/FileStore/TempFile.txt
COPY ["AssessmentSvc.Core/AssessmentSvc.Core.csproj", "AssessmentSvc.Core/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "AssessmentSvc.API/AssessmentSvc.API.csproj"
COPY . .
WORKDIR "/src/AssessmentSvc.API"
RUN dotnet build "AssessmentSvc.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AssessmentSvc.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt install libc6-dev
ENTRYPOINT ["dotnet", "AssessmentSvc.API.dll"]