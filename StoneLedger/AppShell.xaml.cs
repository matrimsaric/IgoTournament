using StoneLedger.Views.Matches;
using StoneLedger.Views.Players;
using StoneLedger.Views.Rounds;
using StoneLedger.Views.Tournaments;

namespace StoneLedger
{
    public partial class AppShell : Shell
    {
        #pragma warning disable CA1416
        public AppShell()
        {
            InitializeComponent();

            // Register Routes here
            Routing.RegisterRoute("playerdetail", typeof(PlayerDetailPage)); 
            Routing.RegisterRoute("players", typeof(PlayerListPage));
            Routing.RegisterRoute("tournaments", typeof(TournamentListPage));
            Routing.RegisterRoute(nameof(MatchDetailPage), typeof(MatchDetailPage));
            Routing.RegisterRoute(nameof(AddMatchPage), typeof(AddMatchPage));
            Routing.RegisterRoute(nameof(MatchListPage), typeof(MatchListPage));
            Routing.RegisterRoute(nameof(RoundListPage), typeof(RoundListPage));
            Routing.RegisterRoute(nameof(AddRoundPage), typeof(AddRoundPage));
        }
        #pragma warning restore CA1416
    }
}
