using StoneLedger.Controls.Annotations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Controls.Annotations
{
    public class AreaHighlightAnnotation : IAnnotation
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public Color Color { get; set; } = Colors.Yellow.WithAlpha(0.3f);

        public void Draw(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            float left = rect.Left + padding + X1 * cellSize;
            float top = rect.Top + padding + Y1 * cellSize;
            float width = (X2 - X1 + 1) * cellSize;
            float height = (Y2 - Y1 + 1) * cellSize;

            canvas.FillColor = Color;
            canvas.FillRectangle(left, top, width, height);
        }
    }

}
