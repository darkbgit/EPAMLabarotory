FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
RUN curl -sL https://deb.nodesource.com/setup_18.x | bash -
RUN apt-get install -y nodejs
COPY ["nuget.config", "."]
COPY ["src/TicketManagement.MVC/TicketManagement.MVC.csproj", "src/TicketManagement.MVC/"]
COPY ["src/TicketManagement.Core.Public/TicketManagement.Core.Public.csproj", "src/TicketManagement.Core.Public/"]
COPY ["src/TicketManagement.DataAccess.EF.Core/TicketManagement.DataAccess.EF.Core.csproj", "src/TicketManagement.DataAccess.EF.Core/"]
RUN dotnet restore "src/TicketManagement.MVC/TicketManagement.MVC.csproj"
COPY . .
WORKDIR "/src/src/TicketManagement.MVC"
RUN dotnet build "TicketManagement.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketManagement.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketManagement.MVC.dll"]