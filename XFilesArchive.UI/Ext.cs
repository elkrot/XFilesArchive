using System.Windows;

namespace Ext
{
    public class E
    {
        public static readonly DependencyProperty IconProperty;

        static E()
        {
            var metadata = new FrameworkPropertyMetadata(null);
            IconProperty = DependencyProperty.RegisterAttached("Icon",
    typeof(FrameworkElement),
    typeof(E), metadata);

        }

        public static FrameworkElement GetIcon(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(IconProperty, value);
        }
    }
}
