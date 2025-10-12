using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SimpleCaptchaDotNet;

public record Captcha(Image<Rgba32> Image, string Text);
