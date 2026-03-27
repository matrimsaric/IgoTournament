using StoneLedger.Controls.Annotations;
using StoneLedger.Models;
using System.Net.NetworkInformation;

namespace StoneLedger.Controls;

public partial class GameReplayerControl : ContentView
{
    public event EventHandler? RequestExpand;
    public bool IsExpanded { get; private set; }


    public static readonly BindableProperty MovesProperty =
     BindableProperty.Create(
         nameof(Moves),
         typeof(IList<SgfMove>),
         typeof(GameReplayerControl),
         propertyChanged: OnMovesChanged);

    public IList<SgfMove>? Moves
    {
        get => (IList<SgfMove>?)GetValue(MovesProperty);
        set => SetValue(MovesProperty, value);
    }

    private static void OnMovesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (GameReplayerControl)bindable;

        if (newValue is IList<SgfMove> moves)
        {
            System.Diagnostics.Debug.WriteLine($"[Replayer] Moves received: {moves.Count}");
            System.Diagnostics.Debug.WriteLine($"[Replayer] First move: {moves[0].X},{moves[0].Y} Color={moves[0].Color}");
            control.Drawable.Moves = moves;
            control.BoardView.Invalidate();
        }
    }

    private void OnToggleAnnotationsPanel(object sender, EventArgs e)
    {
        AnnotationsPanel.IsVisible = !AnnotationsPanel.IsVisible;
    }



    public GameReplayerDrawable Drawable { get; }

    public GameReplayerControl()
    {
        InitializeComponent();
        Drawable = new GameReplayerDrawable();
        BoardView.Drawable = Drawable;   // ← STEP 5 GOES HERE
                                         // Default tool
        AnnotationToolPicker.SelectedIndex = 0;

        RingOptions.IsVisible = true;
        LabelOptions.IsVisible = false;
    }

    private void OnExpandClicked(object sender, EventArgs e)
    {
        RequestExpand?.Invoke(this, EventArgs.Empty);
    }

    bool _showMoveNumbers = false;
    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        System.Diagnostics.Debug.WriteLine($"Modal size: {width} x {height}");

        if (width <= 0 || height <= 0)
            return;

        // Space for the board: use width, and some of the height
        var maxBoardSize = Math.Min(width, height - 80); // 80-ish for buttons/margins

        // Enforce a minimum
        var size = Math.Max(270, maxBoardSize);

        // Optional: snap to whole pixels to keep lines clean
        size = Math.Floor(size);

        BoardView.WidthRequest = size;
        BoardView.HeightRequest = size;

        // Enable move numbers only when the board is comfortably large
        bool shouldShow = size >= 320;

        if (_showMoveNumbers != shouldShow)
        {
            _showMoveNumbers = shouldShow;
            BoardView.Invalidate(); // redraw with/without numbers
        }
    }

    public static readonly BindableProperty CurrentMoveIndexProperty =
    BindableProperty.Create(
        nameof(CurrentMoveIndex),
        typeof(int),
        typeof(GameReplayerControl),
        0,
        propertyChanged: OnCurrentMoveIndexChanged);

    public int CurrentMoveIndex
    {
        get => (int)GetValue(CurrentMoveIndexProperty);
        set => SetValue(CurrentMoveIndexProperty, value);
    }

    private static void OnCurrentMoveIndexChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (GameReplayerControl)bindable;
        control.Drawable.CurrentMoveIndex = (int)newValue;
        control.BoardView.Invalidate();
    }

    private void OnBoardTapped(object sender, TappedEventArgs e)
    {
        if (BoardView.Drawable is not GameReplayerDrawable replayer)
            return;

        var point = e.GetPosition(BoardView);
        if (point == null)
            return;

        // Convert pixel → board coordinate
        var (x, y) = replayer.PixelToBoard(point.Value.X, point.Value.Y);
        if (x < 0 || y < 0)
            return;

        ApplyCurrentAnnotation(x, y);
    }

    private void OnAnnotationToolChanged(object sender, EventArgs e)
    {
        var tool = AnnotationToolPicker.SelectedItem as string;

        if (tool == "Ring")
        {
            RingOptions.IsVisible = true;
            LabelOptions.IsVisible = false;
        }
        else if (tool == "Label")
        {
            RingOptions.IsVisible = false;
            LabelOptions.IsVisible = true;
        }
    }


    private void ApplyCurrentAnnotation(int x, int y)
    {
        if (BoardView.Drawable is not GameReplayerDrawable replayer)
            return;

        var tool = AnnotationToolPicker.SelectedItem as string;

        switch (tool)
        {
            case "Ring":
                ApplyRingAnnotation(replayer, x, y);
                break;

            case "Label":
                ApplyLabelAnnotation(replayer, x, y);
                break;
        }

        BoardView.Invalidate();
    }

    private void ApplyRingAnnotation(GameReplayerDrawable replayer, int x, int y)
    {
        Color ringColor = Colors.Green;

        switch (RingColorPicker.SelectedItem)
        {
            case "Blue Ring":
                ringColor = Colors.Blue;
                break;
        }

        replayer.Annotations.Add(new StoneRingAnnotation
        {
            X = x,
            Y = y,
            Color = ringColor
        });
    }

    private void ApplyLabelAnnotation(GameReplayerDrawable replayer, int x, int y)
    {
        var text = LabelEntry.Text;

        if (string.IsNullOrWhiteSpace(text))
            return;

        replayer.Annotations.Add(new StoneLabelAnnotation
        {
            X = x,
            Y = y,
            Text = text.Trim().Substring(0, 1),
            Color = Colors.IndianRed
        });
    }




    internal void OnClearAnnotationsClicked(object sender, EventArgs e)
    {
        if (BoardView.Drawable is GameReplayerDrawable replayer)
        {
            replayer.Annotations.Clear();
            BoardView.Invalidate();
        }
    }

    private void OnMoveNumberToggleChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BoardView.Drawable is GameReplayerDrawable replayer)
        {
            replayer.ShowMoveNumbers = e.Value;
            BoardView.Invalidate();
        }
    }

    internal void OnAddRingClicked(object sender, EventArgs e)
    {
        if (BoardView.Drawable is not GameReplayerDrawable replayer)
            return;

        if (replayer.Moves == null || replayer.Moves.Count == 0)
            return;

        var move = replayer.Moves[replayer.CurrentMoveIndex];

        // Determine ring color
        Color ringColor = Colors.Green;

        switch (RingColorPicker.SelectedItem)
        {
            case "Blue Ring":
                ringColor = Colors.Blue;
                break;

            case "Green Ring":
            default:
                ringColor = Colors.Green;
                break;
        }

        // Add annotation
        replayer.Annotations.Add(new StoneRingAnnotation
        {
            X = move.X,
            Y = move.Y,
            Color = ringColor
        });

        BoardView.Invalidate();
    }


    public void Redraw()
    {
        BoardView.Invalidate();
    }



    private static void OnSgfTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (GameReplayerControl)bindable;
        var sgf = newValue as string;

        // TODO: parse SGF into an internal model
        //control.Drawable.LoadRawSgf(sgf);
        control.BoardView.Invalidate();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is MatchContentViewModel vm)
        {
            vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(vm.CurrentMoveIndex))
                {
                    Drawable.CurrentMoveIndex = vm.CurrentMoveIndex;
                    Drawable.Moves = vm.ParsedMoves;
                    BoardView.Invalidate();
                }
            };
        }
    }



    public class ExpandEventArgs : EventArgs
    {
        public bool IsExpanded { get; }

        public ExpandEventArgs(bool isExpanded)
        {
            IsExpanded = isExpanded;
        }
    }


}
