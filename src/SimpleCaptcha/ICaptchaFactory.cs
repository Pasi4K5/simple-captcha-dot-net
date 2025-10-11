using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SimpleCaptcha;

public interface ICaptchaFactory
{
    Image<Rgba32> Next();
}
