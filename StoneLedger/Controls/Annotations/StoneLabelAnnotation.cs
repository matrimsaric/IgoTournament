using StoneLedger.Controls.Annotations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoneLedger.Controls.Annotations
{
    public class StoneLabelAnnotation : IAnnotation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Text { get; set; } = "?";
        public Color Color { get; set; } = Colors.Black;

        public void Draw(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            float cx = rect.Left + padding + X * cellSize;
            float cy = rect.Top + padding + Y * cellSize;

            canvas.FontColor = Color;
            canvas.FontSize = cellSize * 0.55f;
            canvas.Font = Microsoft.Maui.Graphics.Font.Default;

            var textRect = new RectF(
                cx - cellSize / 2,
                cy - cellSize / 2,
                cellSize,
                cellSize
            );

            canvas.DrawString(
                Text,
                textRect,
                HorizontalAlignment.Center,
                VerticalAlignment.Center
            );
        }
    }


}
