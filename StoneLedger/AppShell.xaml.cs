using StoneLedger.Views.Players;
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
        }
        #pragma warning restore CA1416
    }
}
