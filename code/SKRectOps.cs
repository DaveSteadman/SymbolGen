using SkiaSharp;

namespace SymbolGen
{
    public static class SKRectOps
    {
        public static SKRect Inset(SKRect rect, float dx, float dy)
        {
            return new SKRect(rect.Left + dx, rect.Top + dy, rect.Right - dx, rect.Bottom - dy);
        }
    }
}
