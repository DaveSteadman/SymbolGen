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

    public static class FssDrawActions
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






        public static void DrawFourLeafClover(SKCanvas canvas, SKRect outerRect, SKRect innerRect, FssDrawStyle style)
        {
            using var fillPaint = new SKPaint
            {
                Color = style.FillColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            using var strokePaint = new SKPaint
            {
                Color = style.StrokeColor,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = style.StrokeWidth,
                StrokeJoin = SKStrokeJoin.Round  // helps with join issues for wide lines
            };

            // Define the inner square’s corners.
            SKPoint topLeft     = new SKPoint(innerRect.Left, innerRect.Top);
            SKPoint topRight    = new SKPoint(innerRect.Right, innerRect.Top);
            SKPoint bottomRight = new SKPoint(innerRect.Right, innerRect.Bottom);
            SKPoint bottomLeft  = new SKPoint(innerRect.Left, innerRect.Bottom);

            // Define the midpoints of the outer rectangle’s edges.
            SKPoint outerTopMid    = new SKPoint(outerRect.MidX, outerRect.Top);
            SKPoint outerRightMid  = new SKPoint(outerRect.Right, outerRect.MidY);
            SKPoint outerBottomMid = new SKPoint(outerRect.MidX, outerRect.Bottom);
            SKPoint outerLeftMid   = new SKPoint(outerRect.Left, outerRect.MidY);

            // Build a continuous closed path that consists of four arcs.
            var path = new SKPath();
            path.MoveTo(topLeft);
            AddArcThroughPoints(path, topLeft, topRight, outerTopMid);
            AddArcThroughPoints(path, topRight, bottomRight, outerRightMid);
            AddArcThroughPoints(path, bottomRight, bottomLeft, outerBottomMid);
            AddArcThroughPoints(path, bottomLeft, topLeft, outerLeftMid);
            path.Close();

            // Draw the closed shape.
            canvas.DrawPath(path, fillPaint);
            canvas.DrawPath(path, strokePaint);
        }











        // New: Missing the top arc (for example, for a submarine)
        // Instead of an arc between topLeft and topRight (guided by outerTopMid), we draw a straight line.
        public static void DrawFourLeafCloverMissingTop(SKCanvas canvas, SKRect outerRect, SKRect innerRect, FssDrawStyle style)
        {
            using var fillPaint = new SKPaint
            {
                Color = style.FillColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            using var strokePaint = new SKPaint
            {
                Color = style.StrokeColor,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = style.StrokeWidth,
                StrokeJoin = SKStrokeJoin.Round
            };

            // Inner square corners.
            SKPoint topLeft     = new SKPoint(innerRect.Left, innerRect.Top);
            SKPoint topRight    = new SKPoint(innerRect.Right, innerRect.Top);
            SKPoint bottomRight = new SKPoint(innerRect.Right, innerRect.Bottom);
            SKPoint bottomLeft  = new SKPoint(innerRect.Left, innerRect.Bottom);

            // Outer rectangle midpoints.
            SKPoint outerTopMid    = new SKPoint(outerRect.MidX, outerRect.Top);
            SKPoint outerRightMid  = new SKPoint(outerRect.Right, outerRect.MidY);
            SKPoint outerBottomMid = new SKPoint(outerRect.MidX, outerRect.Bottom);
            SKPoint outerLeftMid   = new SKPoint(outerRect.Left, outerRect.MidY);

            var path = new SKPath();
            path.MoveTo(topLeft);
            // For the top edge, use a straight line.
            path.LineTo(topRight);
            // Remaining arcs:
            AddArcThroughPoints(path, topRight, bottomRight, outerRightMid);
            AddArcThroughPoints(path, bottomRight, bottomLeft, outerBottomMid);
            AddArcThroughPoints(path, bottomLeft, topLeft, outerLeftMid);
            path.Close();

            canvas.DrawPath(path, fillPaint);
            canvas.DrawPath(path, strokePaint);
        }

        // New: Missing the bottom arc (for example, for an aircraft)
        // Instead of an arc between bottomRight and bottomLeft (guided by outerBottomMid), we draw a straight line.
        public static void DrawFourLeafCloverMissingBottom(SKCanvas canvas, SKRect outerRect, SKRect innerRect, FssDrawStyle style)
        {
            using var fillPaint = new SKPaint
            {
                Color = style.FillColor,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };
            using var strokePaint = new SKPaint
            {
                Color = style.StrokeColor,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = style.StrokeWidth,
                StrokeJoin = SKStrokeJoin.Round
            };

            // Inner square corners.
            SKPoint topLeft     = new SKPoint(innerRect.Left, innerRect.Top);
            SKPoint topRight    = new SKPoint(innerRect.Right, innerRect.Top);
            SKPoint bottomRight = new SKPoint(innerRect.Right, innerRect.Bottom);
            SKPoint bottomLeft  = new SKPoint(innerRect.Left, innerRect.Bottom);

            // Outer rectangle midpoints.
            SKPoint outerTopMid    = new SKPoint(outerRect.MidX, outerRect.Top);
            SKPoint outerRightMid  = new SKPoint(outerRect.Right, outerRect.MidY);
            SKPoint outerBottomMid = new SKPoint(outerRect.MidX, outerRect.Bottom);
            SKPoint outerLeftMid   = new SKPoint(outerRect.Left, outerRect.MidY);

            var path = new SKPath();
            path.MoveTo(topLeft);
            AddArcThroughPoints(path, topLeft, topRight, outerTopMid);
            AddArcThroughPoints(path, topRight, bottomRight, outerRightMid);
            // For the bottom edge, use a straight line from bottomRight to bottomLeft.
            path.LineTo(bottomLeft);
            AddArcThroughPoints(path, bottomLeft, topLeft, outerLeftMid);
            path.Close();

            canvas.DrawPath(path, fillPaint);
            canvas.DrawPath(path, strokePaint);
        }










        // Helper: adds an arc from point A to B that passes through point C.
        // In degenerate (nearly collinear) cases, simply draws a line.
        private static void AddArcThroughPoints(SKPath path, SKPoint A, SKPoint B, SKPoint C)
        {
            float x1 = A.X, y1 = A.Y;
            float x2 = B.X, y2 = B.Y;
            float x3 = C.X, y3 = C.Y;

            // Calculate circle center via determinant.
            float d = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            if (Math.Abs(d) < 1e-6)
            {
                // Points nearly collinear; just draw a line.
                path.LineTo(B);
                return;
            }

            float x1Sq = x1 * x1 + y1 * y1;
            float x2Sq = x2 * x2 + y2 * y2;
            float x3Sq = x3 * x3 + y3 * y3;
            float centerX = (x1Sq * (y2 - y3) + x2Sq * (y3 - y1) + x3Sq * (y1 - y2)) / d;
            float centerY = (x1Sq * (x3 - x2) + x2Sq * (x1 - x3) + x3Sq * (x2 - x1)) / d;
            SKPoint center = new SKPoint(centerX, centerY);

            float radius = (float)Math.Sqrt((A.X - centerX) * (A.X - centerX) + (A.Y - centerY) * (A.Y - centerY));
            SKRect oval = new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);

            // Compute start, mid, and end angles (in degrees) relative to the circle's center.
            double startAngle = Math.Atan2(A.Y - center.Y, A.X - center.X) * 180.0 / Math.PI;
            double midAngle   = Math.Atan2(C.Y - center.Y, C.X - center.X) * 180.0 / Math.PI;
            double endAngle   = Math.Atan2(B.Y - center.Y, B.X - center.X) * 180.0 / Math.PI;

            double Normalize(double angle)
            {
                double a = angle % 360;
                if (a < 0)
                    a += 360;
                return a;
            }

            startAngle = Normalize(startAngle);
            midAngle   = Normalize(midAngle);
            endAngle   = Normalize(endAngle);

            // Compute sweep angle so that the arc passes through midAngle.
            double sweepCW = endAngle - startAngle;
            if (sweepCW < 0)
                sweepCW += 360;
            double sweepCCW = sweepCW - 360;
            double diff = midAngle - startAngle;
            if (diff < 0)
                diff += 360;
            double sweepAngle = (diff <= sweepCW) ? sweepCW : sweepCCW;

            // Append the arc. Using ArcTo (with forceMoveTo false) joins seamlessly.
            path.ArcTo(oval, (float)startAngle, (float)sweepAngle, false);
        }

    }
}
