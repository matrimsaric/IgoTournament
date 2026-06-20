using Microsoft.Maui.Graphics;
using StoneLedger.Models;
using StoneLedger.Resources.Dictionaries;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static StoneLedger.Views.JosekiStudy.JosekiStudyPage;

namespace StoneLedger.ViewModels.JosekiStudy;

public class JosekiStudyViewModel : BindableObject
{
    private string _description;
    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(); }
    }

    public ObservableCollection<VariationType> VariationTypes { get; } =
    new(Enum.GetValues<VariationType>());

    private VariationType _selectedStudyMode;
    public VariationType SelectedStudyMode
    {
        get => _selectedStudyMode;
        set
        {
            if (_selectedStudyMode != value)
            {
                _selectedStudyMode = value;
                OnPropertyChanged();
                UpdateAvailableBranches();
                UpdateBadge();
            }

        }
    }

    private bool _isSente;
    public bool IsSente
    {
        get => _isSente;
        set
        {
            if (_isSente == value) return;
            _isSente = value;
            OnPropertyChanged();
            UpdateResultRingColour();
        }
    }

    private Color _resultRingColour = Colors.Transparent;
    public Color ResultRingColour
    {
        get => _resultRingColour;
        set
        {
            if (_resultRingColour == value) return;
            _resultRingColour = value;
            OnPropertyChanged();
        }
    }

    private void UpdateResultRingColour()
    {
        // Black = sente, Red = gote
        ResultRingColour = IsSente ? Colors.Black : Colors.Red;
    }


    public ObservableCollection<string> AvailableBranches { get; } = new();

    private string _selectedBranch;
    public string SelectedBranch
    {
        get => _selectedBranch;
        set
        {
            if (_selectedBranch != value)
            {
                _selectedBranch = value;
                OnPropertyChanged();
                UpdateBadge();
            }
        }
    }

    private Color _badgeColour;
    public Color BadgeColour
    {
        get => _badgeColour;
        set { _badgeColour = value; OnPropertyChanged(); }
    }

    private string _badgeKanji;
    public string BadgeKanji
    {
        get => _badgeKanji;
        set { _badgeKanji = value; OnPropertyChanged(); }
    }

    private string _title;
    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    private Color _panelBackgroundColour;
    public Color PanelBackgroundColour
    {
        get => _panelBackgroundColour;
        set { _panelBackgroundColour = value; OnPropertyChanged(); }
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

        SelectedStudyMode = VariationType.None; // triggers branch load + badge update
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

    private void UpdateAvailableBranches()
    {
        AvailableBranches.Clear();

        switch (SelectedStudyMode)
        {
            case VariationType.Joseki:
                AvailableBranches.Add("44");
                AvailableBranches.Add("34");
                AvailableBranches.Add("33");
                AvailableBranches.Add("54");
                break;

            case VariationType.Fuseki:
                AvailableBranches.Add("nirensei");
                AvailableBranches.Add("sanrensei");
                AvailableBranches.Add("shusaku");
                AvailableBranches.Add("chinese");
                AvailableBranches.Add("345");
                break;

            case VariationType.Tesuji:
                // Add later
                AvailableBranches.Add("escape");
                AvailableBranches.Add("kill");
                AvailableBranches.Add("surround");
                AvailableBranches.Add("connect");
                AvailableBranches.Add("disconnect");
                break;
        }

        SelectedBranch = AvailableBranches.FirstOrDefault();
        UpdateBadge();
    }

    private void UpdateBadge()
    {
        if (SelectedBranch == null)
            return;

        BadgeColour = StudyVisuals.BranchColours[SelectedBranch];
        BadgeKanji = StudyVisuals.ModeKanji[SelectedStudyMode];

        // High-opacity background (20%)
        PanelBackgroundColour = BadgeColour.WithAlpha(0.2f);
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
