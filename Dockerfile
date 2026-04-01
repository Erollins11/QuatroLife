FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY 44Life.sln ./
COPY src/Hotel.Web/Hotel.Web.csproj src/Hotel.Web/
RUN dotnet restore 44Life.sln

COPY . .
RUN dotnet publish src/Hotel.Web/Hotel.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

CMD ["sh", "-c", "dotnet Hotel.Web.dll --urls http://0.0.0.0:${PORT:-10000}"]
