FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/TicketManagement.UserManagement.API/TicketManagement.UserManagement.API.csproj", "src/TicketManagement.UserManagement.API/"]
COPY ["src/TicketManagement.Core.Public/TicketManagement.Core.Public.csproj", "src/TicketManagement.Core.Public/"]
COPY ["src/TicketManagement.DataAccess.EF.Core/TicketManagement.DataAccess.EF.Core.csproj", "src/TicketManagement.DataAccess.EF.Core/"]
COPY ["src/TicketManagement.UserManagement.Services.Interfaces/TicketManagement.Core.UserManagement.Services.Interfaces.csproj", "src/TicketManagement.UserManagement.Services.Interfaces/"]
COPY ["src/TicketManagement.UserManagement.Services/TicketManagement.Core.UserManagement.Services.csproj", "src/TicketManagement.UserManagement.Services/"]
RUN dotnet restore "src/TicketManagement.UserManagement.API/TicketManagement.UserManagement.API.csproj"
COPY . .
WORKDIR "/src/src/TicketManagement.UserManagement.API"
RUN dotnet build "TicketManagement.UserManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketManagement.UserManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketManagement.UserManagement.API.dll"]