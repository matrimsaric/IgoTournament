using StoneLedger.Controls.Annotations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Controls.Annotations
{
    public class TerritoryAnnotation : IAnnotation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; } = Colors.LightGray;

        public void Draw(ICanvas canvas, RectF boardRect, float padding, float cellSize)
        {
            float cx = boardRect.Left + padding + X * cellSize;
            float cy = boardRect.Top + padding + Y * cellSize;

            var textRect = new RectF(
                cx - cellSize / 2,
                cy - cellSize / 2,
                cellSize,
                cellSize
            );

            canvas.FillColor = Color.WithAlpha(0.35f); // nice soft overlay
            canvas.FillRectangle(textRect);
        }
    }

}
