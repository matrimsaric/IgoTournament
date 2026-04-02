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

    public static readonly BindableProperty SideIndicatorProperty =
     BindableProperty.Create(
         nameof(SideIndicator),
         typeof(int),
         typeof(PlayerContentView),
         0,
         propertyChanged: OnSideIndicatorChanged);

    public int SideIndicator
    {
        get => (int)GetValue(SideIndicatorProperty);
        set => SetValue(SideIndicatorProperty, value);
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

    private static void OnSideIndicatorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlayerContentView view &&
            view.BindingContext is PlayerContentViewModel vm)
        {
            vm.SideIndicator = (int)newValue;
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();


        if (BindingContext is PlayerContentViewModel vm)
        {
            vm.SideIndicator = SideIndicator;
        }
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