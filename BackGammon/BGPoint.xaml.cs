using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
                this.RenderTransform = new CompositeTransform() { Rotation = (value ? 0.0 : 180.0), CenterX=50, CenterY=200 };                
            }
        }

        public Brush TriangleFill
        {
            get { return (Brush)GetValue(TriangleFillProperty); }
            set { SetValue(TriangleFillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TriangleFill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TriangleFillProperty =
            DependencyProperty.Register("TriangleFill", typeof(Brush), typeof(BGPoint), new PropertyMetadata(new SolidColorBrush(Windows.UI.Colors.Red)));
    }

    //public class BoolToRotationConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, string language)
    //    {
            
    //        Debug.WriteLine($"Converter: {value is bool}");
    //        Debug.WriteLine($"Converter: {(bool)value}");
    //        return ((bool)value) ? 0.0 : 180.0;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, string language)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
