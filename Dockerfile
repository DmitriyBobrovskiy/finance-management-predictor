FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /app
COPY finance-management-backend.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build "finance-management-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "finance-management-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "finance-management-backend.dll"]