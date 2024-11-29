using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;

namespace AnastasiaHueApp.Util.Hue;

/// <summary>
/// Wrapper around the Philips Hue API. Makes all the REST api calls and either handles or delegates retrieval for the response.
/// </summary>
public interface IHueHandler
{
    // TODO: Rename all parameters with 'index' to 'id' to reflect meaning. 

    /// <summary>
    /// Tries to retrieve the username from a <see cref="UsernameResponse"/> to make other calls. Should be called before any other call is made. <br/>
    /// Note: if the physical button is not pressed an <see cref="ErrorResponse"/> should be returned!
    /// </summary>
    /// <returns>The username wrapped in a <see cref="UsernameResponse"/> or an <see cref="ErrorResponse"/>.</returns>
    public Task<Either<UsernameResponse, ErrorResponse>> AttemptLinkAsync();

    /// <summary>
    /// Retrieves all lights at once and returns it in a <see cref="List{HueLight}"/>, or returns an <see cref="ErrorResponse"/>.
    /// </summary>
    /// <returns>The <see cref="List{HueLight}"/> of lights or an <see cref="ErrorResponse"/>.</returns>
    public Task<Either<List<HueLight>, ErrorResponse>> GetLights();

    /// <summary>
    /// Retrieves information about one specific light with id <paramref name="index"/> 
    /// </summary>
    /// <param name="index">ID of the <see cref="HueLight"/>.</param>
    /// <returns>The light <see cref="HueLight"/> or an <see cref="ErrorResponse"/>.</returns>
    public Task<Either<HueLight, ErrorResponse>> GetLight(int index);

    /// <summary>
    /// Turns the light with id <paramref name="index"/> on or off. <br/>
    /// <b>On</b>=<see langword="true"/> <br/>
    /// <b>Off</b>=<see langword="false"/> <br/>
    /// </summary>
    /// <param name="index">ID of the <see cref="HueLight"/>.</param>
    /// <param name="on">Whether to turn the light on (<see langword="true"/>) or off (<see langword="false"/>).</param>
    /// <returns>An <see cref="ErrorResponse"/> or <see langword="null"/>.</returns>
    public Task<ErrorResponse?> LightSwitch(int index, bool on);

    /// <summary>
    /// Changes the hue, saturation and brightness of the light with id <paramref name="index"/> to the ones of <paramref name="color"/>.
    /// </summary>
    /// <param name="index">ID of the <see cref="HueLight"/>.</param>
    /// <param name="color">HS color to set the color to.</param>
    /// <returns>An <see cref="ErrorResponse"/> or <see langword="null"/>.</returns>
    public Task<ErrorResponse?> SetColorTo(int index, Color.Color color);

    /// <summary>
    /// Makes the light with id <paramref name="index"/> blink for 15 seconds.
    /// </summary>
    /// <param name="index">ID of the <see cref="HueLight"/>.</param>
    /// <returns>An <see cref="ErrorResponse"/> or <see langword="null"/>.</returns>
    public Task<ErrorResponse?> MakeLightBlink(int index);

    /// <summary>
    /// The light with id <paramref name="index"/> will cycle through all hues using the current brightness and saturation settings
    /// </summary>
    /// <param name="index">ID of the <see cref="HueLight"/>.</param>
    /// <returns>An <see cref="ErrorResponse"/> or <see langword="null"/>.</returns>
    public Task<ErrorResponse?> MakeLightColorLoop(int index);
}