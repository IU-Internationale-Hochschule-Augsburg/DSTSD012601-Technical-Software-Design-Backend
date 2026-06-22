FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
# Von Npgsql für die Aushandlung der DB-Verbindung benötigt (GSSAPI/Kerberos).
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Subscription_Control_Backend.csproj", "./"]
RUN dotnet restore "Subscription_Control_Backend.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./Subscription_Control_Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Subscription_Control_Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Subscription_Control_Backend.dll"]
