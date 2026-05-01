FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/SpecPilot.Api/SpecPilot.Api.csproj", "src/SpecPilot.Api/"]
COPY ["src/SpecPilot.Application/SpecPilot.Application.csproj", "src/SpecPilot.Application/"]
COPY ["src/SpecPilot.Domain/SpecPilot.Domain.csproj", "src/SpecPilot.Domain/"]
COPY ["src/SpecPilot.Infrastructure/SpecPilot.Infrastructure.csproj", "src/SpecPilot.Infrastructure/"]

RUN dotnet restore "src/SpecPilot.Api/SpecPilot.Api.csproj"

COPY ["src/", "src/"]
WORKDIR /src/src/SpecPilot.Api
RUN dotnet publish "SpecPilot.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SpecPilot.Api.dll"]
