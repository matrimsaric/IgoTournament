using Microsoft.Extensions.Logging;
using StoneLedger.Services.Api;
using StoneLedger.Services.ViewModels.Tournaments;
using StoneLedger.ViewModels;
using StoneLedger.ViewModels.Players;
using StoneLedger.Views;
using StoneLedger.Views.Players;
using StoneLedger.Views.Tournaments;
using Microsoft.Extensions.DependencyInjection;

namespace StoneLedger
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
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

           // builder.Services.AddSingleton<TournamentService>();
           // builder.Services.AddSingleton<RoundService>();
           // builder.Services.AddSingleton<MatchService>();

            // ViewModels
            builder.Services.AddTransient<PlayerListViewModel>();
            //builder.Services.AddTransient<TournamentListViewModel>();
            // builder.Services.AddTransient<RoundListViewModel>();
            // builder.Services.AddTransient<MatchListViewModel>();

            // Pages
            builder.Services.AddTransient<PlayerListPage>();
            //builder.Services.AddTransient<TournamentListPage>();
            // builder.Services.AddTransient<RoundListPage>();
            // builder.Services.AddTransient<MatchListPage>();



            return builder.Build();
        }
    }
}
