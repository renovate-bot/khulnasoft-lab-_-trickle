﻿namespace Trickle.SharedKernel.Logging.Options;

internal record ApplicationInsightsOptions
{
    public const string Key = "ApplicationInsights";

    public string ServerTelemetryChannelStoragePath { get; init; } = default!;
}
