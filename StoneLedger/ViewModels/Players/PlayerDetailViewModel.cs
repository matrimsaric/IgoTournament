using CommonModule.Enums;
using ImageDomain.Model;
using PlayerDomain.Model;
using StoneLedger.Services.Api;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Image = ImageDomain.Model.Image;

namespace StoneLedger.ViewModels.Players
{
    public class PlayerDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly ImageService _imageService;

        private Guid _playerId;

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string NameJp { get; set; }
        public string Rank { get; set; }
        public int? BirthYear { get; set; }
        public string Affiliation { get; set; }
        public string Notes { get; set; }
        public bool IsArchived { get; set; }

        public string MainImageUrl { get; set; } // from Image table

        public ObservableCollection<Image> Images { get; set; }

        public List<ImageSizeType> ImageTypes { get; } =   Enum.GetValues(typeof(ImageSizeType)).Cast<ImageSizeType>().ToList();

        private ImageSizeType _selectedImageType = ImageSizeType.Portrait;
        public ImageSizeType SelectedImageType
        {
            get => _selectedImageType;
            set
            {
                if (_selectedImageType != value)
                {
                    _selectedImageType = value;
                    OnPropertyChanged(nameof(SelectedImageWidth));
                    OnPropertyChanged(nameof(SelectedImageHeight));
                    OnPropertyChanged();

                    // Load the image for the new type
                    _ = LoadImageForTypeAsync();
                }
            }
        }

        private string _selectedImageUrl;
        public string SelectedImageUrl
        {
            get => _selectedImageUrl;
            set
            {
                _selectedImageUrl = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeImageCommand { get; }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public PlayerDetailViewModel(ImageService imageService)
        {
            _imageService = imageService;
            SaveCommand = new Command(async () => await SaveCommandAsync());

            DeleteCommand = new Command(async () =>
            {
                // Placeholder delete logic
                await Shell.Current.DisplayAlert("Delete", "Player deleted (placeholder)", "OK");
            });

            ChangeImageCommand = new Command(async () => await ChangeImageAsync());
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("id"))
            {
                _playerId = Guid.Parse(query["id"].ToString());
                
                Name = $"Loaded Player {_playerId.ToString().Substring(0, 8)}"; // placeholder
            }
        }

        private async Task SaveCommandAsync()
        {
            await SaveImageAsync();
        }

        private async Task SaveImageAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedImageUrl))
                return;

            var image = new Image
            {
                ObjectId = _playerId,
                ObjectType = (int)ImageObjectType.Player,
                ImageUrl = SelectedImageUrl,
                SizeType = (int)SelectedImageType,
                SortOrder = 0,
                Notes = "",
                Name = $"{(ImageObjectType.Player)}-{_playerId}-{((ImageSizeType)SelectedImageType)}"
            };

            await _imageService.AddImageAsync(image);
        }

        public async Task LoadPortraitAsync()
        {
            if (_playerId == Guid.Empty)
                return; // New player, nothing to load

            var images = await _imageService.GetImagesForObjectAsync(
                _playerId,
                (int)ImageObjectType.Player
            );

            // Find portrait (or fallback to first image)
            var portrait = images
                .Where(i => i.SizeType == (int)ImageSizeType.Portrait)
                .OrderBy(i => i.SortOrder)
                .FirstOrDefault()
                ?? images.FirstOrDefault();

            SelectedImageUrl = portrait?.ImageUrl;
        }

        private async Task LoadImageForTypeAsync()
        {
            if ( _playerId == Guid.Empty)
                return; // New player, nothing to load

            var images = await _imageService.GetImagesForObjectAsync(
                _playerId,
                (int)ImageObjectType.Player
            );

            var match = images
                .Where(i => i.SizeType == (int)SelectedImageType)
                .OrderBy(i => i.SortOrder)
                .FirstOrDefault();

            SelectedImageUrl = match?.ImageUrl;
        }


        private async Task ChangeImageAsync()
        {
            // Placeholder: pick an image from device
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select an image"
            });

            if (result == null)
                return;

            // For now, just show the local file
            SelectedImageUrl = result.FullPath;

            // Later:
            // - upload to server
            // - save Image record with ObjectId = Player.Id
            // - set SizeType = (int)SelectedImageType
            // - refresh image list
        }

        public double SelectedImageWidth =>
    SelectedImageType switch
    {
        ImageSizeType.Portrait => 200,
        ImageSizeType.Vertical => 160,
        ImageSizeType.Full => 300,
        ImageSizeType.Thumbnail => 100,
        _ => 200
    };

        public double SelectedImageHeight =>
            SelectedImageType switch
            {
                ImageSizeType.Portrait => 260,
                ImageSizeType.Vertical => 300,
                ImageSizeType.Full => 200,
                ImageSizeType.Thumbnail => 100,
                _ => 200
            };

    }
}
