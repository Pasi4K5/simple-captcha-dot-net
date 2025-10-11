using SixLabors.ImageSharp;

namespace SimpleCaptcha;

public interface IColorFactory
{
    Color Next(Random random, bool transparent = false);
}
