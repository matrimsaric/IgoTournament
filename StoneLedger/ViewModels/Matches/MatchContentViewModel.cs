using CompetitionDomain.Model;
using StoneLedger.Models;
using StoneLedger.Services.Api;
using StoneLedger.Services.Api.Interfaces;
using StoneLedger.ViewModels;
using StoneLedger.Views.Matches;
using System.Text.Json;
using System.Windows.Input;

public class MatchContentViewModel : BaseViewModel
{
    private readonly MatchService _matchService;
    private readonly SgfService _sgfService;
    public ICommand NextMoveCommand { get; }
    public ICommand PreviousMoveCommand { get; }
    public ICommand ExpandSgfCommand { get; }
    public ICommand CloseExpandedSgfCommand { get; }

    public ICommand ToggleExpandCommand { get; }


    private CompetitionDomain.Model.Match _match;
    public CompetitionDomain.Model.Match Match
    {
        get => _match;
        set => SetProperty(ref _match, value);
    }

    private IList<SgfMove>? _parsedMoves;
    public IList<SgfMove>? ParsedMoves
    {
        get => _parsedMoves;
        set => SetProperty(ref _parsedMoves, value);
    }

    private int _currentMoveIndex;
    public int CurrentMoveIndex
    {
        get => _currentMoveIndex;
        set
        {
            if (SetProperty(ref _currentMoveIndex, value))
                OnPropertyChanged(nameof(CurrentMove));
        }
    }

    public SgfMove? CurrentMove =>
        (ParsedMoves != null && CurrentMoveIndex >= 0 && CurrentMoveIndex < ParsedMoves.Count)
            ? ParsedMoves[CurrentMoveIndex]
            : null;



    public MatchContentViewModel(MatchService matchService, SgfService sgfService)
    {
        _matchService = matchService;
        _sgfService = sgfService;

        NextMoveCommand = new Command(NextMove);
        PreviousMoveCommand = new Command(PreviousMove);
        ExpandSgfCommand = new Command(ExpandSgf);
        CloseExpandedSgfCommand = new Command(CloseExpandedSgf);
    }

    private void NextMove()
    {
        if (ParsedMoves == null) return;
        if (CurrentMoveIndex < ParsedMoves.Count - 1)
            CurrentMoveIndex++;
    }

    private void PreviousMove()
    {
        if (CurrentMoveIndex > 0)
            CurrentMoveIndex--;
    }

    private async void CloseExpandedSgf()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }


    private async void ExpandSgf()
    {
        await Shell.Current.Navigation.PushModalAsync(
            new ExpandedSgfPage(ParsedMoves)
        );
    }

    public async Task LoadMatchAsync(Guid matchId)
    {
        IsBusy = true;

        try
        {
            Match = await _matchService.GetMatchByIdAsync(matchId);

            if (Match?.SgfId is Guid sgfId)
            {
                var sgfRecord = await _sgfService.GetSgfRecordByIdAsync(sgfId);

                if (!string.IsNullOrWhiteSpace(sgfRecord?.ParsedMovesJson))
                {
                    ParsedMoves = JsonSerializer.Deserialize<List<SgfMove>>(sgfRecord.ParsedMovesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                                   ?? new List<SgfMove>();
                }
                else
                {
                    ParsedMoves = new List<SgfMove>();
                }
            }
            else
            {
                ParsedMoves = new List<SgfMove>();
            }
            Console.WriteLine($"Loaded {ParsedMoves?.Count ?? 0} moves");
        }
        finally
        {
            IsBusy = false;
        }
    }

}
