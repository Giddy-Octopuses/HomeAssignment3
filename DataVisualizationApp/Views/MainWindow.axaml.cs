using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DataVisualizationApp.Views
{
    public partial class MainWindow : Window
    {
       // private Border mainBorder;

        public MainWindow()
        {
            InitializeComponent();
            // SetBorderSize();
        }

       /* private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            mainBorder = this.FindControl<Border>("MainBorder");
        }

        private void SetBorderSize()
        {
            var primaryScreen = Screens.Primary;
            var screenWidth = primaryScreen.Bounds.Width;
            var screenHeight = primaryScreen.Bounds.Height;

            mainBorder.Width = screenWidth;
            mainBorder.Height = screenHeight;
        } */
    }
}