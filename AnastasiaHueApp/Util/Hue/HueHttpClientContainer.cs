namespace AnastasiaHueApp.Util.Hue;

public class HueHttpClientContainer : IHttpClientContainer
{
    private static readonly HttpClient ContainedClient = new()
    {
#if ANDROID
        BaseAddress =
 new Uri("http://10.0.2.2/api/"), // NOTE: Emulator. If using port 80 no port needs to be specified.
        //BaseAddress = new Uri("http://192.168.1.179/api/"), // NOTE: Hardware. If using port 80 no port needs to be specified
#elif WINDOWS
        BaseAddress =
 new Uri("http://localhost/api/"), // NOTE: Emulator. If using port 80 no port needs to be specified.
        //BaseAddress = new Uri("http://192.168.1.179/api/"), // NOTE: Hardware. If using port 80 no port needs to be specified.
#else

#endif
    };

    /// <inheritdoc />
    public HttpClient HttpClient => ContainedClient;
}