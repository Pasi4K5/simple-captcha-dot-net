using System;
using System.IO;
using System.Reflection;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SimpleCaptcha;

public sealed class CaptchaFactory : ICaptchaFactory
{
    private static readonly CaptchaPhraseFactory DefaultPhraseFactory = new();
    private static readonly CaptchaColorFactory DefaultColorFactory = new();

    private readonly IPhraseFactory _phraseGen;
    private readonly IColorFactory _colorFactory;
    private readonly CaptchaOptions _opt;
    private readonly Random _rng;
    private readonly Font _font;

    public CaptchaFactory(
        CaptchaOptions? options = null,
        IPhraseFactory? phraseGenerator = null,
        IColorFactory? colorFactory = null,
        Random? random = null,
        Stream? fontStream = null
    )
    {
        _phraseGen = phraseGenerator ?? DefaultPhraseFactory;
        _colorFactory = colorFactory ?? DefaultColorFactory;
        _opt = options ?? CaptchaOptions.Default;
        _rng = random ?? new();

        fontStream ??= Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("SimpleCaptcha.Resources.MomsTypewriter.ttf")
            ?? throw new InvalidOperationException("Could not find embedded font resource.");

        _font = new FontCollection().Add(fontStream).CreateFont(_opt.FontSize);
    }

    public Image<Rgba32> Next()
    {
        var image = new Image<Rgba32>(_opt.Width, _opt.Height);

        image.Mutate(img =>
        {
            img.BackgroundColor(Color.Gray);
            var phrase = _phraseGen.Next();

            for (var i = 0; i < phrase.Length; i++)
            {
                var j = i;
                using var textImg = CharacterImageFactory.Create(_font, phrase[i], _colorFactory.Next(_rng));

                textImg.Mutate(t =>
                {
                    t.Skew(
                        _opt.SkewAmount * (float)(_rng.NextDouble() - 0.5),
                        _opt.SkewAmount * (float)(_rng.NextDouble() - 0.5)
                    );

                    t.Rotate(_opt.RotationAmount * (float)(_rng.NextDouble() - 0.5f));
                });

                var targetCenterPos = new Point(
                    (j + 1) * image.Width / (phrase.Length + 1),
                    image.Height / 2
                );
                var targetPos = new Point(
                    targetCenterPos.X - textImg.Width / 2,
                    targetCenterPos.Y - textImg.Height / 2
                );

                img.DrawImage(textImg, targetPos, 1);
            }

            var numLines = _rng.Next(_opt.MinLineAmount, _opt.MaxLineAmount + 1);

            for (var i = 0; i < Math.Ceiling((double)numLines / 2); i++)
            {
                // Vertical
                var x1 = _rng.Next(0, _opt.Width);
                var x2 = _rng.Next(0, _opt.Width);
                img.DrawLine(_colorFactory.Next(_rng, transparent: true), _rng.Next(5, 7), new(x1, 0), new(x2, _opt.Height));

                if (i >= numLines / 2)
                {
                    break;
                }

                // Horizontal
                var y1 = _rng.Next(0, _opt.Height);
                var y2 = _rng.Next(0, _opt.Height);
                img.DrawLine(_colorFactory.Next(_rng, transparent: true), _rng.Next(5, 7), new(0, y1), new(_opt.Width, y2));
            }

            img.MedianBlur(_opt.BlurAmount, preserveAlpha: false);
            img.BoxBlur();
            img.GaussianSharpen(_opt.SharpenAmount);

            if (_opt.ApplyVignette)
            {
                img.Vignette(Color.Black);
            }
        });

        return image;
    }
}
