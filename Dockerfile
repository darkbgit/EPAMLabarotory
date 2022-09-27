FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# RUN apt-get update
# RUN apt-get install -y curl

#RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_18.x | bash -
RUN apt-get install -y nodejs
# RUN apt-get update
# RUN apt-get install -y curl
# RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
# RUN apt-get install -y nodejs

# copy csproj and restore as distinct layers
COPY src/TicketManagement.MVC/*.csproj src/TicketManagement.MVC/
# COPY src/TicketManagement.EventManagement.API/*.csproj src/TicketManagement.EventManagement.API/
# COPY src/TicketManagement.UserManagement.API/*.csproj src/TicketManagement.UserManagement.API/
# COPY src/TicketManagement.OrderManagement.API/*.csproj src/TicketManagement.OrderManagement.API/
COPY src/TicketManagement.Core.Public/*.csproj src/TicketManagement.Core.Public/
COPY src/TicketManagement.DataAccess.EF.Core/*.csproj src/TicketManagement.DataAccess.EF.Core/
RUN dotnet restore src/TicketManagement.MVC/TicketManagement.MVC.csproj

# copy and build app and libraries
COPY src/TicketManagement.MVC src/TicketManagement.MVC/
# COPY src/TicketManagement.EventManagement.API/ src/TicketManagement.EventManagement.API/
# COPY src/TicketManagement.UserManagement.API/ src/TicketManagement.UserManagement.API/
# COPY src/TicketManagement.OrderManagement.API/ src/TicketManagement.OrderManagement.API/
COPY src/TicketManagement.Core.Public/ src/TicketManagement.Core.Public/
COPY src/TicketManagement.DataAccess.EF.Core/ src/TicketManagement.DataAccess.EF.Core/
WORKDIR /source/src/TicketManagement.MVC
RUN dotnet build -c Release -o /app/build

# test stage -- exposes optional entrypoint
# target entrypoint with: docker build --target test
# FROM build AS test
# WORKDIR /source/test

# COPY test/TicketManagement.UnitTests/*.csproj TicketManagement.UnitTests/
# RUN dotnet restore TicketManagement.UnitTests/TicketManagement.UnitTests/.csproj

# COPY test/TicketManagement.UnitTests/ test/TicketManagement.UnitTests/
# WORKDIR /source/test/TicketManagement.UnitTests
# RUN dotnet build --no-restore

# ENTRYPOINT ["dotnet", "test", "--logger:trx", "--no-restore", "--no-build"]

FROM build AS publish
# RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
# RUN apt-get install -y nodejs

RUN dotnet publish -c Release -o /app/publish 

# final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketManagement.MVC.dll"]

# # https://hub.docker.com/_/microsoft-dotnet
# FROM mcr.microsoft.com/dotnet/sdk:5.0.103 AS build
# WORKDIR /build

# RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
# RUN apt-get install -y nodejs

# # copy csproj and restore as distinct layers
# COPY ./*.csproj .
# RUN dotnet restore

# # copy everything else and build app
# COPY . .
# WORKDIR /build
# RUN dotnet publish -c release -o published --no-cache

# # final stage/image
# FROM mcr.microsoft.com/dotnet/aspnet:5.0
# WORKDIR /app
# COPY --from=build /build/published ./
# ENTRYPOINT ["dotnet", "react-dotnet-example.dll"]

# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src
# COPY ["Project1/Project1.csproj", "Project1/"]
# RUN dotnet restore "Project1/Project1.csproj"
# COPY . .
# WORKDIR "/src/Project1"
# RUN dotnet build "Project1.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "Project1.csproj" -c Release -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "Project1.dll"]