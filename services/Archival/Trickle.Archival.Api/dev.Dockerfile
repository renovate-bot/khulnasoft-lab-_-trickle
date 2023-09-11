# init base for Visual Studio debugging
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app

# init build
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

# restore API
WORKDIR /app
COPY SharedKernel/Trickle.SharedKernel.Domain.SeedWork/Trickle.SharedKernel.Domain.SeedWork.csproj SharedKernel/Trickle.SharedKernel.Domain.SeedWork/
COPY SharedKernel/Trickle.SharedKernel.Logging/Trickle.SharedKernel.Logging.csproj SharedKernel/Trickle.SharedKernel.Logging/
COPY Directory/Trickle.Directory.Api.Contracts/Trickle.Directory.Api.Contracts.csproj Directory/Trickle.Directory.Api.Contracts/
COPY Archival/Trickle.Archival.Domain/Trickle.Archival.Domain.csproj Archival/Trickle.Archival.Domain/
COPY Archival/Trickle.Archival.Infrastructure/Trickle.Archival.Infrastructure.csproj Archival/Trickle.Archival.Infrastructure/
COPY Archival/Trickle.Archival.Application/Trickle.Archival.Application.csproj Archival/Trickle.Archival.Application/
WORKDIR /app/Archival/Trickle.Archival.Api
COPY Archival/Trickle.Archival.Api/Trickle.Archival.Api.csproj .
RUN dotnet restore

# build API
WORKDIR /app
COPY /.editorconfig .
COPY SharedKernel/Trickle.SharedKernel.Domain.SeedWork/. SharedKernel/Trickle.SharedKernel.Domain.SeedWork/
COPY SharedKernel/Trickle.SharedKernel.Logging/. SharedKernel/Trickle.SharedKernel.Logging/
COPY Directory/Trickle.Directory.Api.Contracts/. Directory/Trickle.Directory.Api.Contracts/
COPY Archival/Trickle.Archival.Domain/. Archival/Trickle.Archival.Domain/
COPY Archival/Trickle.Archival.Infrastructure/. Archival/Trickle.Archival.Infrastructure/
COPY Archival/Trickle.Archival.Application/. Archival/Trickle.Archival.Application/
WORKDIR /app/Archival/Trickle.Archival.Api
COPY Archival/Trickle.Archival.Api/. .
RUN dotnet publish --no-restore -o /app/publish -r linux-musl-x64

# package final
FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ./Trickle.Archival.Api