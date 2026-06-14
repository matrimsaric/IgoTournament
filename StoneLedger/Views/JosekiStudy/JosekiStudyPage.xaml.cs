using StoneLedger.Controls;
using StoneLedger.ViewModels.JosekiStudy;

namespace StoneLedger.Views.JosekiStudy;

public partial class JosekiStudyPage : ContentPage
{
	public JosekiStudyPage()
	{
		InitializeComponent();

        BindingContext = new JosekiStudyViewModel();
    }

    private void OnClearAnnotationsClicked(object sender, EventArgs e)
    {

    }

    private void OnAnnotationToolChanged(object sender, EventArgs e)
    {

    }

    private void OnBoardTapped(object sender, (int X, int Y) e)
    {
        if (BindingContext is not JosekiStudyViewModel vm)
            return;

        vm.AddDefaultStone(e.X, e.Y);
        GameReplayer.SetDefaultStones((IEnumerable<Models.SgfMove>)vm.DefaultStones);
    }
}