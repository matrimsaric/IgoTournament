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

        // Variation variables
        public List<SgfMove> VariationMoves { get; set; } = new();
        public List<SgfMove> DefaultStones { get; set; } = new ();
        public int VariationStartIndex { get; set; } = -1;
        public bool ShowVariation { get; set; } = false;
        private static readonly Color VariationBlackColor = Color.FromArgb("4A6FA5"); // Steel Blue
        private static readonly Color VariationWhiteColor = Color.FromArgb("D8C27A");


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
        public int MoveNumberStartIndex { get; set; } = 0;

        /// <summary>
        /// If true, move numbers start at 1 from MoveNumberStartIndex.
        /// If false, they use their actual SGF move numbers.
        /// </summary>
        public bool RenumberFromOne { get; set; } = false;


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

        #region Variation Code
        public void AddVariationMove(int x, int y)
        {
            ShowVariation = true; // ALWAYS show variations when adding one
            string color = GetNextVariationColor();

            if (VariationStartIndex < 0)
            {
                if (DefaultStones?.Count > 0)
                    VariationStartIndex = DefaultStones.Count - 1;
            }
            if(VariationStartIndex < 0)
                VariationStartIndex = CurrentMoveIndex;

            VariationMoves.Add(new SgfMove { X = x, Y = y, Color = color });
        }

        private string GetNextVariationColor()
        {
            // --- 1. Determine the "main line" last stone ---
            SgfMove? lastMain = null;

            // Prefer Moves if they exist
            if (Moves != null && Moves.Count > 0 &&
                CurrentMoveIndex >= 0 && CurrentMoveIndex < Moves.Count)
            {
                lastMain = Moves[CurrentMoveIndex];
            }
            // Otherwise fall back to DefaultStones
            else if (DefaultStones != null && DefaultStones.Count > 0)
            {
                lastMain = DefaultStones.Last();
            }

            // --- 2. If no stones at all, first variation move is Black ---
            if (lastMain == null)
                return "B";

            // --- 3. If this is the FIRST variation move ---
            if (VariationStartIndex < 0)
            {
                // Variation starts AFTER the last main-line stone
                return lastMain.Color == "B" ? "W" : "B";
            }

            // --- 4. Determine starting colour for the variation branch ---
            // VariationStartIndex refers to Moves, so guard it
            SgfMove? variationAnchor = null;

            if (Moves != null && VariationStartIndex >= 0 && VariationStartIndex < Moves.Count)
            {
                variationAnchor = Moves[VariationStartIndex];
            }
            else if (DefaultStones != null && DefaultStones.Count > 0)
            {
                variationAnchor = DefaultStones.Last();
            }

            if (variationAnchor == null)
                return "B";

            string startColor = variationAnchor.Color == "B" ? "W" : "B";

            // --- 5. Alternate based on how many variation moves already exist ---
            return (VariationMoves.Count % 2 == 0)
                ? startColor
                : (startColor == "B" ? "W" : "B");
        }



        private void DrawVariation(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            for (int i = 0; i < VariationMoves.Count; i++)
            {
                var m = VariationMoves[i];

                float x = rect.Left + padding + m.X * cellSize;
                float y = rect.Top + padding + m.Y * cellSize;

                float radius = cellSize * 0.45f;

                // Choose palette colour
                canvas.FillColor = m.Color == "B"
                    ? VariationBlackColor
                    : VariationWhiteColor;
                canvas.FillCircle(x, y, radius);

                canvas.StrokeColor = Colors.Black;
                canvas.StrokeSize = 1;
                canvas.DrawCircle(x, y, radius);

                DrawVariationLetter(canvas, m, i, cellSize);
            }
        }

        private void DrawVariationLetter(ICanvas canvas, SgfMove move, int index, float cellSize)
        {
            string letter = GetVariationLabel(index);

            float cx = (move.X + 1f) * cellSize;
            float cy = (move.Y + 1f) * cellSize;

            var rect = new RectF(
                cx - cellSize / 2,
                cy - cellSize / 2,
                cellSize,
                cellSize
            );

            canvas.Font = Font.Default;
            canvas.FontSize = cellSize * 0.45f;
            canvas.FontColor = Colors.Black;

            canvas.DrawString(
                letter.ToString(),
                rect,
                HorizontalAlignment.Center,
                VerticalAlignment.Center
            );
        }

        private string GetVariationLabel(int index)
        {
            // index = 0 → A
            // index = 25 → Z
            // index = 26 → AA
            // index = 27 → AB
            // etc.

            string label = String.Empty;
            index++; // shift to 1‑based

            while (index > 0)
            {
                index--; // convert to 0‑based
                label = (char)('A' + (index % 26)) + label;
                index /= 26;
            }

            return label;
        }
        #endregion  Variation Code
        // -------------------------
        //   BUILD BOARD HISTORY
        // -------------------------
        private void BuildBoardHistory()
        {
            BoardHistory.Clear();

            var board = new BoardState();

            // --- NEW: Apply default stones first ---
            foreach (var stone in DefaultStones)
                board.Grid[stone.X, stone.Y] = stone.Color;

            if (Moves == null || Moves.Count == 0)
            {
                BoardHistory.Add(board);   // ← THIS is the missing piece
                return;
            }

            foreach (var move in Moves)
            {
                board = board.Clone();
                ApplyMove(board, move);
                BoardHistory.Add(board);
            }
        }

        public void SetDefaultStones(IEnumerable<SgfMove> stones)
        {
            DefaultStones.Clear();
            DefaultStones.AddRange(stones);
            BuildBoardHistory();
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

        public void RemoveLastVariationMove()
        {
            if (VariationMoves.Count == 0)
                return;

            VariationMoves.RemoveAt(VariationMoves.Count - 1);

            // If no moves left, reset the branch
            if (VariationMoves.Count == 0)
                VariationStartIndex = -1;
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

        public void RemoveAnnotationAt(int x, int y)
        {
            var existing = Annotations.FirstOrDefault(a => a.X == x && a.Y == y);
            if (existing != null)
                Annotations.Remove(existing);
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

            //if (Moves is null || Moves.Count == 0)
            //    return;

            //DrawStones(canvas, BoardRect, Padding, CellSize);

            //// Only draw move-dependent layers if moves exist
            //if (Moves is null || Moves.Count == 0)
            //    return;

            DrawStones(canvas, BoardRect, Padding, CellSize);
            DrawLastMoveHighlight(canvas, BoardRect, Padding, CellSize);

            if (ShowMoveNumbers && Moves?.Count > 0)
                DrawMoveNumbers(canvas, Moves, CellSize);

            if (ShowVariation && VariationMoves.Count > 0)
                DrawVariation(canvas, BoardRect, Padding, CellSize);

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


        //private void DrawLastMoveHighlight(ICanvas canvas, RectF rect, float padding, float cellSize)
        //{
        //    if ((Moves == null || Moves?.Count == 0) && DefaultStones.Count == 0)
        //        return;
        //    if (CurrentMoveIndex < 0 || CurrentMoveIndex >= Moves?.Count)
        //        return;

        //    var last =  Moves != null ?  Moves[CurrentMoveIndex] : DefaultStones[DefaultStones.Count  - 1];

        //    float x = rect.Left + padding + last.X * cellSize;
        //    float y = rect.Top + padding + last.Y * cellSize;

        //    canvas.StrokeColor = Colors.Red;
        //    canvas.StrokeSize = 2;
        //    canvas.DrawCircle(x, y, cellSize * 0.55f);
        //}

        private void DrawLastMoveHighlight(ICanvas canvas, RectF rect, float padding, float cellSize)
        {
            // --- 1. Determine the last stone on the board ---
            SgfMove? last = null;

            // Prefer main-line moves
            if (Moves != null && Moves.Count > 0 &&
                CurrentMoveIndex >= 0 && CurrentMoveIndex < Moves.Count)
            {
                last = Moves[CurrentMoveIndex];
            }
            // Otherwise fall back to default stones
            else if (DefaultStones != null && DefaultStones.Count > 0)
            {
                last = DefaultStones.Last();
            }

            // Nothing to highlight
            if (last == null)
                return;

            // --- 2. Compute centre point ---
            float cx = rect.Left + padding + last.X * cellSize;
            float cy = rect.Top + padding + last.Y * cellSize;

            // --- 3. Correct highlight sizing ---
            float radius = cellSize * 0.45f;      // fits inside stone
            float stroke = cellSize * 0.08f;      // thin, clean ring

            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = stroke;
            canvas.DrawCircle(cx, cy, radius);
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
            if (moves == null || moves.Count == 0)
                return;

            canvas.Font = Font.Default;
            canvas.FontSize = cellSize * 0.45f;

            var board = BoardHistory[CurrentMoveIndex];

            int start = Math.Max(0, Math.Min(MoveNumberStartIndex, CurrentMoveIndex));

            var latestAtPoint = new Dictionary<(int x, int y), int>();

            for (int i = start; i <= CurrentMoveIndex && i < moves.Count; i++)
            {
                var m = moves[i];

                // Only consider stones that still exist on the board
                if (board.Grid[m.X, m.Y] != null)
                    latestAtPoint[(m.X, m.Y)] = i;
            }

            foreach (var kvp in latestAtPoint)
            {
                var (x, y) = kvp.Key;
                int i = kvp.Value;

                int number = RenumberFromOne
                    ? (i - start + 1)
                    : (i + 1);

                float cx = (x + 1f) * cellSize;
                float cy = (y + 1f) * cellSize;

                var rect = new RectF(
                    cx - cellSize / 2,
                    cy - cellSize / 2,
                    cellSize,
                    cellSize
                );

                var move = moves[i];
                canvas.FontColor = move.Color == "B" ? Colors.White : Colors.Black;

                canvas.DrawString(
                    number.ToString(),
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
