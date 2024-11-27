using AnastasiaHueApp.Models;
using AnastasiaHueApp.Models.Message;

namespace AnastasiaHueApp.Util.Hue;

public interface IHueHandler
{
    public Task<Either<UsernameResponse, ErrorResponse>> AttemptLinkAsync();
    public Task<Either<List<HueLight>, ErrorResponse>> GetLights();
    public Task<Either<HueLight, ErrorResponse>> GetLight(int index);
    public Task<ErrorResponse?> LightSwitch(int index, bool on);
    public Task<ErrorResponse?> SetColorTo(int index, Color.Color color);
    public Task<ErrorResponse?> MakeLightBlink(int index);
}