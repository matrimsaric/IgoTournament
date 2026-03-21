using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using StoneLedger.Models;

namespace StoneLedger.Controls
{
    public class GameReplayerDrawable : IDrawable
    {
        private IList<SgfMove>? _moves;
        public IList<SgfMove>? Moves { get; set; }
        public int CurrentMoveIndex { get; set; }


        public void LoadMoves(IList<SgfMove>? moves)
        {
            _moves = moves ?? new List<SgfMove>();
        }


        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // 1. Determine square board area
            float size = Math.Min(dirtyRect.Width, dirtyRect.Height);
            float left = (dirtyRect.Width - size) / 2f;
            float top = (dirtyRect.Height - size) / 2f;

            RectF boardRect = new RectF(left, top, size, size);

            // 2. Background
            canvas.FillColor = Colors.Bisque;
            canvas.FillRectangle(boardRect);

            // 3. Grid settings
            int boardSize = 19;
            float padding = size * 0.05f; // 5% margin
            float cellSize = (size - 2 * padding) / (boardSize - 1);

            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 1;

            // 4. Draw grid lines
            for (int i = 0; i < boardSize; i++)
            {
                float offset = boardRect.Left + padding + i * cellSize;

                // Horizontal
                canvas.DrawLine(
                    boardRect.Left + padding,
                    offset,
                    boardRect.Right - padding,
                    offset);

                // Vertical
                canvas.DrawLine(
                    offset,
                    boardRect.Top + padding,
                    offset,
                    boardRect.Bottom - padding);
            }

            // 5. Star points
            int[] starPoints = { 3, 9, 15 };
            float radius = size * 0.01f;

            canvas.FillColor = Colors.Black;

            foreach (int row in starPoints)
            {
                foreach (int col in starPoints)
                {
                    float x = boardRect.Left + padding + col * cellSize;
                    float y = boardRect.Top + padding + row * cellSize;

                    canvas.FillCircle(x, y, radius);
                }
            }

            if (Moves != null)
            {
                for (int i = 0; i <= CurrentMoveIndex && i < Moves.Count; i++)
                {
                    DrawStone(canvas, boardRect, padding, cellSize, Moves[i]);

                }
                var last = Moves[CurrentMoveIndex];
                float x = boardRect.Left + padding + last.X * cellSize;
                float y = boardRect.Top + padding + last.Y * cellSize;

                canvas.StrokeColor = Colors.Red;
                canvas.StrokeSize = 2;
                canvas.DrawCircle(x, y, cellSize * 0.55f);

            }
        }

        private void DrawStone(ICanvas canvas, RectF boardRect, float padding, float cellSize, SgfMove move)
        {
            float x = boardRect.Left + padding + move.X * cellSize;
            float y = boardRect.Top + padding + move.Y * cellSize;

            float radius = cellSize * 0.45f;

            // SGF uses "B" or "W" but your model uses Color
            bool isBlack = move.Color == "B";

            canvas.FillColor = isBlack ? Colors.Black : Colors.White;
            canvas.FillCircle(x, y, radius);

            if (!isBlack)
            {
                canvas.StrokeColor = Colors.Black;
                canvas.StrokeSize = 1;
                canvas.DrawCircle(x, y, radius);
            }

           

        }


    }
}
