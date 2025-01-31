using System;
using SkiaSharp;

namespace SymbolGen
{
    public struct FssDrawStyle
    {
        public SKColor FillColor;
        public SKColor StrokeColor;
        public float StrokeWidth;
    }

    public static partial class FssDrawActions
    {
        public static void DrawOctagon(SKCanvas canvas, SKRect rect, FssDrawStyle style)
        {
            using var fillPaint = new SKPaint { Color = style.FillColor, IsAntialias = true, Style = SKPaintStyle.Fill };
            using var strokePaint = new SKPaint { Color = style.StrokeColor, IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = style.StrokeWidth };

            // Regular (non-rotated) octagon using an offset from the rectangle edges.
            float offset = rect.Width * (1 - 1 / (float)Math.Sqrt(2)) / 2;
            var path = new SKPath();
            path.MoveTo(rect.Left + offset, rect.Top);
            path.LineTo(rect.Right - offset, rect.Top);
            path.LineTo(rect.Right, rect.Top + offset);
            path.LineTo(rect.Right, rect.Bottom - offset);
            path.LineTo(rect.Right - offset, rect.Bottom);
            path.LineTo(rect.Left + offset, rect.Bottom);
            path.LineTo(rect.Left, rect.Bottom - offset);
            path.LineTo(rect.Left, rect.Top + offset);
            path.Close();

            canvas.DrawPath(path, fillPaint);
            canvas.DrawPath(path, strokePaint);
        }

        public static void DrawSquare(SKCanvas canvas, SKRect rect, FssDrawStyle style)
        {
            using var fillPaint = new SKPaint { Color = style.FillColor, IsAntialias = true, Style = SKPaintStyle.Fill };
            using var strokePaint = new SKPaint { Color = style.StrokeColor, IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = style.StrokeWidth };

            canvas.DrawRect(rect, fillPaint);
            canvas.DrawRect(rect, strokePaint);
        }

        public static void DrawDiamond(SKCanvas canvas, SKRect rect, FssDrawStyle style)
        {
            using var fillPaint = new SKPaint { Color = style.FillColor, IsAntialias = true, Style = SKPaintStyle.Fill };
            using var strokePaint = new SKPaint { Color = style.StrokeColor, IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = style.StrokeWidth };

            var path = new SKPath();
            path.MoveTo(rect.MidX, rect.Top);
            path.LineTo(rect.Right, rect.MidY);
            path.LineTo(rect.MidX, rect.Bottom);
            path.LineTo(rect.Left, rect.MidY);
            path.Close();

            canvas.DrawPath(path, fillPaint);
            canvas.DrawPath(path, strokePaint);
        }

        public static void DrawCircle(SKCanvas canvas, SKRect rect, FssDrawStyle style)
        {
            using var fillPaint = new SKPaint { Color = style.FillColor, IsAntialias = true, Style = SKPaintStyle.Fill };
            using var strokePaint = new SKPaint { Color = style.StrokeColor, IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = style.StrokeWidth };

            float radius = Math.Min(rect.Width, rect.Height) / 2;
            canvas.DrawCircle(rect.MidX, rect.MidY, radius, fillPaint);
            canvas.DrawCircle(rect.MidX, rect.MidY, radius, strokePaint);
        }

        public static void DrawTriangle(SKCanvas canvas, SKRect rect, FssDrawStyle style)
        {
            using var fillPaint = new SKPaint { Color = style.FillColor, IsAntialias = true, Style = SKPaintStyle.Fill };
            using var strokePaint = new SKPaint { Color = style.StrokeColor, IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = style.StrokeWidth };

            var path = new SKPath();
            path.MoveTo(rect.MidX, rect.Top);
            path.LineTo(rect.Right, rect.Bottom);
            path.LineTo(rect.Left, rect.Bottom);
            path.Close();

            canvas.DrawPath(path, fillPaint);
            canvas.DrawPath(path, strokePaint);
        }

        // New: Rotated octagon with vertices "rotated" 22.5 degrees so one vertex falls near the center of a side.
        public static void DrawRotatedOctagon(SKCanvas canvas, SKRect rect, FssDrawStyle style)
        {
            using var fillPaint = new SKPaint
            {
                Color = style.FillColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            using var strokePaint = new SKPaint
            {
                Color       = style.StrokeColor,
                IsAntialias = true,
                Style       = SKPaintStyle.Stroke,
                StrokeWidth = style.StrokeWidth
            };

            // Determine the center of the rectangle.
            float cx = rect.MidX;
            float cy = rect.MidY;
            // Use the smaller half-dimension so that the extreme vertices (at 0°, 90°, etc.) lie on the boundary.
            float half = Math.Min(rect.Width, rect.Height) / 2f;
            // For our rotated octagon, the maximum radius is just the half-dimension.
            float R = half;

            var path = new SKPath();
            // Instead of starting at 22.5° (typical for an octagon with flat cardinal sides),
            // we subtract 22.5° (i.e. start at 0°) by simply computing vertices at multiples of 45°.
            for (int i = 0; i < 8; i++)
            {
                double theta = i * 45.0 * Math.PI / 180.0;
                float x = cx + R * (float)Math.Cos(theta);
                float y = cy + R * (float)Math.Sin(theta);
                if (i == 0)
                    path.MoveTo(x, y);
                else
                    path.LineTo(x, y);
            }
            path.Close();

            canvas.DrawPath(path, fillPaint);
            canvas.DrawPath(path, strokePaint);
        }




    }
}
