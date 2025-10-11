using System;
using SixLabors.ImageSharp;

namespace SimpleCaptchaDotNet;

public interface IColorFactory
{
    Color Next(Random random, bool transparent = false);
}
