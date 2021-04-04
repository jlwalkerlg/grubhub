FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /build

# copy csproj and restore
COPY GrubHub.sln ./
COPY src/Web/Web.csproj ./src/Web/
RUN dotnet restore src/Web

# copy source code and build app
COPY settings*.json ./
COPY src/Web/. ./src/Web/
WORKDIR /build/src/Web
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "Web.dll"]
