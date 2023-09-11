# init base for Visual Studio debugging
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

# init build
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

# restore API
WORKDIR /app
COPY SharedKernel/Trickle.SharedKernel.Domain.SeedWork/Trickle.SharedKernel.Domain.SeedWork.csproj SharedKernel/Trickle.SharedKernel.Domain.SeedWork/
COPY SharedKernel/Trickle.SharedKernel.Logging/Trickle.SharedKernel.Logging.csproj SharedKernel/Trickle.SharedKernel.Logging/
COPY Directory/Trickle.Directory.Domain/Trickle.Directory.Domain.csproj Directory/Trickle.Directory.Domain/
COPY Directory/Trickle.Directory.Infrastructure/Trickle.Directory.Infrastructure.csproj Directory/Trickle.Directory.Infrastructure/
COPY Directory/Trickle.Directory.Infrastructure.Migrations/Trickle.Directory.Infrastructure.Migrations.csproj Directory/Trickle.Directory.Infrastructure.Migrations/
COPY Directory/Trickle.Directory.Api.Contracts/Trickle.Directory.Api.Contracts.csproj Directory/Trickle.Directory.Api.Contracts/
COPY Directory/Trickle.Directory.Application/Trickle.Directory.Application.csproj Directory/Trickle.Directory.Application/
WORKDIR /app/Directory/Trickle.Directory.Api
COPY Directory/Trickle.Directory.Api/Trickle.Directory.Api.csproj .
RUN dotnet restore

# build API
WORKDIR /app
COPY /.editorconfig .
COPY SharedKernel/Trickle.SharedKernel.Domain.SeedWork/. SharedKernel/Trickle.SharedKernel.Domain.SeedWork/
COPY SharedKernel/Trickle.SharedKernel.Logging/. SharedKernel/Trickle.SharedKernel.Logging/
COPY Directory/Trickle.Directory.Domain/. Directory/Trickle.Directory.Domain/
COPY Directory/Trickle.Directory.Infrastructure/. Directory/Trickle.Directory.Infrastructure/
COPY Directory/Trickle.Directory.Infrastructure.Migrations/. Directory/Trickle.Directory.Infrastructure.Migrations/
COPY Directory/Trickle.Directory.Api.Contracts/. Directory/Trickle.Directory.Api.Contracts/
COPY Directory/Trickle.Directory.Application/. Directory/Trickle.Directory.Application/
WORKDIR /app/Directory/Trickle.Directory.Api
COPY Directory/Trickle.Directory.Api/. .
RUN dotnet publish --no-restore -o /app/publish -r linux-musl-x64

# package final
FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ./Trickle.Directory.Api