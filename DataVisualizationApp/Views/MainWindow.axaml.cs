using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Primitives;
using DataVisualizationApp.ViewModels;

namespace DataVisualizationApp.Views
{
    public partial class MainWindow : Window
    {
        private Popup? popup;

        public MainWindow()
        {
            InitializeComponent();
            popup = this.FindControl<Popup>("Popup");
            DataContext = new MainWindowViewModel();
        }
    }
}
