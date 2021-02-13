FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY DrugTimer/Client/*.csproj DrugTimer/Client/
COPY DrugTimer/Server/*.csproj DrugTimer/Server/
COPY DrugTimer/Shared/*.csproj DrugTimer/Shared/
RUN dotnet restore DrugTimer/Server/DrugTimer.Server.csproj -r linux-musl-x64

COPY DrugTimer.sln ./
COPY DrugTimer/Client/ ./DrugTimer/Client/
COPY DrugTimer/Server/ ./DrugTimer/Server/
COPY DrugTimer/Shared/ ./DrugTimer/Shared/
RUN dotnet publish DrugTimer.sln -c release -o /app -r linux-musl-x64 --self-contained true --no-restore /p:PublishTrimmed=true

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine-amd64
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80
ENTRYPOINT [ "./DrugTimer.Server" ]