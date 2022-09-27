FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/TicketManagement.OrderManagement.API/TicketManagement.OrderManagement.API.csproj", "src/TicketManagement.OrderManagement.API/"]
COPY ["src/TicketManagement.Core.Public/TicketManagement.Core.Public.csproj", "src/TicketManagement.Core.Public/"]
COPY ["src/TicketManagement.DataAccess.EF.Core/TicketManagement.DataAccess.EF.Core.csproj", "src/TicketManagement.DataAccess.EF.Core/"]
COPY ["src/TicketManagement.OrderManagement.Services.Interfaces/TicketManagement.Core.OrderManagement.Services.Interfaces.csproj", "src/TicketManagement.OrderManagement.Services.Interfaces/"]
COPY ["src/TicketManagement.OrderManagement.Services/TicketManagement.Core.OrderManagement.Services.csproj", "src/TicketManagement.OrderManagement.Services/"]
RUN dotnet restore "src/TicketManagement.OrderManagement.API/TicketManagement.OrderManagement.API.csproj"
COPY . .
WORKDIR "/src/src/TicketManagement.OrderManagement.API"
RUN dotnet build "TicketManagement.OrderManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketManagement.OrderManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketManagement.OrderManagement.API.dll"]