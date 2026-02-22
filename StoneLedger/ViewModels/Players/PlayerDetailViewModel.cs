using System.Windows.Input;

namespace StoneLedger.ViewModels.Players
{
    public class PlayerDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private Guid _playerId;

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public PlayerDetailViewModel()
        {
            SaveCommand = new Command(async () =>
            {
                // Placeholder save logic
                await Shell.Current.DisplayAlert("Save", "Player saved (placeholder)", "OK");
            });

            DeleteCommand = new Command(async () =>
            {
                // Placeholder delete logic
                await Shell.Current.DisplayAlert("Delete", "Player deleted (placeholder)", "OK");
            });
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("id"))
            {
                _playerId = Guid.Parse(query["id"].ToString());
                Name = $"Loaded Player {_playerId.ToString().Substring(0, 8)}"; // placeholder
            }
        }
    }
}
