using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SimpleCaptchaDotNet;

public sealed record Captcha(Image<Rgba32> Image, string Text) : IDisposable
{
    ~Captcha()
    {
        Dispose();
    }

    public void Dispose()
    {
        Image.Dispose();
        GC.SuppressFinalize(this);
    }
}
