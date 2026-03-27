using Microsoft.Maui.Graphics;
using StoneLedger.Controls.Annotations.Interfaces;
using StoneLedger.Models;
using System.Collections.Generic;
using Font = Microsoft.Maui.Graphics.Font;

namespace StoneLedger.Controls
{
    public class GameReplayerDrawable : IDrawable
    {
        public List<IAnnotation> Annotations { get; set; } = new();
        // Cached layout values (updated each Draw call)
        public RectF BoardRect { get; private set; }
        public float Padding { get; private set; }
        public float CellSize { get; private set; }



        private IList<SgfMove>? _moves;
        public IList<SgfMove>? Moves
        {
            get => _moves;
            set
            {
                _moves = value;
                BuildBoardHistory();
            }
        }

        public int CurrentMoveIndex { get; set; }
        public bool ShowMoveNumbers { get; set; } = true;

        // --- NEW: Board history with captures applied ---
        private List<BoardState> BoardHistory { get; set; } = new();


        // -------------------------
        //   BOARD STATE MODEL
        // -------------------------
        public class BoardState
        {
            public string?[,] Grid { get; } = new string?[19, 19];

            public BoardState Clone()
            {
                var copy = new BoardState();
                Array.Copy(Grid, copy.Grid, Grid.Length);
                return copy;
            }
        }




        // -------------------------
        //   BUILD BOARD HISTORY
        // -------------------------
        private void BuildBoardHistory()
        {
            BoardHistory.Clear();

            if (Moves == null || Moves.Count == 0)
                return;

            var board = new BoardState();

            foreach (var move in Moves)
            {
                board = board.Clone();
                ApplyMove(board, move);
                BoardHistory.Add(board);
            }
        }


        // -------------------------
        //   APPLY MOVE + CAPTURES
        // -------------------------
        private void ApplyMove(BoardState board, SgfMove move)
        {
            int x = move.X;
            int y = move.Y;

            board.Grid[x, y] = move.Color;

            // Check adjacent enemy groups
            foreach (var (nx, ny) in Neighbours(x, y))
            {
                if (board.Grid[nx, ny] != null &&
                    board.Grid[nx, ny] != move.Color)
                {
                    if (!HasLiberties(board, nx, ny, new HashSet<(int, int)>()))
                        RemoveGroup(board, nx, ny);
                }
            }

            // Suicide check (rare)
            if (!HasLiberties(board, x, y, new HashSet<(int, int)>()))
                RemoveGroup(board, x, y);
        }


        private bool HasLiberties(BoardState board, int x, int y, HashSet<(int, int)> visited)
        {
            if (visited.Contains((x, y)))
                return false;

            visited.Add((x, y));
            string? color = board.Grid[x, y];

            foreach (var (nx, ny) in Neighbours(x, y))
            {
                if (board.Grid[nx, ny] == null)
                    return true;

                if (board.Grid[nx, ny] == color &&
                    HasLiberties(board, nx, ny, visited))
                    return true;
            }

            return false;
        }

        public (int x, int y) PixelToBoard(double px, double py)
        {
            // Ensure layout is valid
            if (CellSize <= 0)
                return (-1, -1);

            float x = (float)((px - BoardRect.Left - Padding) / CellSize);
            float y = (float)((py - BoardRect.Top - Padding) / CellSize);

            int ix = (int)Math.Round(x);
            int iy = (int)Math.Round(y);

            if (ix < 0 || ix > 18 || iy < 0 || iy > 18)
                return (-1, -1);

            return (ix, iy);
        }



        private void RemoveGroup(BoardState board, int x, int y)
        {
            string? color = board.Grid[x, y];
            var stack = new Stack<(int, int)>();
            stack.Push((x, y));

            while (stack.Count > 0)
            {
                var (cx, cy) = stack.Pop();
                if (board.Grid[cx, cy] != color)
                    continue;

                board.Grid[cx, cy] = null;

                foreach (var (nx, ny) in Neighbours(cx, cy))
                {
                    if (board.Grid[nx, ny] == color)
                        stack.Push((nx, ny));
                }
            }
        }


        private IEnumerable<(int, int)> Neighbours(int x, int y)
        {
            if (x > 0) yield return (x - 1, y);
            if (x < 18) yield return (x + 1, y);
            if (y > 0) yield return (x, y - 1);
            if (y < 18) yield return (x, y + 1);
        }


        // -------------------------
        //   DRAWING
        // -------------------------
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            BoardRect = CalculateBoardRect(dirtyRect);
            Padding = BoardRect.Width * 0.05f;
            CellSize = (BoardRect.Width - 2 * Padding) / 18f;

            DrawBackground(canvas, BoardRect);
            DrawGrid(canvas, BoardRect, Padding, CellSize);
            DrawStarPoints(canvas, BoardRect, Padding, CellSize);
            DrawBoardNotation(canvas, BoardRect, Padding, CellSize);

            if (Moves is null || Moves.Count == 0)
                return;

            DrawStones(canvas, BoardRect, Padding, CellSize);
            DrawLastMoveHighlight(canvas, BoardRect, Padding, CellSize);

            if (ShowMoveNumbers)
                DrawMoveNumbers(canvas, Moves, CellSize);

            foreach (var annotation in Annotations)
                annotation.Draw(canvas, BoardRect, Padding, CellSize);
        }



        private void DrawStones(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            if (CurrentMoveIndex < 0 || CurrentMoveIndex >= BoardHistory.Count)
                return;

            var board = BoardHistory[CurrentMoveIndex];

            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    var color = board.Grid[x, y];
                    if (color != null)
                    {
                        DrawStone(canvas, rect, padding, cellSize,
                            new SgfMove { X = x, Y = y, Color = color });
                    }
                }
            }
        }


        private void DrawLastMoveHighlight(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            if (CurrentMoveIndex < 0 || CurrentMoveIndex >= Moves.Count)
                return;

            var last = Moves[CurrentMoveIndex];

            float x = rect.Left + padding + last.X * cellSize;
            float y = rect.Top + padding + last.Y * cellSize;

            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 2;
            canvas.DrawCircle(x, y, cellSize * 0.55f);
        }


        // --- Existing drawing helpers unchanged ---
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

                canvas.DrawLine(rect.Left + padding, offset, rect.Right - padding, offset);
                canvas.DrawLine(offset, rect.Top + padding, offset, rect.Bottom - padding);
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

        void DrawBackground(ICanvas canvas, RectF rect)
        {
            canvas.FillColor = Colors.Bisque;
            canvas.FillRectangle(rect);
        }

        void DrawMoveNumbers(ICanvas canvas, IList<SgfMove> moves, float cellSize)
        {
            canvas.Font = Font.Default;
            canvas.FontSize = cellSize * 0.45f;

            var board = BoardHistory[CurrentMoveIndex];

            for (int i = 0; i <= CurrentMoveIndex && i < moves.Count; i++)
            {
                var move = moves[i];

                // Only draw number if the stone still exists
                if (board.Grid[move.X, move.Y] != move.Color)
                    continue;

                var number = (i + 1).ToString();

                float cx = (move.X + 1f) * cellSize;
                float cy = (move.Y + 1f) * cellSize;

                var rect = new RectF(
                    cx - cellSize / 2,
                    cy - cellSize / 2,
                    cellSize,
                    cellSize
                );

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

            for (int i = 0; i < 19; i++)
            {
                float x = rect.Left + padding + i * cellSize;

                float topY = rect.Top + padding - cellSize * 0.35f;
                float bottomY = rect.Bottom - padding + cellSize * 0.50f;

                canvas.DrawString(columns[i], x, topY, HorizontalAlignment.Center);
                canvas.DrawString(columns[i], x, bottomY, HorizontalAlignment.Center);
            }

            for (int i = 0; i < 19; i++)
            {
                int rowNumber = 19 - i;
                float y = rect.Top + padding + i * cellSize;

                float leftX = rect.Left + padding - cellSize * 0.55f;
                float rightX = rect.Right - padding + cellSize * 0.40f;

                float adjustedY = y + cellSize * 0.10f;

                canvas.DrawString(rowNumber.ToString(), leftX, adjustedY, HorizontalAlignment.Center);
                canvas.DrawString(rowNumber.ToString(), rightX, adjustedY, HorizontalAlignment.Center);
            }
        }
    }
}
