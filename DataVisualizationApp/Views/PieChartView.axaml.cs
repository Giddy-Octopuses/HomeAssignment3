using Avalonia.Controls;
using DataVisualizationApp.ViewModels;

namespace DataVisualizationApp.Views;

public partial class PieChartView : UserControl
{
    public PieChartView()
    {
        InitializeComponent();
        DataContext = new PieChartViewModel();
    }
}
