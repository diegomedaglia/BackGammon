using System;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace BackGammon
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Ellipse_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            Debug.WriteLine("Ellipse Drag starting");

            Ellipse e = (Ellipse)sender;

            Debug.WriteLine("is ellipse");

            args.Data.RequestedOperation = DataPackageOperation.Move;
            args.Data.SetText(e.Name);

            Debug.WriteLine("finished Ellipse_DragStarting");
        }
             
        private void Rectangle_DragLeave(object sender, DragEventArgs e)
        {
            Debug.WriteLine("Rectangle_DragLeave");
            Rectangle r = (Rectangle)sender;
            r.Fill = new SolidColorBrush(Colors.Transparent);
        }

        private void Rectangle_DragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("Rectangle_DragEnter drag enter");
            Rectangle r = (Rectangle)sender;
            
            r.Opacity = 0.7;
            r.Fill = new SolidColorBrush(Colors.LightSeaGreen);
            
            e.AcceptedOperation = DataPackageOperation.Move;

        }

        private async void Rectangle_Drop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("Rectangle_Drop");

            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                var name = await e.DataView.GetTextAsync();

                if (sender is Rectangle rect)
                {
                    int row = Grid.GetRow(rect);
                    int col = Grid.GetColumn(rect);

                    if (grid.FindName(name) is Ellipse checker)
                    {
                        Grid.SetRow(checker, row);
                        Grid.SetColumn(checker, col);

                        if (row == 0)
                        {
                            checker.RenderTransform = new TranslateTransform() { Y = -80 };
                        }
                        else
                        {
                            checker.RenderTransform = new TranslateTransform() { Y = 80 };
                        }
                     
                    }
                    rect.Fill = new SolidColorBrush(Colors.Transparent);
                    e.Handled = true;
                }
            }
        }
    }
}
