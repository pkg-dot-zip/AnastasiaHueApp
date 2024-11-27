using System.Diagnostics;
using AnastasiaHueApp.Util.Json;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace AnastasiaHueApp.Util.Extensions;

/// <summary>
/// Extension methods for the <see cref="HttpContent"/> class.
/// </summary>
public static class HttpContentExtensions
{
    // Only here for readability reasons for now.
    // TODO: Verify json here. If not of Json format throw exception.
    public static async Task<string> ReadAsJsonString(this HttpContent content) => await content.ReadAsStringAsync();

    public static async Task<T?> ReadAsOrNullAsync<T>(this HttpContent content, IJsonRegistry registry, [CallerMemberName] string caller = "")
    {
        Debug.Write($"{nameof(ReadAsOrNullAsync)} called by {caller}");
        var json = await content.ReadAsJsonString();
        return registry.Parse<T>(json);
    }

    public static async Task<Either<T1, T2>> ReadAsEitherAsync<T1, T2>(this HttpContent content, IJsonRegistry registry, [CallerMemberName] string caller = "")
    {
        Debug.Write($"{nameof(ReadAsEitherAsync)} called by {caller}");
        var json = await content.ReadAsJsonString();
        var value1 = registry.Parse<T1>(json);
        var value2 = registry.Parse<T2>(json);
        return new Either<T1, T2>(value1, value2);
    }
}