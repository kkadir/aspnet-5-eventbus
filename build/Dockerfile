FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bussy.Server/Bussy.Server.csproj", "Bussy.Server/"]
RUN dotnet restore "Bussy.Server/Bussy.Server.csproj"
COPY . .
WORKDIR "/src/Bussy.Server"
RUN dotnet build "Bussy.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bussy.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bussy.Server.dll"]
