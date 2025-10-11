namespace SimpleCaptcha;

public sealed class CaptchaPhraseFactory : IPhraseFactory
{
    public int Length { get; init; } = 6;
    public string Characters { get; init; } = "abcdefghijkmnopqrstuvwxyzABCDEFGHIJKLMNPQRTSUVWXYZ23456789";

    private readonly Random _random;

    public CaptchaPhraseFactory() : this(new())
    {
    }

    public CaptchaPhraseFactory(Random random)
    {
        _random = random;
    }

    public string Next() =>
        new(Enumerable.Range(0, Length)
            .Select(_ => Characters[_random.Next(Characters.Length)])
            .ToArray());
}
