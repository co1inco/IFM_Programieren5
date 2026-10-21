using System.Windows.Controls;
using System.Windows.Input;

namespace Cagd.Views;

public partial class ControlPointView : UserControl
{
    public ControlPointView()
    {
        InitializeComponent();
    }

    private void ControlPointView_OnMouseEnter(object sender, MouseEventArgs e)
    {
        if (DataContext is not ControlPointViewModel viewModel)
            return;
        viewModel.IsHovered = true;
    }

    private void ControlPointView_OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (DataContext is not ControlPointViewModel viewModel)
            return;
        viewModel.IsHovered = false;
    }
}