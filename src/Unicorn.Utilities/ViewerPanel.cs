using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Unicorn.Utilities
{
    public class ViewerPanel : Panel
    {
        public int VisibleIndex
        {
            get
            {
                return (int)GetValue(VisibleIndexProperty);
            }
            set
            {
                SetValue(VisibleIndexProperty, value);
            }
        }
        public static readonly DependencyProperty VisibleIndexProperty = DependencyProperty.Register("VisibleIndex", typeof(int), typeof(ViewerPanel), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, null, IndexCoerceCallBack));

        private static object IndexCoerceCallBack(DependencyObject d, object baseValue)
        {
            int value = (int)baseValue;
            if (value < 0)
            {
                return 0;
            }

            ViewerPanel panel = (ViewerPanel)d;
            int count = panel.NonCollapsedChildren.Count();
            if (value >= count - 1)
            {
                return count - 1;
            }

            return baseValue;
        }

        public IEnumerable<UIElement> NonCollapsedChildren
        {
            get
            {
                return this.InternalChildren.Cast<UIElement>().Where<UIElement>((Func<UIElement, bool>)(e =>
                {
                    if (e != null)
                        return e.Visibility != Visibility.Collapsed;
                    return false;
                }));
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            UIElement current = this.NonCollapsedChildren.Skip(this.VisibleIndex).FirstOrDefault();
            current.Measure(availableSize);

            foreach (UIElement item in this.NonCollapsedChildren)
            {
                if (!object.ReferenceEquals(item, current))
                {
                    current.Measure(new Size(0, 0));
                }
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            UIElement current = this.NonCollapsedChildren.Skip(this.VisibleIndex).FirstOrDefault();
            current.Arrange(new Rect(new Point(0, 0), finalSize));

            foreach (UIElement item in this.NonCollapsedChildren)
            {
                if (!object.ReferenceEquals(item, current))
                {
                    item.Arrange(new Rect(new Point(0, 0), new Size(0, 0)));
                }
            }

            return finalSize;
        }
    }
}
