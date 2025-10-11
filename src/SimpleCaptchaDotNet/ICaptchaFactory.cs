using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SimpleCaptchaDotNet;

public interface ICaptchaFactory
{
    Image<Rgba32> Next();
}
