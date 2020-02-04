FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY rover/rover.csproj rover/
RUN dotnet restore "rover/rover.csproj"
COPY . .
WORKDIR "/src/rover"
RUN dotnet build "rover.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "rover.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "rover.dll"]
