using CommonModule.Enums;
using PlayerDomain.Model;
using PlayerDomain.Services.Interfaces;
using StoneLedger.Services.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace StoneLedger.ViewModels.Players
{
    public class PlayerContentViewModel : BaseViewModel
    {
        private readonly PlayerService _playerService;
        private readonly ImageService _imageService;
        private int _loadToken;

        public ICommand OpenPlayerCommand { get; }
        public ICommand ReloadPortraitCommand { get; }


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

        private int _sideIndicator;
        public int SideIndicator
        {
            get => _sideIndicator;
            set => SetProperty(ref _sideIndicator, value);
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
            ReloadPortraitCommand = new Command(async () => await ReloadPortraitAsync());

        }

        private async Task ReloadPortraitAsync()
        {
            if (string.IsNullOrWhiteSpace(PortraitUrl))
                return;

            var baseUrl = PortraitUrl.Split('?')[0];
            PortraitUrl = $"{baseUrl}?t={DateTime.UtcNow.Ticks}";

            OnPropertyChanged(nameof(PortraitUrl));
        }


        public async Task LoadAsync(Guid playerId)
        {
            // Each call gets its own token. If PlayerId changes again before this call
            // finishes, a newer token will be issued and this call's results are discarded
            // below, preventing stale data (e.g. a previous player's portrait) from
            // overwriting the currently bound player.
            var token = ++_loadToken;
            var vmId = RuntimeHelpers.GetHashCode(this);
            Debug.WriteLine($"[PlayerVM {vmId}] LoadAsync START playerId={playerId} token={token}");

            // Kick off both requests at the same time
            var playerTask = _playerService.GetPlayerByIdAsync(playerId);
            var imagesTask = _imageService.GetImagesForObjectAsync(
                playerId,
                (int)CommonModule.Enums.ImageSizeType.Portrait
            );

            await Task.WhenAll(playerTask, imagesTask);

            if (token != _loadToken)
            {
                Debug.WriteLine($"[PlayerVM {vmId}] LoadAsync STALE after await, playerId={playerId} token={token} currentToken={_loadToken} - discarding");
                return; // a newer LoadAsync call has superseded this one
            }

            Player = playerTask.Result;
            if (Player is null)
            {
                Debug.WriteLine($"[PlayerVM {vmId}] LoadAsync playerId={playerId} token={token} - Player is null, aborting");
                return;
            }

            Name = Player.Name;
            Rank = Player.Rank;
            BirthYear = Player.BirthYear;

            var images = imagesTask.Result;

            var portrait = images
                .Where(i => i.SizeType == (int)ImageSizeType.Portrait)
                .OrderBy(i => i.SortOrder)
                .FirstOrDefault()
                ?? images.FirstOrDefault();

            Debug.WriteLine($"[PlayerVM {vmId}] LoadAsync playerId={playerId} token={token} Name={Name} PortraitUrl={portrait?.ImageUrl ?? "<null>"}");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (token != _loadToken)
                {
                    Debug.WriteLine($"[PlayerVM {vmId}] LoadAsync STALE on main thread, playerId={playerId} token={token} currentToken={_loadToken} - discarding PortraitUrl assignment");
                    return;
                }

                PortraitUrl = portrait?.ImageUrl;
                OnPropertyChanged(nameof(Player));
                Debug.WriteLine($"[PlayerVM {vmId}] LoadAsync APPLIED playerId={playerId} token={token} PortraitUrl={PortraitUrl ?? "<null>"}");
            });

            // ------------------------------------------------------------
            // TEAM LOGO: fire-and-forget, cannot break portrait load
            // ------------------------------------------------------------
            _ = LoadTeamLogoAsync(playerId, token);
        }

        private async Task LoadTeamLogoAsync(Guid playerId, int token)
        {
            var vmId = RuntimeHelpers.GetHashCode(this);
            try
            {
                var teamImage = await _imageService.GetTeamImagesForObjectAsync(playerId);

                if (token != _loadToken)
                {
                    Debug.WriteLine($"[PlayerVM {vmId}] LoadTeamLogoAsync STALE playerId={playerId} token={token} currentToken={_loadToken} - discarding");
                    return; // a newer LoadAsync call has superseded this one
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (token != _loadToken)
                    {
                        Debug.WriteLine($"[PlayerVM {vmId}] LoadTeamLogoAsync STALE on main thread, playerId={playerId} token={token} currentToken={_loadToken} - discarding");
                        return;
                    }

                    TeamImageUrl = teamImage?.ImageUrl;
                    Debug.WriteLine($"[PlayerVM {vmId}] LoadTeamLogoAsync APPLIED playerId={playerId} token={token} TeamImageUrl={TeamImageUrl ?? "<null>"}");
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PlayerVM {vmId}] LoadTeamLogoAsync EXCEPTION playerId={playerId} token={token}: {ex.Message}");
                // swallow silently — team logo is optional
            }
        }

        //private async Task LoadTeamLogoAsync(Guid playerId)
        //{
        //    try
        //    {
        //        var teamImage = await _imageService.GetTeamImagesForObjectAsync(playerId);

        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            TeamImageUrl = teamImage?.ImageUrl;
        //        });
        //    }
        //    catch
        //    {
        //        // swallow silently — team logo is optional
        //    }
        //}

        private async Task OpenPlayer()
        {
            if (Player is null)
                return;

            await Shell.Current.GoToAsync($"playerdetail?playerId={Player.Id}");
        }

    }


}
