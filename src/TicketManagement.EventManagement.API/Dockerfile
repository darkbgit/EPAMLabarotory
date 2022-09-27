FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/TicketManagement.EventManagement.API/TicketManagement.EventManagement.API.csproj", "src/TicketManagement.EventManagement.API/"]
COPY ["src/TicketManagement.Core.Public/TicketManagement.Core.Public.csproj", "src/TicketManagement.Core.Public/"]
COPY ["src/TicketManagement.DataAccess.EF.Core/TicketManagement.DataAccess.EF.Core.csproj", "src/TicketManagement.DataAccess.EF.Core/"]
COPY ["src/TicketManagement.EventManagement.Services.Interfaces/TicketManagement.Core.EventManagement.Services.Interfaces.csproj", "src/TicketManagement.EventManagement.Services.Interfaces/"]
COPY ["src/TicketManagement.EventManagement.Services/TicketManagement.Core.EventManagement.Services.csproj", "src/TicketManagement.EventManagement.Services/"]
RUN dotnet restore "src/TicketManagement.EventManagement.API/TicketManagement.EventManagement.API.csproj"
COPY . .
WORKDIR "/src/src/TicketManagement.EventManagement.API"
RUN dotnet build "TicketManagement.EventManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketManagement.EventManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS test
WORKDIR /source/test

COPY test/TicketManagement.UnitTests/*.csproj .
RUN dotnet restore tests.csproj

COPY tests/ .
RUN dotnet build --no-restore

ENTRYPOINT ["dotnet", "test", "--logger:trx", "--no-restore", "--no-build"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketManagement.EventManagement.API.dll"]