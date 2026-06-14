using Microsoft.Maui.Graphics;
using StoneLedger.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StoneLedger.ViewModels.JosekiStudy;

public class JosekiStudyViewModel : BindableObject
{
    private string _description;
    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(); }
    }

    public object BoardDrawable { get; }

    // Tab state
    public bool IsAnnotationsTab { get; private set; } = true;
    public bool IsDescriptionTab { get; private set; }
    public bool IsSavePrintTab { get; private set; }

    // Tab button colours
    public Color AnnotationsTabColor => IsAnnotationsTab ? Colors.LightGray : Colors.Transparent;
    public Color DescriptionTabColor => IsDescriptionTab ? Colors.LightGray : Colors.Transparent;
    public Color SavePrintTabColor => IsSavePrintTab ? Colors.LightGray : Colors.Transparent;

    public ICommand SelectTabCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand LoadCommand { get; }
    public ICommand ExportCommand { get; }

    public ObservableCollection<SgfMove> DefaultStones { get; } = new();

    private string _nextColor = "B"; // alternate automatically

    public JosekiStudyViewModel()
    {
        //BoardDrawable = new JosekiBoardDrawable();

        SelectTabCommand = new Command<string>(SelectTab);
        SaveCommand = new Command(() => { /* TODO */ });
        LoadCommand = new Command(() => { /* TODO */ });
        ExportCommand = new Command(() => { /* TODO */ });
    }

    public void AddDefaultStone(int x, int y)
    {
        // Prevent duplicates
        if (DefaultStones.Any(s => s.X == x && s.Y == y))
            return;

        DefaultStones.Add(new SgfMove
        {
            X = x,
            Y = y,
            Color = _nextColor
        });

        // Alternate colour
        _nextColor = _nextColor == "B" ? "W" : "B";

        OnPropertyChanged(nameof(DefaultStones));
    }

    private void SelectTab(string tab)
    {
        IsAnnotationsTab = tab == "Annotations";
        IsDescriptionTab = tab == "Description";
        IsSavePrintTab = tab == "SavePrint";

        OnPropertyChanged(nameof(IsAnnotationsTab));
        OnPropertyChanged(nameof(IsDescriptionTab));
        OnPropertyChanged(nameof(IsSavePrintTab));

        OnPropertyChanged(nameof(AnnotationsTabColor));
        OnPropertyChanged(nameof(DescriptionTabColor));
        OnPropertyChanged(nameof(SavePrintTabColor));
    }
}
