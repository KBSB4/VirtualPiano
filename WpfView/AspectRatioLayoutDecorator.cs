using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfView
{
    /// <summary>
    /// Obtained from <see href="https://coding4life.wordpress.com/2012/10/15/wpf-resize-maintain-aspect-ratio/"/>
    /// </summary>
    public class AspectRatioLayoutDecorator : Decorator
    {
        public static readonly DependencyProperty AspectRatioProperty =
           DependencyProperty.Register(
              "AspectRatio",
              typeof(double),
              typeof(AspectRatioLayoutDecorator),
              new FrameworkPropertyMetadata
                 (
                    1d,
                    FrameworkPropertyMetadataOptions.AffectsMeasure
                 ),
              ValidateAspectRatio);
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        private static bool ValidateAspectRatio(object value)
        {
            if (value is not double)
            {
                return false;
            }

            var aspectRatio = (double)value;
            return aspectRatio > 0
                     && !double.IsInfinity(aspectRatio)
                     && !double.IsNaN(aspectRatio);
        }
    }
}
