FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["EnterpriseIdentity_Auth/EnterpriseIdentity_Auth.csproj", "EnterpriseIdentity_Auth/"]

RUN dotnet restore "EnterpriseIdentity_Auth/EnterpriseIdentity_Auth.csproj"

COPY . .

WORKDIR /src/EnterpriseIdentity_Auth

RUN dotnet publish "EnterpriseIdentity_Auth.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

COPY --from=build /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "EnterpriseIdentity_Auth.dll"]
