using StoneLedger;
using StoneLedger.Controls;
using StoneLedger.ViewModels.Matches;
using static StoneLedger.Controls.GameReplayerControl;

namespace StoneLedger.Views.Matches;

public partial class MatchContentView : ContentView
{
    private readonly MatchContentViewModel _vm;

    public MatchContentView()
    {
        InitializeComponent();

        // Resolve VM from DI container
        _vm = App.Services.GetRequiredService<MatchContentViewModel>();

        // IMPORTANT: bind only the internal layout to the VM
        RootLayout.BindingContext = _vm;

        // Subscribe to the event from the GameReplayerControl
        Replayer.RequestExpand += OnExpandRequested;
    }

    public MatchContentView(MatchContentViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        RootLayout.BindingContext = vm;

        // Subscribe to the event from the GameReplayerControl
        Replayer.RequestExpand += OnExpandRequested;
    }

    public static readonly BindableProperty MatchIdProperty =
        BindableProperty.Create(
            nameof(MatchId),
            typeof(Guid),
            typeof(MatchContentView),
            Guid.Empty,
            propertyChanged: OnMatchIdChanged);

    public Guid MatchId
    {
        get => (Guid)GetValue(MatchIdProperty);
        set => SetValue(MatchIdProperty, value);
    }

    private void OnExpandRequested(object sender, EventArgs e)
    {
        _vm.ExpandSgfCommand.Execute(null);
    }

    private static async void OnMatchIdChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (MatchContentView)bindable;
        var vm = control._vm;

        if (newValue is Guid id && id != Guid.Empty)
        {
            await vm.LoadMatchAsync(id);
        }
    }
}
