using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using StoneLedger.Models;
using Font = Microsoft.Maui.Graphics.Font;

namespace StoneLedger.Controls
{
    public class GameReplayerDrawable : IDrawable
    {
        private IList<SgfMove>? _moves;
        public IList<SgfMove>? Moves { get; set; }
        public int CurrentMoveIndex { get; set; }

        public bool ShowMoveNumbers { get; set; }


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

               // if (ShowMoveNumbers)
               // {
                    DrawMoveNumbers(canvas, Moves, cellSize);
               // }

            }

           
        }

        void DrawMoveNumbers(ICanvas canvas, IList<SgfMove> moves, float cellSize)
        {
            canvas.Font = Font.Default;
            canvas.FontSize = cellSize * 0.45f; // scales with board size

            for (int i = 0; i <= CurrentMoveIndex && i < moves.Count; i++)
            {
                var move = moves[i];
                var number = (i + 1).ToString();

                // Center of the stone (corrected)
                float cx = (move.X + 1f) * cellSize;
                float cy = (move.Y + 1f) * cellSize;

                var rect = new RectF(
                    cx - cellSize / 2,
                    cy - cellSize / 2,
                    cellSize,
                    cellSize
                );

                // Optional: white numbers on black stones, black on white
                canvas.FontColor = move.Color == "B" ? Colors.White : Colors.Black;

                canvas.DrawString(
                    number,
                    rect,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Center
                );
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
