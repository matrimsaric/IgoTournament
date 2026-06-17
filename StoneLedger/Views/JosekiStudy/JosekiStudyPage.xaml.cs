using StoneLedger.Controls;
using StoneLedger.ViewModels.JosekiStudy;

namespace StoneLedger.Views.JosekiStudy;

public partial class JosekiStudyPage : ContentPage
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

    private EditorMode _mode = EditorMode.DefaultStones;


    public JosekiStudyPage()
	{
		InitializeComponent();
        VariationButton.IsVisible = false;

        BindingContext = new JosekiStudyViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
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