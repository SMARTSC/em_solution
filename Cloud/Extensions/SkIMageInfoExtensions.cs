namespace EM.Cloud.Extensions
{
    public static class SKImageInfoExtensions
    {
        public static SkiaSharp.SKImageInfo LandscapeScale(this SkiaSharp.SKImageInfo source, int desiredWidth)
        {
            return new SkiaSharp.SKImageInfo(desiredWidth, (int)System.Math.Round(source.Height * desiredWidth / (double)source.Width));
        }

        public static SkiaSharp.SKImageInfo PortraitScale(this SkiaSharp.SKImageInfo source, int desiredHeight)
        {
            return new SkiaSharp.SKImageInfo((int)System.Math.Round(source.Width * desiredHeight / (double)source.Height), desiredHeight);
        }
    }
}
