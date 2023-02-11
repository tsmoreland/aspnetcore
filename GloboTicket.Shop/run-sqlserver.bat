@echo off

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=%1" -v sqlserverdata:/var/opt/mssql  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
