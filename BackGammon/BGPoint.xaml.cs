using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BackGammon
{
    public sealed partial class BGPoint : UserControl
    {
        public BGPoint()
        {
            this.InitializeComponent();
        }

        public bool TopRow
        {
            set
            {
                RenderTransform = new CompositeTransform() { Rotation = value ? 0.0 : 180.0, CenterX = 50, CenterY = 200 };
            }
        }

        public int PointNumber { get; set; }

        public Brush TriangleFill
        {
            get { return (Brush)GetValue(TriangleFillProperty); }
            set { SetValue(TriangleFillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TriangleFill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TriangleFillProperty =
            DependencyProperty.Register("TriangleFill", typeof(Brush), typeof(BGPoint), new PropertyMetadata(new SolidColorBrush(Windows.UI.Colors.Red)));

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

                    //if (grid.FindName(name) is Ellipse checker)
                    //{
                    //    Grid.SetRow(checker, row);
                    //    Grid.SetColumn(checker, col);

                    //    if (row == 0)
                    //    {
                    //        checker.RenderTransform = new TranslateTransform() { Y = -80 };
                    //    }
                    //    else
                    //    {
                    //        checker.RenderTransform = new TranslateTransform() { Y = 80 };
                    //    }

                    //}
                    rect.Fill = new SolidColorBrush(Colors.Transparent);
                    e.Handled = true;
                }
            }
        }
    }
}
