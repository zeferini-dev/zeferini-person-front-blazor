FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/ ./

RUN dotnet restore zeferini-person-front-blazor/zeferini-person-front-blazor.sln

WORKDIR /src/zeferini-person-front-blazor
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "zeferini-person-front-blazor.dll"]