using Avalonia.Controls;
using DataVisualizationApp.ViewModels;

namespace DataVisualizationApp.Views
{
    public partial class Nr4BarChartView : UserControl
    {
        public Nr4BarChartView()
        {
            InitializeComponent();
            DataContext = new Nr4BarChartViewModel();
        }
    }
}
