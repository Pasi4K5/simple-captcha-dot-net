using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SimpleCaptcha;

public static class CharacterImageFactory
{
    private static readonly Rgba32 TransparentPixel = new(0, 0, 0, 0);

    public static Image<Rgba32> Create(Font font, char charToDraw, Color color)
    {
        var size = (int) Math.Ceiling(font.Size);
        var image = new Image<Rgba32>(size, size);
        image.Mutate(img => img.DrawText(charToDraw.ToString(), font, color, PointF.Empty));

        var usedArea = image.Bounds;

        new List<Side>
        {
            Side.Left,
            Side.Right,
            Side.Bottom,
            Side.Top
        }.ForEach(side => TrimRectSide(image, ref usedArea, side));

        image.Mutate(img => img.Crop(usedArea));

        return image;
    }

    private static void TrimRectSide(Image<Rgba32> img, ref Rectangle rect, Side side)
    {
        var isVertical = ((int)side & (int)SideType.Vertical) != 0;
        var isMin = ((int)side & (int)SideType.Min) != 0;

        while (true)
        {
            var iStart = isVertical ? rect.X : rect.Y;
            var iEnd = isVertical ? rect.Right : rect.Bottom;
            var fixedIdx = side switch
            {
                Side.Top => rect.Top,
                Side.Bottom => rect.Bottom - 1,
                Side.Left => rect.Left,
                Side.Right => rect.Right - 1,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };

            if (Enumerable.Range(iStart, iEnd).Select(i => img[isVertical ? i : fixedIdx, isVertical ? fixedIdx : i]).Any(c => c != TransparentPixel))
            {
                return;
            }

            if (isVertical)
            {
                rect.Height--;

                if (isMin)
                {
                    rect.Y++;
                }
            }
            else
            {
                rect.Width--;

                if (isMin)
                {
                    rect.X++;
                }
            }
        }
    }

    private enum Side
    {
        Top = 0b1,
        Bottom = 0b10,
        Left = 0b100,
        Right = 0b1000,
    }

    private enum SideType
    {
        Vertical = Side.Top | Side.Bottom,
        Min = Side.Top | Side.Left,
    }
}
