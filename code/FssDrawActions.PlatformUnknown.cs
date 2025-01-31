using System;
using SkiaSharp;

namespace SymbolGen
{

    public static partial class FssDrawActions
    {
        public static void DrawPlatformUnknown(SKCanvas canvas, SKRect outerRect, SKRect innerRect, FssDrawStyle style)
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

        // ----------------------------------------------------------------------------------------

        // Variation 1: Missing the top arc (for submarine)
        public static void DrawPlatformUnknownMissingTop(SKCanvas canvas, SKRect outerRect, SKRect innerRect, FssDrawStyle style)
        {
            // Set up the paint objects.
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
                StrokeJoin = SKStrokeJoin.Round,
                StrokeCap = SKStrokeCap.Round
            };

            // Define inner square corners.
            SKPoint topLeft     = new SKPoint(innerRect.Left, innerRect.Top);
            SKPoint topRight    = new SKPoint(innerRect.Right, innerRect.Top);
            SKPoint bottomRight = new SKPoint(innerRect.Right, innerRect.Bottom);
            SKPoint bottomLeft  = new SKPoint(innerRect.Left, innerRect.Bottom);

            // Define outer rectangle midpoints.
            SKPoint outerTopMid    = new SKPoint(outerRect.MidX, outerRect.Top);
            SKPoint outerRightMid  = new SKPoint(outerRect.Right, outerRect.MidY);
            SKPoint outerBottomMid = new SKPoint(outerRect.MidX, outerRect.Bottom);
            SKPoint outerLeftMid   = new SKPoint(outerRect.Left, outerRect.MidY);

            // Create fill path (closed)
            SKPath fillPath = new SKPath();
            fillPath.MoveTo(topLeft);
            // Missing top arc: straight line from topLeft to topRight.
            fillPath.LineTo(topRight);
            AddArcThroughPoints(fillPath, topRight, bottomRight, outerRightMid);
            AddArcThroughPoints(fillPath, bottomRight, bottomLeft, outerBottomMid);
            AddArcThroughPoints(fillPath, bottomLeft, topLeft, outerLeftMid);
            fillPath.Close();

            // Create stroke path (open) built the same way but without calling Close()
            SKPath strokePath = new SKPath();
            strokePath.MoveTo(topRight);
            AddArcThroughPoints(strokePath, topRight, bottomRight, outerRightMid);
            AddArcThroughPoints(strokePath, bottomRight, bottomLeft, outerBottomMid);
            AddArcThroughPoints(strokePath, bottomLeft, topLeft, outerLeftMid);
            // Do not call strokePath.Close() so no extra line is drawn.

            canvas.DrawPath(fillPath, fillPaint);
            canvas.DrawPath(strokePath, strokePaint);
        }

        // Variation 2: Missing the bottom arc (for aircraft)
        public static void DrawPlatformUnknownMissingBottom(SKCanvas canvas, SKRect outerRect, SKRect innerRect, FssDrawStyle style)
        {
            // Set up the paint objects.
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
                StrokeJoin = SKStrokeJoin.Round,
                StrokeCap = SKStrokeCap.Round
            };

            // Define inner square corners.
            SKPoint topLeft     = new SKPoint(innerRect.Left, innerRect.Top);
            SKPoint topRight    = new SKPoint(innerRect.Right, innerRect.Top);
            SKPoint bottomRight = new SKPoint(innerRect.Right, innerRect.Bottom);
            SKPoint bottomLeft  = new SKPoint(innerRect.Left, innerRect.Bottom);

            // Define outer rectangle midpoints.
            SKPoint outerTopMid    = new SKPoint(outerRect.MidX, outerRect.Top);
            SKPoint outerRightMid  = new SKPoint(outerRect.Right, outerRect.MidY);
            SKPoint outerBottomMid = new SKPoint(outerRect.MidX, outerRect.Bottom);
            SKPoint outerLeftMid   = new SKPoint(outerRect.Left, outerRect.MidY);

            // Create fill path (closed)
            SKPath fillPath = new SKPath();
            fillPath.MoveTo(topLeft);
            AddArcThroughPoints(fillPath, topLeft, topRight, outerTopMid);
            AddArcThroughPoints(fillPath, topRight, bottomRight, outerRightMid);
            // Missing bottom arc: straight line from bottomRight to bottomLeft.
            fillPath.LineTo(bottomLeft);
            AddArcThroughPoints(fillPath, bottomLeft, topLeft, outerLeftMid);
            fillPath.Close();

            // Create stroke path (open) built the same way but without calling Close()
            SKPath strokePath = new SKPath();
            strokePath.MoveTo(bottomLeft); // missing arc replaced by straight line
            AddArcThroughPoints(strokePath, bottomLeft, topLeft, outerLeftMid);
            AddArcThroughPoints(strokePath, topLeft, topRight, outerTopMid);
            AddArcThroughPoints(strokePath, topRight, bottomRight, outerRightMid);

            canvas.DrawPath(fillPath, fillPaint);
            canvas.DrawPath(strokePath, strokePaint);
        }

        // ----------------------------------------------------------------------------------------

        // Variation 2: Missing the bottom arc (for aircraft)
        public static void DrawPlatformUnknownLeftBar(SKCanvas canvas, SKRect outerRect, SKRect innerRect, FssDrawStyle style)
        {
            // Set up the paint objects.
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
                StrokeJoin = SKStrokeJoin.Round,
                StrokeCap = SKStrokeCap.Round
            };

            // Define inner square corners.
            SKPoint topLeft     = new SKPoint(innerRect.Left, innerRect.Top);
            SKPoint topRight    = new SKPoint(innerRect.Right, innerRect.Top);
            SKPoint bottomRight = new SKPoint(innerRect.Right, innerRect.Bottom);
            SKPoint bottomLeft  = new SKPoint(innerRect.Left, innerRect.Bottom);

            // Define outer rectangle midpoints.
            SKPoint outerTopMid     = new SKPoint(outerRect.MidX, outerRect.Top);
            SKPoint outerRightMid   = new SKPoint(outerRect.Right, outerRect.MidY);
            SKPoint outerBottomMid  = new SKPoint(outerRect.MidX, outerRect.Bottom);
            SKPoint outerLeftMid    = new SKPoint(outerRect.Left, outerRect.MidY);
            SKPoint outerBottomLeft = new SKPoint(outerRect.Left, outerRect.Bottom);

            // Create fill path (closed)
            SKPath fillPath = new SKPath();
            fillPath.MoveTo(topLeft);
            AddArcThroughPoints(fillPath, topLeft, topRight, outerTopMid);
            AddArcThroughPoints(fillPath, topRight, bottomRight, outerRightMid);
            AddArcThroughPoints(fillPath, bottomRight, bottomLeft, outerBottomMid);
            AddArcThroughPoints(fillPath, bottomLeft, topLeft, outerLeftMid);
            fillPath.Close();

            // Create stroke path (open) built the same way but without calling Close()
            SKPath strokePath = new SKPath();
            strokePath.MoveTo(bottomLeft); // missing arc replaced by straight line
            AddArcThroughPoints(strokePath, bottomLeft, topLeft, outerLeftMid);
            AddArcThroughPoints(strokePath, topLeft, topRight, outerTopMid);
            AddArcThroughPoints(strokePath, topRight, bottomRight, outerRightMid);
            AddArcThroughPoints(strokePath, bottomRight, bottomLeft, outerBottomMid);
            strokePath.MoveTo(outerLeftMid);
            strokePath.LineTo(outerBottomLeft);

            canvas.DrawPath(fillPath, fillPaint);
            canvas.DrawPath(strokePath, strokePaint);
        }

        // ----------------------------------------------------------------------------------------

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
