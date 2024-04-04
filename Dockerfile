FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out /p:UseAppHost=false 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR "/app"
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "MediportaTask.dll"]