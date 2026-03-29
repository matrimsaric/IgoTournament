using CommonModule.Enums;
using PlayerDomain.Model;
using PlayerDomain.Services.Interfaces;
using StoneLedger.Services.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace StoneLedger.ViewModels.Players
{
    public class PlayerContentViewModel : BaseViewModel
    {
        private readonly PlayerService _playerService;
        private readonly ImageService _imageService;

        public ICommand OpenPlayerCommand { get; }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _rank;
        public string Rank
        {
            get => _rank;
            set => SetProperty(ref _rank, value);
        }

        private int? _birthYear;
        public int? BirthYear
        {
            get => _birthYear;
            set => SetProperty(ref _birthYear, value);
        }

        private string _portraitUrl;
        public string PortraitUrl
        {
            get => _portraitUrl;
            set => SetProperty(ref _portraitUrl, value);
        }

        private string _teamImageUrl;
        public string TeamImageUrl
        {
            get => _teamImageUrl;
            set => SetProperty(ref _teamImageUrl, value);
        }

        public Player Player { get; private set; }

        public PlayerContentViewModel(PlayerService playerService, ImageService imageService)
        {
            _playerService = playerService;
            _imageService = imageService;
            OpenPlayerCommand = new Command(async () => await OpenPlayer());
        }

        public async Task LoadAsync(Guid playerId)
        {
            // Kick off both requests at the same time
            var playerTask = _playerService.GetPlayerByIdAsync(playerId);
            var imagesTask = _imageService.GetImagesForObjectAsync(
                playerId,
                (int)CommonModule.Enums.ImageSizeType.Portrait
            );

            await Task.WhenAll(playerTask, imagesTask);

            Player = playerTask.Result;
            if (Player is null)
                return;

            Name = Player.Name;
            Rank = Player.Rank;
            BirthYear = Player.BirthYear;

            var images = imagesTask.Result;

            var portrait = images
                .Where(i => i.SizeType == (int)ImageSizeType.Portrait)
                .OrderBy(i => i.SortOrder)
                .FirstOrDefault()
                ?? images.FirstOrDefault();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                PortraitUrl = portrait?.ImageUrl;
            });

            OnPropertyChanged(nameof(PortraitUrl));
            OnPropertyChanged(nameof(Player));

            // ------------------------------------------------------------
            // TEAM LOGO: fire-and-forget, cannot break portrait load
            // ------------------------------------------------------------
            _ = LoadTeamLogoAsync(playerId);
        }

        private async Task LoadTeamLogoAsync(Guid playerId)
        {
            try
            {
                var teamImage = await _imageService.GetTeamImagesForObjectAsync(playerId);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TeamImageUrl = teamImage?.ImageUrl;
                });

                OnPropertyChanged(nameof(TeamImageUrl));
            }
            catch
            {
                // swallow silently — team logo is optional
            }
        }

        private async Task OpenPlayer()
        {
            if (Player is null)
                return;

            await Shell.Current.GoToAsync($"playerdetail?playerId={Player.Id}");
        }

    }


}
