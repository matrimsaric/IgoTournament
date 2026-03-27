using StoneLedger.Controls.Annotations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Controls.Annotations
{
    public class StoneRingAnnotation : IAnnotation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; } = Colors.Blue;

        public void Draw(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            float cx = rect.Left + padding + X * cellSize;
            float cy = rect.Top + padding + Y * cellSize;

            canvas.StrokeColor = Color;
            canvas.StrokeSize = 3;
            canvas.DrawCircle(cx, cy, cellSize * 0.55f);
        }
    }

}
