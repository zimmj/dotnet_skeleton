FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "src/Zimmj.Bootstrap/Zimmj.Bootstrap.csproj" --runtime linux-musl-x64


FROM build-env as testrunner

WORKDIR "/App"
CMD ["dotnet", "test", "--logger:trx"]

FROM build-env as publish

RUN dotnet publish "src/Zimmj.Bootstrap/Zimmj.Bootstrap.csproj" -c Release -o /App/publish \
    --no-restore \
    --self-contained true \
    --runtime linux-musl-x64 \
    /p:PublishTrimmed=true \
    /p:PublishSingleFile=true

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app

USER dotnetuser
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /App
COPY --from=publish /App/publish .
ENTRYPOINT ["./Zimmj.Bootstrap"]