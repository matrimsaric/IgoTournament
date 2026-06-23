using CommonModule.Enums;
using JosekiDomain.Model;
using JosekiDomain.Services.Interfaces;
using Microsoft.Maui.Graphics;
using StoneLedger.Models;
using StoneLedger.Resources.Dictionaries;
using StoneLedger.Services.Api;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using static StoneLedger.Views.JosekiStudy.JosekiStudyPage;

namespace StoneLedger.ViewModels.JosekiStudy;

public class JosekiStudyViewModel : BindableObject
{
    public event Action? JosekiLoaded;
    private readonly JosekiEntryService _josekiEntryService;
    private readonly JosekiBookService _josekiBookService;

    public Guid? ParentId { get; set; }
    public int? VariationChangeIndex { get; set; }
    public string VariationChangeCoord { get; set; } = string.Empty;
    public bool IsEvenResult { get; set; }
    public Guid? BookId { get; set; }

    public ICommand SaveCommand { get; }

    private string _description;
    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(); }
    }

    private string _movesJson = string.Empty;
    public string MovesJson
    {
        get => _movesJson;
        set
        {
            if (_movesJson != value)
            {
                _movesJson = value;
                OnPropertyChanged();
            }
        }
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
    private bool _titleManuallyEdited = false;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            _titleManuallyEdited = true;
            OnPropertyChanged();
        }
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
    public ICommand LoadCommand { get; }
    public ICommand ExportCommand { get; }

    public ObservableCollection<SgfMove> DefaultStones { get; } = new();

    private string _nextColor = "B"; // alternate automatically

    public JosekiStudyViewModel(JosekiEntryService josekiEntryService, JosekiBookService josekiBookService)
    {
        //BoardDrawable = new JosekiBoardDrawable();
        _josekiEntryService = josekiEntryService;
        _josekiBookService = josekiBookService;

        SelectTabCommand = new Command<string>(SelectTab);
        SaveCommand = new Command(async () => await SaveAsync());
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

    public async Task LoadJosekiAsync(Guid id)
    {
        var entry = await _josekiEntryService.GetJosekiByIdAsync(id);
        if (entry == null)
            return;

        Title = entry.Name;
        Description = entry.Description;
        MovesJson = entry.Moves;

        SelectedStudyMode = (VariationType)entry.Category;
        SelectedBranch = entry.SubCategory.ToString() ?? "";
        Intent = (JosekiIntent)entry.Intent;
        IsSente = entry.IsSente;

        ParentId = entry.ParentId;
        VariationChangeIndex = entry.VariationChangeIndex;
        VariationChangeCoord = entry.VariationChangeCoord;

        UpdateBadge();
        UpdateResultRingColour();
    }


    internal async Task SaveAsync()
    {
        var entry = new JosekiEntry
        {
            Id = Guid.NewGuid(),
            Name = this.Title ?? string.Empty,
            Category = (int)this.SelectedStudyMode,
            SubCategory = ParseSubCategory(SelectedBranch) ?? 0,
            IsSente = this.IsSente,
            BookId = this.BookId ?? DefaultJosekiBookId,
            Description = this.Description ?? string.Empty,
            Moves = MovesJson,
            VariationChangeIndex = this.VariationChangeIndex ?? 0,
            VariationChangeCoord = this.VariationChangeCoord ?? string.Empty,
            ParentId = this.ParentId ?? Guid.Empty,
            IsEvenResult = this.IsEvenResult,
            Intent = (int)this.Intent,
        };

        await _josekiEntryService.CreateJosekiAsync(entry);
    }

    private static readonly Guid DefaultJosekiBookId =
    Guid.Parse("019ee999-c1d2-733c-816d-ebe3c2f090ae");

    public void AutoGenerateTitle()
    {
        if (_titleManuallyEdited)
            return;

        string modeName = SelectedStudyMode.ToString(); // Joseki, Fuseki, Tesuji
        string branch = SelectedBranch ?? "";

        // Prefer Kanji if available
        string kanji = BadgeKanji;

        // Format: "定石 44" or "布石 sanrensei"
        Title = $"{kanji} {branch}".Trim();

    }

    public List<JosekiIntent> IntentOptions { get; } =
    Enum.GetValues(typeof(JosekiIntent)).Cast<JosekiIntent>().ToList();


    private JosekiIntent _intent = JosekiIntent.Neutral;
    public JosekiIntent Intent
    {
        get => _intent;
        set
        {
            _intent = value;
            OnPropertyChanged();
        }
    }


    private int? ParseSubCategory(string branch)
    {
        if (int.TryParse(branch, out var value))
            return value;

        return null;
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

        AutoGenerateTitle();
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
