# SimpleCaptchaDotNet

A simple .NET library that generates CAPTCHA images.

## Installation

```shell
dotnet add package SimpleCaptchaDotNet --version 1.1.1
```

## Example

![Example CAPTCHA](img/captcha.png)

## Usage

### Basic

```csharp
var captchaFactory = new CaptchaFactory();
var captcha = captchaFactory.Next();
await captcha.Image.SaveAsPngAsync("captcha.png");

Console.Write("Enter CAPTCHA code: ");

Console.WriteLine(
    Console.ReadLine() == captcha.Text
        ? "Correct."
        : "Incorrect."
);
```

### Customization

```csharp
var fontStream = Assembly.GetExecutingAssembly()
    .GetManifestResourceStream("MyProject.Resources.MyCustomFont.ttf");

if (fontStream is null)
{
    Console.WriteLine("Font not found.");
    return;
}

var captchaFactory = new CaptchaFactory(
    new CaptchaOptions 
    {
        Width = 1000,
        Height = 300,
        FontSize = 110,
        MinLineAmount = 8,
        MaxLineAmount = 15,
        SkewAmount = 0,
        RotationAmount = 25,
        BlurAmount = 3,
        SharpenAmount = 4,
        ApplyVignette = false,
    },
    // You can also inject your own factory by implementing IPhraseFactory.
    new CaptchaPhraseFactory
    {
        Characters = "0123456789",
        Length = 5,
    },
    new MyColorFactory(),
    fontStream
);

// Use captchaFactory...

return;

public class MyColorFactory : IColorFactory
{
    public Color Next(Random random, bool transparent = false) => Color.Red;
}
```
