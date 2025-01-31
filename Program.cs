using System;
using SkiaSharp;

namespace SymbolGen
{
    class Program
    {
        static void Main(string[] args)
        {
            int width  = 1000;
            int height = 1000;
            SKRect boundaryRect = new SKRect(0, 0, width, height);

            // Create the transparent image using the utility.
            SKBitmap bitmap = FssImageOps.CreateTransparentImage(width, height);

            // Draw directly onto the bitmap.
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Transparent);

                // create an inset rectangle to start drawing in
                SKRect insetRect  = SKRectOps.Inset(boundaryRect,  30,  30);
                SKRect insetRect2 = SKRectOps.Inset(boundaryRect, 240, 240);

                FssDrawStyle style = new FssDrawStyle
                {
                    FillColor   = SKColors.Green,
                    StrokeColor = SKColors.White,
                    StrokeWidth = 20
                };
                // FssDrawActions.DrawRotatedOctagon(canvas, insetRect,  style);
                //FssDrawActions.DrawRotatedOctagon(canvas, insetRect2, style);

                FssDrawActions.DrawPlatformUnknownLeftBar(canvas, insetRect, insetRect2, style);
            }

            // Create an image from the bitmap.
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                string fileName = "output.png";
                using (var stream = System.IO.File.OpenWrite(fileName))
                {
                    data.SaveTo(stream);
                }
                Console.WriteLine($"Image saved to {fileName}");
            }
        }
    }
}
