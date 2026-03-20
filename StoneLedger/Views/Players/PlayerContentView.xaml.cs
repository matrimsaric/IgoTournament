using PlayerDomain.Services.Interfaces;
using StoneLedger.ViewModels.Players;

namespace StoneLedger.Views.Players;

public partial class PlayerContentView : ContentView
{
    private readonly PlayerContentViewModel _vm;

    public PlayerContentView()
    {
        InitializeComponent();

        // Resolve VM from DI container
        _vm = StoneLedger.App.Services.GetRequiredService<PlayerContentViewModel>();
        // IMPORTANT: bind only the internal layout to the VM
        RootLayout.BindingContext = _vm;
    }

    public static readonly BindableProperty IsCompactProperty =
    BindableProperty.Create(nameof(IsCompact), typeof(bool), typeof(PlayerContentView), false);

    public bool IsCompact
    {
        get => (bool)GetValue(IsCompactProperty);
        set => SetValue(IsCompactProperty, value);
    }


    public PlayerContentView(PlayerContentViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;

        this.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(PlayerId))
                Console.WriteLine($"PlayerContentView: PlayerId changed to {PlayerId}");
        };
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        // The parent BindingContext is now available
        // The control can now bind PlayerId correctly
    }

    public static readonly BindableProperty PlayerIdProperty =
        BindableProperty.Create(
            nameof(PlayerId),
            typeof(Guid),
            typeof(PlayerContentView),
            Guid.Empty,
            propertyChanged: OnPlayerIdChanged);

    public Guid PlayerId
    {
        get => (Guid)GetValue(PlayerIdProperty);
        set => SetValue(PlayerIdProperty, value);
    }



private static async void OnPlayerIdChanged(BindableObject bindable, object oldValue, object newValue)
    {
    Console.WriteLine($"Image loading");
    var control = (PlayerContentView)bindable;
        var vm = control._vm;

        if (newValue is Guid id && id != Guid.Empty)
            await vm.LoadAsync(id);
    }
}