using System.Windows;
using System.Windows.Controls;

namespace Unicorn.Utilities
{
    public abstract class LazyPanel : Panel
    {
        public ArrangeMode ArrangeMode
        {
            get
            {
                return (ArrangeMode)GetValue(ArrangeModeProperty);
            }
            set
            {
                SetValue(ArrangeModeProperty, value);
            }
        }
        public static readonly DependencyProperty ArrangeModeProperty = DependencyProperty.Register("ArrangeMode", typeof(ArrangeMode), typeof(LazyPanel), new PropertyMetadata(ArrangeMode.LazyOneTime, ArrangeModeChanged));

        private static void ArrangeModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LazyPanel)d).OnArrangeModeChanged();
        }

        protected virtual void OnArrangeModeChanged()
        {

        }
    }
}
