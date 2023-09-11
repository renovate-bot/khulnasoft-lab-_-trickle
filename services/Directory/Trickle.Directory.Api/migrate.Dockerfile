# init test-migrations
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS test-migrations
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

# restore
WORKDIR /app
COPY SharedKernel/Trickle.SharedKernel.Domain.SeedWork/Trickle.SharedKernel.Domain.SeedWork.csproj SharedKernel/Trickle.SharedKernel.Domain.SeedWork/
COPY SharedKernel/Trickle.SharedKernel.Logging/Trickle.SharedKernel.Logging.csproj SharedKernel/Trickle.SharedKernel.Logging/
COPY Directory/Trickle.Directory.Domain/Trickle.Directory.Domain.csproj Directory/Trickle.Directory.Domain/
COPY Directory/Trickle.Directory.Infrastructure/Trickle.Directory.Infrastructure.csproj Directory/Trickle.Directory.Infrastructure/
COPY Directory/Trickle.Directory.Infrastructure.Migrations/Trickle.Directory.Infrastructure.Migrations.csproj Directory/Trickle.Directory.Infrastructure.Migrations/
COPY Directory/Trickle.Directory.Api.Contracts/Trickle.Directory.Api.Contracts.csproj Directory/Trickle.Directory.Api.Contracts/
COPY Directory/Trickle.Directory.Application/Trickle.Directory.Application.csproj Directory/Trickle.Directory.Application/
COPY Directory/Trickle.Directory.Api/Trickle.Directory.Api.csproj Directory/Trickle.Directory.Api/
WORKDIR /app/Directory/Trickle.Directory.Infrastructure.Migrations.Tests
COPY Directory/Trickle.Directory.Infrastructure.Migrations.Tests/Trickle.Directory.Infrastructure.Migrations.Tests.csproj .
RUN dotnet restore

# build
WORKDIR /app
COPY SharedKernel/Trickle.SharedKernel.Domain.SeedWork/. SharedKernel/Trickle.SharedKernel.Domain.SeedWork/
COPY SharedKernel/Trickle.SharedKernel.Logging/. SharedKernel/Trickle.SharedKernel.Logging/
COPY Directory/Trickle.Directory.Domain/. Directory/Trickle.Directory.Domain/
COPY Directory/Trickle.Directory.Infrastructure/. Directory/Trickle.Directory.Infrastructure/
COPY Directory/Trickle.Directory.Infrastructure.Migrations/. Directory/Trickle.Directory.Infrastructure.Migrations/
COPY Directory/Trickle.Directory.Api.Contracts/. Directory/Trickle.Directory.Api.Contracts/
COPY Directory/Trickle.Directory.Application/. Directory/Trickle.Directory.Application/
COPY Directory/Trickle.Directory.Api/. Directory/Trickle.Directory.Api/
WORKDIR /app/Directory/Trickle.Directory.Infrastructure.Migrations.Tests
COPY Directory/Trickle.Directory.Infrastructure.Migrations.Tests/. .
RUN dotnet build -c Release --no-restore -p:TreatWarningsAsErrors=true -r linux-musl-x64 --self-contained true