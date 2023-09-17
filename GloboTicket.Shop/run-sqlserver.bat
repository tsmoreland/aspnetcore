@echo off

docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=%1' -e 'CONFIG_EDGE_BUILD=0' -p 1433:1433 --name sql-concert -h sql-concert -v sqlserverdata:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2019-latest
