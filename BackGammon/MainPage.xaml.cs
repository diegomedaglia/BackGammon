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
    }
}
