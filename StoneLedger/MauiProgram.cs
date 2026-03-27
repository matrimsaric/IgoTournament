using CommunityToolkit.Maui;
using CompetitionDomain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StoneLedger.Resources.Converters;
using StoneLedger.Services.Api;
using StoneLedger.ViewModels;
using StoneLedger.ViewModels.Matches;
using StoneLedger.ViewModels.Players;
using StoneLedger.ViewModels.Rounds;
using StoneLedger.Views;
using StoneLedger.Views.Matches;
using StoneLedger.Views.Players;
using StoneLedger.Views.Rounds;
using StoneLedger.Views.Tournaments;

namespace StoneLedger
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>()
       .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // API Services
            builder.Services.AddHttpClient<PlayerService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
            });

            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/")
            });

            builder.Services.AddTransient<TournamentService>();

            // builder.Services.AddSingleton<RoundService>();
            // builder.Services.AddSingleton<MatchService>();

            // ViewModels
            builder.Services.AddTransient<PlayerListViewModel>();
            builder.Services.AddTransient<TournamentListViewModel>();
            // builder.Services.AddTransient<RoundListViewModel>();
            // builder.Services.AddTransient<MatchListViewModel>();

            // Pages
            builder.Services.AddTransient<PlayerListPage>();
            builder.Services.AddTransient<TournamentListPage>();
            // builder.Services.AddTransient<RoundListPage>();
            // builder.Services.AddTransient<MatchListPage>();

            builder.Services.AddTransient<RoundService>();
            builder.Services.AddTransient<RoundListViewModel>();
            builder.Services.AddTransient<RoundListPage>();

            builder.Services.AddTransient<MatchListViewModel>();
            builder.Services.AddTransient<MatchListPage>();
            builder.Services.AddTransient<MatchService>();

            builder.Services.AddTransient<AddRoundPage>();
            builder.Services.AddTransient<AddRoundPage>();
            builder.Services.AddTransient<AddRoundViewModel>();

            builder.Services.AddTransient<AddMatchPage>();
            builder.Services.AddTransient<AddMatchPage>();
            builder.Services.AddTransient<AddMatchViewModel>();
            builder.Services.AddTransient<SgfService>();
            builder.Services.AddTransient<MatchDetailPage>();

            builder.Services.AddTransient<ImageService>();
            builder.Services.AddTransient<PlayerDetailViewModel>();
            builder.Services.AddTransient<PlayerDetailPage>();

            builder.Services.AddTransient<PlayerContentViewModel>();
            builder.Services.AddTransient<PlayerContentView>();
            builder.Services.AddTransient<MatchDetailViewModel>();
            builder.Services.AddTransient<MatchContentViewModel>();
            builder.Services.AddTransient<MatchContentView>();
            builder.Services.AddTransient<ExpandedSgfPage>();

            builder.Services.AddSingleton<NullToDefaultImageConverter>();


            return builder.Build();
        }
    }
}
