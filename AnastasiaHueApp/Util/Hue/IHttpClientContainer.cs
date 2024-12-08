namespace AnastasiaHueApp.Util.Hue;

public interface IHttpClientContainer
{
    /// <summary>
    /// The <see cref="HttpClient"/> to be used in this software.
    /// When retrieving this <see cref="HttpClient"/> it is already configured to make the correct api calls.
    /// </summary>
    public HttpClient HttpClient { get; }
}