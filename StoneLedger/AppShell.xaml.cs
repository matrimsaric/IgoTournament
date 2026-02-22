using StoneLedger.Views.Players;

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
        }
        #pragma warning restore CA1416
    }
}
