using System;
using System.Linq;
using SixLabors.ImageSharp;

namespace SimpleCaptchaDotNet;

public class CaptchaColorFactory : IColorFactory
{
    public Color Next(Random random, bool transparent = false)
    {
        const int nonDominantMax = 91;
        const int dominantMin = 128;
        const int dominantMax = 256;
        const int minTransparency = 64;
        const int maxTransparency = 127;

        var colors = Enumerable.Range(0, 3).Select(_ => (byte)random.Next(nonDominantMax)).ToArray();
        var dominantColor = random.Next(3);
        colors[dominantColor] = (byte)random.Next(dominantMin, dominantMax);

        return Color.FromRgba(
            colors[0],
            colors[1],
            colors[2],
            transparent ? (byte)random.Next(minTransparency, maxTransparency) : (byte)255
        );
    }
}
