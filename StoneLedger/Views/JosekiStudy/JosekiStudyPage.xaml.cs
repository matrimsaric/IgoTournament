using CommonModule.Enums;
using StoneLedger.Controls;
using StoneLedger.Models;
using StoneLedger.Resources.Dictionaries;
using StoneLedger.ViewModels.JosekiStudy;
using System.Text.Json;

namespace StoneLedger.Views.JosekiStudy;

public partial class JosekiStudyPage : ContentPage, IQueryAttributable
{
    public enum EditorMode
    {
        DefaultStones,
        Annotations
    }

    public enum StudyMode
    {
        Joseki,
        Variation
    }

    

    public StudyMode _currentMode = StudyMode.Joseki;

    public VariationType _currentVariationType = VariationType.Joseki;

    private EditorMode _mode = EditorMode.DefaultStones;


    public JosekiStudyPage(JosekiStudyViewModel vm)
	{
		InitializeComponent();
        VariationButton.IsVisible = false;

        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SelectJosekiMode();


    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var idObj) &&
            idObj is string idString &&
            Guid.TryParse(idString, out var id))
        {
            _ = LoadAndApplyAsync(id);
        }
    }

    private async Task LoadAndApplyAsync(Guid id)
    {
        if (BindingContext is not JosekiStudyViewModel vm)
            return;

        await vm.LoadJosekiAsync(id);

        if (!string.IsNullOrWhiteSpace(vm.MovesJson))
        {
            var data = JsonSerializer.Deserialize<JosekiMoveData>(vm.MovesJson);
            if (data != null)
            {
                GameReplayer.SetVariationState(
                    data.Moves,
                    data.JosekiEndIndex,
                    data.ShowVariation);
            }
        }

        // Ensure the right mode/buttons
        SelectJosekiMode();
    }


    private void SelectJosekiMode()
    {
        _currentMode = StudyMode.Joseki;

        JosekiButton.IsVisible = false;
        VariationButton.IsVisible = true;
    }

    private void SelectVariationMode()
    {
        _currentMode = StudyMode.Variation;

        JosekiButton.IsVisible = false;
        VariationButton.IsVisible = true;
    }

    private void OnJosekiClicked(object sender, EventArgs e)
    {
        SelectJosekiMode();
    }

    private void OnVariationClicked(object sender, EventArgs e)
    {
        SelectVariationMode();
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (BindingContext is not JosekiStudyViewModel vm)
            return;

        // Extract move state from the replayer
        var moveData = new JosekiMoveData
        {
            Moves = GameReplayer.GetVariationMoves(),
            JosekiEndIndex = GameReplayer.GetJosekiEndIndex(),
            ShowVariation = GameReplayer.GetShowVariation()
        };

        // Serialise to JSON
        vm.MovesJson = JsonSerializer.Serialize(moveData);

        // Call the ViewModel save
        await vm.SaveAsync();
    }


    private void OnClearAnnotationsClicked(object sender, EventArgs e)
    {
        GameReplayer.OnClearAnnotationsClicked(sender, e);
        _mode = EditorMode.DefaultStones;
        SelectJosekiMode();
    }

    private void OnAnnotationToolChanged(object sender, EventArgs e)
    {
        _mode = EditorMode.Annotations;
    }

    private void OnBoardTapped(object sender, (int X, int Y) e)
    {

            if (BindingContext is not JosekiStudyViewModel vm)
            return;

       
        // Annotation mode
        ApplyAnnotationFromPage(e.X, e.Y);
    }

    private void ApplyAnnotationFromPage(int x, int y)
    {

        GameReplayer.ApplyAnnotationFromParent(x, y, "Variation", "", null, Colors.Black, _currentMode == StudyMode.Joseki);
    }


}