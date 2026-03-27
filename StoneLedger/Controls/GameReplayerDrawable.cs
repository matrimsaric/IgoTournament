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
            // Compute geometry
            var boardRect = CalculateBoardRect(dirtyRect);
            float padding = boardRect.Width * 0.05f;
            float cellSize = (boardRect.Width - 2 * padding) / 18f; // 19x19 grid

            DrawBackground(canvas, boardRect);
            DrawGrid(canvas, boardRect, padding, cellSize);
            DrawStarPoints(canvas, boardRect, padding, cellSize);
            DrawBoardNotation(canvas, boardRect, padding, cellSize);

            if (Moves is null || Moves.Count == 0)
                return;

            DrawStones(canvas, boardRect, padding, cellSize);
            DrawLastMoveHighlight(canvas, boardRect, padding, cellSize);
            DrawMoveNumbers(canvas, Moves, cellSize);
        }

        RectF CalculateBoardRect(RectF dirty)
        {
            float size = Math.Min(dirty.Width, dirty.Height);
            float left = (dirty.Width - size) / 2f;
            float top = (dirty.Height - size) / 2f;

            return new RectF(left, top, size, size);
        }

        void DrawGrid(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 1;

            for (int i = 0; i < 19; i++)
            {
                float offset = rect.Left + padding + i * cellSize;

                // Horizontal
                canvas.DrawLine(
                    rect.Left + padding,
                    offset,
                    rect.Right - padding,
                    offset);

                // Vertical
                canvas.DrawLine(
                    offset,
                    rect.Top + padding,
                    offset,
                    rect.Bottom - padding);
            }
        }

        void DrawStarPoints(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            int[] star = { 3, 9, 15 };
            float radius = rect.Width * 0.01f;

            canvas.FillColor = Colors.Black;

            foreach (int r in star)
            {
                foreach (int c in star)
                {
                    float x = rect.Left + padding + c * cellSize;
                    float y = rect.Top + padding + r * cellSize;
                    canvas.FillCircle(x, y, radius);
                }
            }
        }

        void DrawStones(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            for (int i = 0; i <= CurrentMoveIndex && i < Moves.Count; i++)
                DrawStone(canvas, rect, padding, cellSize, Moves[i]);
        }

        void DrawLastMoveHighlight(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            var last = Moves[CurrentMoveIndex];

            float x = rect.Left + padding + last.X * cellSize;
            float y = rect.Top + padding + last.Y * cellSize;

            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 2;
            canvas.DrawCircle(x, y, cellSize * 0.55f);
        }





        void DrawBackground(ICanvas canvas, RectF rect)
        {
            canvas.FillColor = Colors.Bisque;
            canvas.FillRectangle(rect);
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

        void DrawBoardNotation(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            string[] columns = {
        "A","B","C","D","E","F","G","H","J","K",
        "L","M","N","O","P","Q","R","S","T"
    };

            canvas.Font = Font.Default;
            canvas.FontSize = cellSize * 0.45f;
            canvas.FontColor = Colors.Black;

            // --- Column letters (top & bottom) ---
            for (int i = 0; i < 19; i++)
            {
                float x = rect.Left + padding + i * cellSize;

                // Adjusted vertical offsets
                float topY = rect.Top + padding - cellSize * 0.35f;      // was 0.7f
                float bottomY = rect.Bottom - padding + cellSize * 0.50f; // was 0.2f

                canvas.DrawString(columns[i], x, topY, HorizontalAlignment.Center);
                canvas.DrawString(columns[i], x, bottomY, HorizontalAlignment.Center);
            }

            // --- Row numbers (left & right) ---
            for (int i = 0; i < 19; i++)
            {
                int rowNumber = 19 - i;
                float y = rect.Top + padding + i * cellSize;

                // Adjusted horizontal offsets
                float leftX = rect.Left + padding - cellSize * 0.55f;     // was 0.7f
                float rightX = rect.Right - padding + cellSize * 0.40f;   // was 0.2f

                // Slight vertical nudge downward for better centering
                float adjustedY = y + cellSize * 0.10f;

                canvas.DrawString(rowNumber.ToString(), leftX, adjustedY, HorizontalAlignment.Center);
                canvas.DrawString(rowNumber.ToString(), rightX, adjustedY, HorizontalAlignment.Center);
            }
        }




    }
}
