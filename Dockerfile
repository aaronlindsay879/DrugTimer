FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /source

COPY DrugTimer/Client/*.csproj DrugTimer/Client/
COPY DrugTimer/Server/*.csproj DrugTimer/Server/
COPY DrugTimer/Shared/*.csproj DrugTimer/Shared/
RUN dotnet restore DrugTimer/Server/DrugTimer.Server.csproj -r linux-musl-x64

COPY DrugTimer.sln ./
COPY DrugTimer/Client/ ./DrugTimer/Client/
COPY DrugTimer/Server/ ./DrugTimer/Server/
COPY DrugTimer/Shared/ ./DrugTimer/Shared/
RUN dotnet publish DrugTimer.sln -c release -o /app -r linux-musl-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80
ENTRYPOINT [ "./DrugTimer.Server" ]