FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/TicketManagement.Database.Build/TicketManagement.Database.Build.csproj", "src/TicketManagement.Database.Build/"]
RUN dotnet restore "src/TicketManagement.Database.Build/TicketManagement.Database.Build.csproj"
COPY ["src/TicketManagement.Database/", "src/TicketManagement.Database/"]
WORKDIR "/src/src/TicketManagement.Database.Build"
RUN dotnet build "TicketManagement.Database.Build.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/mssql/server:2019-latest

# Elevate to root to install required packages
USER root

RUN apt-get update \
    && apt-get install unzip libunwind8 libicu66 libssl-dev -y


# Install SQLPackage for Linux and make it executable
RUN wget -progress=bar:force -q -O sqlpackage.zip https://aka.ms/sqlpackage-linux \
    && unzip -qq sqlpackage.zip -d /opt/sqlpackage \
    && chmod +x /opt/sqlpackage/sqlpackage \
    && chown -R mssql /opt/sqlpackage \
    && mkdir /tmp/db \
    && chown -R mssql /tmp/db

# Add the DACPAC to the image
COPY  --from=build /app/build/TicketManagement.Database.Build.dacpac /tmp/db/db.dacpac

# Configure external build arguments to allow configurability.
ARG DBNAME=mydatabase
ARG PASSWORD

# Configure the required environmental variables
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=Admin!1admin

# Launch SQL Server, confirm startup is complete, deploy the DACPAC, then terminate SQL Server.
# See https://stackoverflow.com/a/51589787/488695
RUN ( /opt/mssql/bin/sqlservr & ) | grep -q "Service Broker manager has started" \
    && /opt/sqlpackage/sqlpackage /a:Publish /tsn:localhost /tdn:${DBNAME} /tu:sa /tp:$SA_PASSWORD /sf:/tmp/db/db.dacpac \
    && rm -r /tmp/db \
    && pkill sqlservr \
    && rm -r /opt/sqlpackage