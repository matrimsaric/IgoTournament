using StoneLedger.Controls.Annotations.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace StoneLedger.Controls.Annotations
{
    public class StoneSymbolAnnotation : IAnnotation
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Symbol { get; set; } = "";
        public Color Color { get; set; } = Colors.Black;

        public void Draw(ICanvas canvas, RectF boardRect, float padding, float cellSize)
        {
            float cx = boardRect.Left + padding + X * cellSize;
            float cy = boardRect.Top + padding + Y * cellSize;

            canvas.FontColor = Color;
            canvas.FontSize = cellSize * 0.6f;

            var textRect = new RectF(
                cx - cellSize / 2,
                cy - cellSize / 2,
                cellSize,
                cellSize
            );

            canvas.DrawString(
               Symbol,
               textRect,
               HorizontalAlignment.Center,
               VerticalAlignment.Center
           );
        }
    }

}
