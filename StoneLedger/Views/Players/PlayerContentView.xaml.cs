using PlayerDomain.Services.Interfaces;
using StoneLedger.ViewModels.Players;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

        Debug.WriteLine($"[PlayerContentView {RuntimeHelpers.GetHashCode(this)}] created (default ctor), VM={RuntimeHelpers.GetHashCode(_vm)}");
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

        Debug.WriteLine($"[PlayerContentView {RuntimeHelpers.GetHashCode(this)}] created (DI ctor), VM={RuntimeHelpers.GetHashCode(_vm)}");

        this.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(PlayerId))
                Debug.WriteLine($"[PlayerContentView {RuntimeHelpers.GetHashCode(this)}] PlayerId changed to {PlayerId}");
        };
    }

    private static void OnSideIndicatorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlayerContentView view)
        {
            view._vm.SideIndicator = (int)newValue;
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        // NOTE: The control intentionally keeps its own _vm bound to RootLayout,
        // separate from the inherited parent BindingContext (e.g. MatchDetailViewModel).
        // Sync SideIndicator directly onto _vm instead of relying on BindingContext.
        _vm.SideIndicator = SideIndicator;

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
        var control = (PlayerContentView)bindable;
        var vm = control._vm;
        var viewId = RuntimeHelpers.GetHashCode(control);
        var vmId = RuntimeHelpers.GetHashCode(vm);

        Debug.WriteLine($"[PlayerContentView {viewId}] OnPlayerIdChanged old={oldValue} new={newValue} VM={vmId}");

        if (newValue is not Guid id || id == Guid.Empty)
            return;

        try
        {
            await vm.LoadAsync(id);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[PlayerContentView {viewId}] failed to load player {id}: {ex}");
        }
    }
}