using CompetitionDomain.Model;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using System.Windows.Input;

public class AddRoundViewModel : BaseViewModel
{
    private readonly RoundService _roundService;

    public string RoundName { get; set; }

    public ICommand SaveCommand { get; }

    public AddRoundViewModel(RoundService roundService)
    {
        _roundService = roundService;
        SaveCommand = new Command(async () => await SaveAsync());
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(RoundName))
            return;

        await _roundService.CreateRoundAsync(new Round
        {
            Id = Guid.NewGuid(),
            Name = RoundName
        });

        await Shell.Current.GoToAsync(".."); // navigate back
    }
}
