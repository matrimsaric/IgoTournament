using CompetitionDomain.Model;
using StoneLedger.Controls;
using StoneLedger.ViewModels.Matches;
using StoneLedger.ViewModels.Players;

namespace StoneLedger.Views.Matches;

[QueryProperty(nameof(MatchId), "MatchId")]
public partial class MatchDetailPage : ContentPage
{


    private readonly MatchDetailViewModel _vm;

    public MatchDetailPage(MatchDetailViewModel vm)
    {
        _vm = vm;
        BindingContext = vm;
        InitializeComponent();

        this.Focus(); // ensures keyboard events arrive

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine($"MatchDetailPage: MatchId = {((MatchDetailViewModel)BindingContext).MatchId}");
    }
   
    public string MatchId
    {
        set
        {
            Console.WriteLine($"MatchDetailPage received MatchId = {value}");

            if (Guid.TryParse(value, out var parsed))
            {
                if (BindingContext is MatchDetailViewModel vm)
                   _ =  _vm.LoadMatchAsync(parsed);
            }
        }
    }

    private void OnMoveNumberToggleChanged(object sender, CheckedChangedEventArgs e) =>
        MatchContentViewInstance.SetShowMoveNumbers(e.Value);

    private void OnJumpClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.JumpToMove(JumpEntry.Text);

    private void OnUndoVariationClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.UndoVariation();

    private void OnRingToolClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.SelectAnnotationTool("Ring");

    private void OnLabelToolClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.SelectAnnotationTool("Label");

    private void OnSymbolToolClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.SelectAnnotationTool("Symbol");

    private void OnTerritoryToolClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.SelectAnnotationTool("Territory");

    private void OnEraserToolClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.SelectAnnotationTool("Eraser");

    private void OnMovesFromToolClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.SelectAnnotationTool("Moves From");

    private void OnVariationToolClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.SelectAnnotationTool("Variation");

    private void OnClearAnnotationsClicked(object sender, EventArgs e) =>
        MatchContentViewInstance.ClearAnnotations();

    private void OnToggleToolbarClicked(object sender, EventArgs e)
    {
        bool isVisible = !AnnotationToolbar.IsVisible;
        AnnotationToolbar.IsVisible = isVisible;
        ToggleToolbarButton.Text = isVisible ? "Hide Tools" : "Show Tools";
    }
}