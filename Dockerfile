# syntax=docker/dockerfile:1.6
# Build context: ExpoAppWeb/ (raiz da solution)

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os .csproj primeiro para aproveitar cache de restore
COPY ExpoApp.sln ./
COPY ExpoApp/ExpoApp.Api.csproj                                           ExpoApp/
COPY ExpoApp.Application/ExpoApp.Application.csproj                       ExpoApp.Application/
COPY ExpoApp.Auth.SDK/ExpoApp.Auth.SDK.csproj                             ExpoApp.Auth.SDK/
COPY ExpoApp.Domain/ExpoApp.Domain.csproj                                 ExpoApp.Domain/
COPY ExpoApp.Repository/ExpoApp.Repository.csproj                         ExpoApp.Repository/
COPY ExpoShared/ExpoShared.Application/ExpoShared.Application.csproj      ExpoShared/ExpoShared.Application/
COPY ExpoShared/ExpoShared.Domain/ExpoShared.Domain.csproj                ExpoShared/ExpoShared.Domain/
COPY ExpoShared/ExpoShared.Infrastructure/ExpoShared.Infrastructure.csproj ExpoShared/ExpoShared.Infrastructure/
COPY ExpoShared/ExpoShared.Repository/ExpoShared.Repository.csproj        ExpoShared/ExpoShared.Repository/

RUN dotnet restore ExpoApp/ExpoApp.Api.csproj

# Copia o resto do código
COPY . .

RUN dotnet publish ExpoApp/ExpoApp.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8081 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_gcServer=1

COPY --from=publish /app/publish ./

EXPOSE 8081

ENTRYPOINT ["dotnet", "ExpoApp.Api.dll"]
