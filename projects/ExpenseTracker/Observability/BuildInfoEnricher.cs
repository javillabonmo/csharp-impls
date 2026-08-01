using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace ExpenseTracker.Observability;

/// <summary>
/// Enriches log events with build metadata read directly from the assembly.
/// Values are baked in at compile time via MSBuild properties in the .csproj,
/// so no environment variables or external configuration is required.
/// </summary>
public class BuildInfoEnricher : ILogEventEnricher
{
    private readonly string _buildVersion;
    private readonly string _commitHash;

    public BuildInfoEnricher()
    {
        var assembly = typeof(BuildInfoEnricher).Assembly;
        var informationalVersion = assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        // InformationalVersion is formatted as "1.0.0+35212b3" by our .csproj
        if (informationalVersion is not null)
        {
            var plusIndex = informationalVersion.IndexOf('+');
            if (plusIndex > 0)
            {
                _buildVersion = informationalVersion[..plusIndex];
                _commitHash = informationalVersion[(plusIndex + 1)..];
            }
            else
            {
                _buildVersion = informationalVersion;
                _commitHash = "unknown";
            }
        }
        else
        {
            _buildVersion = assembly.GetName().Version?.ToString() ?? "unknown";
            _commitHash = "unknown";
        }
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("BuildVersion", _buildVersion));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CommitHash", _commitHash));
    }
}
