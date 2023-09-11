# init base
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine AS base
LABEL org.opencontainers.image.description="trickle.khulnasoft.com | github.com/khulnasoft-lab/trickle | An ASP.NET Core API archiving and serving copies of Trickle for mirrors and analysis."
WORKDIR /app
EXPOSE 80

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

# TODO: enable PublishTrimmed in csproj file
# TODO: enable PublishReadyToRun
RUN dotnet publish -c Release -p:DebugType=None -p:DebugSymbols=false --no-restore -o /app/publish -p:TreatWarningsAsErrors=true -r linux-musl-x64 --self-contained true -p:PublishSingleFile=true

# package final
FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ./Trickle.Archival.Api