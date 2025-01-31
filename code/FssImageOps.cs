using System.IO;
using SkiaSharp;

namespace SymbolGen
{
    public static class FssImageOps
    {
        // Usage: SKBitmap bitmap = FssImageOps.CreateTransparentImage(256, 256);
        public static SKBitmap CreateTransparentImage(int width, int height)
        {
            var bitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Transparent);
            }
            return bitmap;
        }

        // ----------------------------------------------------------------------------------------

        public static SKBitmap LoadImage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Image file not found at: {filePath}");
            }
            using var stream = File.OpenRead(filePath);
            return SKBitmap.Decode(stream);
        }

        // ----------------------------------------------------------------------------------------

        public static void SaveImage(SKBitmap bitmap, string filePath, int quality = 100)
        {
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, quality);

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var stream = File.OpenWrite(filePath);
            data.SaveTo(stream);
        }
    }
}