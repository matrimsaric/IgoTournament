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

    private EditorMode _mode = EditorMode.DefaultStones;



    public JosekiStudyPage()
	{
		InitializeComponent();

        BindingContext = new JosekiStudyViewModel();
    }

    private void OnClearAnnotationsClicked(object sender, EventArgs e)
    {
        GameReplayer.OnClearAnnotationsClicked(sender, e);
        _mode = EditorMode.DefaultStones;
    }

    private void OnAnnotationToolChanged(object sender, EventArgs e)
    {
        _mode = EditorMode.Annotations;
    }

    private void OnBoardTapped(object sender, (int X, int Y) e)
    {
        if (BindingContext is not JosekiStudyViewModel vm)
            return;

        if (_mode == EditorMode.DefaultStones)
        {
            vm.AddDefaultStone(e.X, e.Y);
            GameReplayer.SetDefaultStones(vm.DefaultStones);
            return;
        }

        // Annotation mode
        ApplyAnnotationFromPage(e.X, e.Y);
    }

    private void ApplyAnnotationFromPage(int x, int y)
    {
        string tool = AnnotationToolPicker.SelectedItem as string ?? "Ring";
        string? label = LabelEntry.Text;
        string? symbol = SymbolPicker.SelectedItem as string;
        Color color = GetSelectedColor();

        GameReplayer.ApplyAnnotationFromParent(x, y, tool, label, symbol, color, tool == "Joseki");
    }

    private Color GetSelectedColor()
    {
        return RingColorPicker.SelectedItem switch
        {
            "Green" => Colors.Green,
            "LimeGreen" => Colors.LimeGreen,
            "Blue" => Colors.Blue,
            "Purple" => Colors.Purple,
            "White" => Colors.White,
            "Black" => Colors.Black,
            "Yellow" => Colors.Yellow,
            "LightYellow" => Colors.LightYellow,
            "Orange" => Colors.Orange,
            "Red" => Colors.Red,
            _ => Colors.Black
        };
    }
}