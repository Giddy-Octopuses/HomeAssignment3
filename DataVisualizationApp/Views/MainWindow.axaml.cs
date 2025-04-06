using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Styling;
using DataVisualizationApp.ViewModels;

namespace DataVisualizationApp.Views
{
    public partial class MainWindow : Window
    {
        private Popup? popup;
        private Border? _previousSelectedBorder;

        public MainWindow()
        {
            InitializeComponent();
            popup = this.FindControl<Popup>("Popup");
            DataContext = new MainWindowViewModel();
        }

        // Ensure that clicking on charts works properly
        private void OnChartSlotPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (sender is Border border)
            {
                // If the Border has no content, the click is ignored
                var contentControl = border.Child as ContentControl;
                if (contentControl?.Content == null)
                {
                    return;
                }

                // Determine the slot index based on the Border's Grid.Column and Grid.Row properties
                var slotIndex = (Grid.GetRow(border) == 0 && Grid.GetColumn(border) == 1) ? 0 :
                                (Grid.GetRow(border) == 0 && Grid.GetColumn(border) == 3) ? 1 :
                                (Grid.GetRow(border) == 1 && Grid.GetColumn(border) == 1) ? 2 :
                                (Grid.GetRow(border) == 1 && Grid.GetColumn(border) == 3) ? 3 :
                                (Grid.GetRow(border) == 2 && Grid.GetColumn(border) == 1) ? 4 :
                                (Grid.GetRow(border) == 2 && Grid.GetColumn(border) == 3) ? 5 : -1;

                if (slotIndex == -1) return; // Invalid slot index

                var viewModel = DataContext as MainWindowViewModel;

                // If the clicked border is already selected, unselect it
                if (_previousSelectedBorder == border)
                {
                    ((IPseudoClasses)border.Classes).Set(":selected", false);
                    _previousSelectedBorder = null; // Clear the selection
                    if (viewModel != null)
                    {
                        viewModel.DeleteButtonVisible = false; // Hide the delete button
                    }
                    return;
                }

                // Remove the :selected pseudo-class from the previously selected border
                if (_previousSelectedBorder != null)
                {
                    ((IPseudoClasses)_previousSelectedBorder.Classes).Set(":selected", false);
                }

                // Apply the :selected pseudo-class to the clicked border
                ((IPseudoClasses)border.Classes).Set(":selected", true);

                // Update the reference to the currently selected border
                _previousSelectedBorder = border;

                if (viewModel != null)
                {
                    viewModel.SelectChart(slotIndex);
                    viewModel.DeleteButtonVisible = true;
                }
            }
        }

        // Handle unclicking after deletion   
        public void UnselectBorder()
        {
            if (_previousSelectedBorder != null)
            {
                ((IPseudoClasses)_previousSelectedBorder.Classes).Set(":selected", false);
                _previousSelectedBorder = null;
            }
        }
    }
}
