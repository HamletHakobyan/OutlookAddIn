using System.Windows;
using System.Windows.Input;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public static class FocusExtension
    {
        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached(
            "IsFocused", typeof (bool), typeof (FocusExtension),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnIsFocusedPropertyChanged));

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (!(bool) e.NewValue || element == null)
            {
                return;
            }

            if (element.IsVisible)
            {

                element.Focusable = true;
                Keyboard.Focus(element);
                element.LostFocus += OnLostFocus;
            }
            else
            {
                DependencyPropertyChangedEventHandler handler = null;
                handler = (sender, args) =>
                {
                    if (!(bool) args.NewValue)
                    {
                        return;
                    }

                    element.Focusable = true;
                    Keyboard.Focus(element);
                    element.LostFocus += OnLostFocus;
                    element.IsVisibleChanged -= handler;
                };
                element.IsVisibleChanged += handler;

            }
        }

        private static void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            var element = sender as UIElement;
            if (element == null)
            {
                return;
            }

            element.LostFocus -= OnLostFocus;
            element.SetCurrentValue(IsFocusedProperty, false);
        }
    }
}