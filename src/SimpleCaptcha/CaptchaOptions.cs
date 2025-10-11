namespace SimpleCaptcha;

public readonly struct CaptchaOptions
{
    public static CaptchaOptions Default => new();

    public CaptchaOptions()
    {
    }

    public int Width { get; init; } = 800;
    public int Height { get; init; } = 200;
    public int FontSize { get; init; } = 96;
    public int MinLineAmount { get; init; } = 10;
    public int MaxLineAmount { get; init; } = 13;
    public int SkewAmount { get; init; } = 20;
    public int RotationAmount { get; init; } = 30;
    public int BlurAmount { get; init; } = 2;
    public int SharpenAmount { get; init; } = 5;
    public bool ApplyVignette { get; init; } = true;
}
