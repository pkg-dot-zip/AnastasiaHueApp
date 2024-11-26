using System.Diagnostics.CodeAnalysis;

namespace AnastasiaHueApp.Util.Json;

public interface IJsonRegistry
{
    public void Register<T>(Func<string, T> parser);
    public T? Parse<T>([StringSyntax(StringSyntaxAttribute.Json)] string json);
}