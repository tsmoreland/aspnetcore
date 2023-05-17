#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Admin/FrontEnd/App/SunDoeCoffeeShop.Admin.FrontEnd.App.csproj", "src/Admin/FrontEnd/App/"]
COPY ["src/Shared/AuthPersistence/SunDoeCoffeeShop.Shared.AuthPersistence.csproj", "src/Shared/AuthPersistence/"]
COPY ["src/Shared/Roles/SunDoeCoffeeShop.Shared.Roles.csproj", "src/Shared/Roles/"]
RUN dotnet restore "src/Admin/FrontEnd/App/SunDoeCoffeeShop.Admin.FrontEnd.App.csproj"
COPY . .
WORKDIR "/src/src/Admin/FrontEnd/App"
RUN dotnet build "SunDoeCoffeeShop.Admin.FrontEnd.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SunDoeCoffeeShop.Admin.FrontEnd.App.csproj" -c Release -o /app/publish /p:UseAppHost=false
COPY ["data/admin-certificate.pfx", "/app/admin-certificate.pfx"]

FROM base AS final
WORKDIR /certs
COPY --from=publish /app/admin-certificate.pfx .
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SunDoeCoffeeShop.Admin.FrontEnd.App.dll"]
