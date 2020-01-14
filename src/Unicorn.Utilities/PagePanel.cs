using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Unicorn.Utilities
{
    /// <summary>
    /// 对Children每项进行分页显示
    /// </summary>
    public class PagePanel : Panel, IPagePanel
    {

        public int PageCount
        {
            get
            {
                return (int)GetValue(PageCountProperty);
            }
            private set
            {
                SetValue(PageCountProperty, value);
            }
        }
        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register("PageCount", typeof(int), typeof(PagePanel), new PropertyMetadata(0));

        public int PageIndex
        {
            get
            {
                return (int)GetValue(PageIndexProperty);
            }
            set
            {
                SetValue(PageIndexProperty, value);
            }
        }
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(int), typeof(PagePanel), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange, PageIndexChangedCallBack, PageIndexCoerceValueCallback));
        private static void PageIndexChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        private static object PageIndexCoerceValueCallback(DependencyObject d, object baseValue)
        {
            int value = (int)baseValue;
            if (value < 0)
            {
                return 0;
            }

            PagePanel panel = (PagePanel)d;
            int pagecount = panel.NonCollapsedChildren.Count();
            if (value >= pagecount - 1)
            {
                return pagecount - 1;
            }

            return baseValue;
        }
        private IEnumerable<UIElement> NonCollapsedChildren
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

        private bool CheckSize(Size size)
        {
            return !(double.IsInfinity(size.Width)
                            || double.IsNaN(size.Width)
                            || double.IsInfinity(size.Height)
                            || double.IsNaN(size.Height));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.PageCount = this.NonCollapsedChildren.Count();

            UIElement element = this.NonCollapsedChildren.Skip(this.PageIndex).FirstOrDefault();
            if (element != null)
            {
                element.Measure(availableSize);

                if (this.CheckSize(availableSize))
                {
                    return new Size(Math.Max(availableSize.Width, element.DesiredSize.Width), Math.Max(availableSize.Height, element.DesiredSize.Height));
                }
                return element.DesiredSize;
            }

            return new Size(0, 0);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect rect = new Rect(0, 0, finalSize.Width, finalSize.Height);
            Rect collapsed = new Rect(new Point(0, 0), new Size(0, 0));

            UIElement element = this.NonCollapsedChildren.Skip(this.PageIndex).FirstOrDefault();
            if (element != null)
            {
                element.Arrange(rect);

                foreach (UIElement item in this.NonCollapsedChildren)
                {
                    if (!object.ReferenceEquals(item, element))
                    {
                        item.Arrange(collapsed);
                    }
                }

                return finalSize;
            }

            return new Size(0, 0);
        }
    }

}
