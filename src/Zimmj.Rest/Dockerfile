FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Zimmj.Presentation/Zimmj.Presentation.csproj", "src/Zimmj.Presentation/"]
RUN dotnet restore "src/Zimmj.Presentation/Zimmj.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Zimmj.Presentation"
RUN dotnet build "Zimmj.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zimmj.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Zimmj.Presentation.dll"]
