using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using Unicorn.Utilities.Util;

namespace Unicorn.Utilities
{
    /// <summary>
    /// 可分页的WrapPanel
    /// 宽度和高度自适应
    /// </summary>
    public class PaginationWrapPanel : LazyPanel, IScrollInfo, IPagePanel
    {
        public double ItemWidth
        {
            get
            {
                return (double)GetValue(ItemWidthProperty);
            }
            set
            {
                SetValue(ItemWidthProperty, value);
            }
        }
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(PaginationWrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double ItemHeight
        {
            get
            {
                return (double)GetValue(ItemHeightProperty);
            }
            set
            {
                SetValue(ItemHeightProperty, value);
            }
        }
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(PaginationWrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public int Rows
        {
            get
            {
                return (int)GetValue(RowsProperty);
            }
            set
            {
                SetValue(RowsProperty, value);
            }
        }
        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows", typeof(int), typeof(PaginationWrapPanel), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public int Columns
        {
            get
            {
                return (int)GetValue(ColumnsProperty);
            }
            set
            {
                SetValue(ColumnsProperty, value);
            }
        }
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(int), typeof(PaginationWrapPanel), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

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
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(int), typeof(PaginationWrapPanel), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, PageIndexChangedCallBack, PageIndexCoerceValueCallback));
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

            PaginationWrapPanel panel = (PaginationWrapPanel)d;
            if (value >= panel.PageCount - 1)
            {
                return panel.PageCount - 1;
            }

            return baseValue;
        }

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
        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register("PageCount", typeof(int), typeof(PaginationWrapPanel), new PropertyMetadata(0));


        private static bool GetIsArranged(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsArrangedProperty);
        }
        private static void SetIsArranged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsArrangedProperty, value);
        }
        //此属性标记元素是不是已经由WPF布局引擎初始化过，若初始化过则需重新排列，否侧延迟排列，用以优化布局第一次加载
        private static readonly DependencyProperty IsArrangedProperty = DependencyProperty.RegisterAttached("IsArranged", typeof(bool), typeof(PaginationWrapPanel), new PropertyMetadata(false));


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

        public IEnumerable<UIElement> CurrentPageChildren
        {
            get
            {
                return this.NonCollapsedChildren.Skip(this._startIndex).Take(this._pageCount);
            }
        }


        public UIElement AnchoredItem
        {
            get
            {
                return (UIElement)GetValue(AnchoredItemProperty);
            }
            set
            {
                SetValue(AnchoredItemProperty, value);
            }
        }
        public static readonly DependencyProperty AnchoredItemProperty = DependencyProperty.Register("AnchoredItem", typeof(UIElement), typeof(PaginationWrapPanel), new PropertyMetadata(null, AnchoredItemChanged));

        private static void AnchoredItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PaginationWrapPanel)d).InvalidateVisual();
        }

        public bool ShowGridLines
        {
            get
            {
                return (bool)GetValue(ShowGridLinesProperty);
            }
            set
            {
                SetValue(ShowGridLinesProperty, value);
            }
        }
        public static readonly DependencyProperty ShowGridLinesProperty = DependencyProperty.Register("ShowGridLines", typeof(bool), typeof(PaginationWrapPanel), new FrameworkPropertyMetadata(false, ShowGridLinesChanged));

        private static void ShowGridLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PaginationWrapPanel panel = (PaginationWrapPanel)d;
            panel.InvalidateVisual();
        }

        public Brush LineBrush
        {
            get
            {
                return (Brush)GetValue(ControlLinesRenderer.LineBrushProperty);
            }
            set
            {
                SetValue(ControlLinesRenderer.LineBrushProperty, value);
            }
        }

        public double LineThickness
        {
            get
            {
                return (double)GetValue(ControlLinesRenderer.LineThicknessProperty);
            }
            set
            {
                SetValue(ControlLinesRenderer.LineThicknessProperty, value);
            }
        }

        public LineStyle LineStyle
        {
            get
            {
                return (LineStyle)GetValue(ControlLinesRenderer.LineStyleProperty);
            }
            set
            {
                SetValue(ControlLinesRenderer.LineStyleProperty, value);
            }
        }

        private double GetLineThickness()
        {
            return this.LineThickness >= 0 ? this.LineThickness : 1d;
        }

        private Brush GetLineBrush()
        {
            return this.LineBrush == null ? new SolidColorBrush(Colors.Black) : this.LineBrush;
        }

        public bool CheckColumns()
        {
            return this.Columns > 0;
        }
        private bool CheckItemWidth()
        {
            return !double.IsInfinity(this.ItemWidth)
                            && !double.IsNaN(this.ItemWidth)
                            && this.ItemWidth > 0;
        }

        public bool CheckRows()
        {
            return this.Rows > 0;
        }
        private bool CheckItemHeight()
        {
            return !double.IsInfinity(this.ItemHeight)
                            && !double.IsNaN(this.ItemHeight)
                            && this.ItemHeight > 0;
        }


        private ControlLinesRenderer _controlLinesRenderer = null;
        protected override Visual GetVisualChild(int index)
        {
            if (index < base.VisualChildrenCount)
            {
                return base.GetVisualChild(index);
            }
            else
            {
                if (index == base.VisualChildrenCount)
                {
                    if (this.AnchoredItem != null)
                    {
                        return (Visual)this.AnchoredItem;
                    }
                    else if (this._controlLinesRenderer != null)
                    {
                        return this._controlLinesRenderer;
                    }
                }
                else if (index == base.VisualChildrenCount + 1)
                {
                    if (this._controlLinesRenderer == null)
                        throw new ArgumentOutOfRangeException(nameof(index), (object)index, "Visual_ArgumentOutOfRange");

                    return this._controlLinesRenderer;
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index), (object)index, "Visual_ArgumentOutOfRange");
        }

        private bool _isAnchoredItemInit = false;
        protected override int VisualChildrenCount
        {
            get
            {
                return base.VisualChildrenCount
                        + this.AnchoredItemCount()
                        + (this.ShowGridLines && this._controlLinesRenderer != null ? 1 : 0);
            }
        }


        private int AnchoredItemCount()
        {
            return this._isAnchoredItemInit && this.AnchoredItem != null ? 1 : 0;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            UIElement first = this.NonCollapsedChildren.FirstOrDefault();

            if (first == null)
            {
                this._rows = 0;
                this._columns = 0;

                return new Size(0, 0);
            }

            if (double.IsInfinity(availableSize.Width)
                    && !this.CheckItemWidth())
            {
                throw new Exception("无法确定元素占用宽度，请指定ItemWidth。");
            }

            if (double.IsInfinity(availableSize.Height)
                    && !this.CheckItemHeight())
            {
                throw new Exception("无法确定元素占用宽度，请指定ItemHeight。");
            }

            bool flag = false;

            if (double.IsInfinity(availableSize.Width)
                    && double.IsInfinity(availableSize.Height))
            {
                flag = true;

                int count = this.NonCollapsedChildren.Count();

                availableSize = new Size(count * this.ItemWidth, this.ItemHeight);
            }
            else if (double.IsInfinity(availableSize.Width))
            {
                flag = true;

                int count = this.NonCollapsedChildren.Count();

                availableSize = new Size(count * this.ItemWidth, availableSize.Height);
            }
            else if (double.IsInfinity(availableSize.Height))
            {
                flag = true;

                int count = this.NonCollapsedChildren.Count();

                availableSize = new Size(availableSize.Width, count * this.ItemHeight);
            }

            if (this.Rows > 0)
            {
                if (this.Columns > 0)
                {
                    this._rows = this.Rows;
                    this._columns = this.Columns;
                }
                else if (this.CheckItemWidth())
                {
                    this._rows = this.Rows;
                    this._columns = Math.Max(1, (int)(availableSize.Width / this.ItemWidth));
                }
                else
                {
                    throw new Exception("无法确定子项大小。");
                }
            }
            else
            {
                if (this.Columns > 0)
                {
                    if (this.CheckItemHeight())
                    {
                        this._rows = Math.Max(1, (int)(availableSize.Height / this.ItemHeight));
                        this._columns = this.Columns;
                    }
                    else
                    {
                        throw new Exception("无法确定子项大小。");
                    }
                }
                else
                {
                    if (!this.CheckItemHeight()
                            || !this.CheckItemWidth())
                    {
                        throw new Exception("无法确定子项大小。");
                    }

                    this._rows = Math.Max(1, (int)(availableSize.Height / this.ItemHeight));
                    this._columns = Math.Max(1, (int)(availableSize.Width / this.ItemWidth));
                }
            }


            this._eachSize = new Size(availableSize.Width / this._columns, availableSize.Height / this._rows);

            this._sumCount = this.NonCollapsedChildren.Count();

            //实际元素个数所占用的实际行数和列数
            var _actualRows = Math.Ceiling((double)this._sumCount / this._columns);
            var _actualColumns = this._sumCount <= this._columns ? this._sumCount : this._columns;

            this._pageCount = this._rows * this._columns - this.AnchoredItemCount();

            this._pageCount = Math.Max(1, this._pageCount);

            int maxpage = this._sumCount / this._pageCount;
            if (this._sumCount % this._pageCount > 0)
            {
                maxpage++;
            }

            this.PageCount = maxpage;

            int currentpageindex = Math.Min(this.PageIndex, maxpage - 1);

            this._startIndex = currentpageindex * this._pageCount;
            this._endIndex = this._startIndex + this._pageCount - 1;

            this._currentPageCount = Math.Min(this._pageCount, this._sumCount - currentpageindex * this._pageCount);

            switch (this.ArrangeMode)
            {
                case ArrangeMode.Nomal:
                    this.MeasureCore();
                    break;
                case ArrangeMode.Lazy:
                    this._buffer.ReSet(null);
                    break;
                case ArrangeMode.LazyOneTime:
                    if (this._isMeasureArrangedFirstFlag)
                    {
                        this.MeasureCore();
                    }
                    else
                    {
                        this._buffer.ReSet(null);
                    }
                    break;
            }

            if (flag)
            {
                this._rows = (int)Math.Ceiling((double)this._sumCount / this._columns);

                availableSize = new Size(this._eachSize.Width * this._columns, this._eachSize.Height * this._rows);
            }

            if (this._sumCount < this._pageCount)
            {
                availableSize = new Size(this._eachSize.Width * _actualColumns, this._eachSize.Height * _actualRows);
            }

            this._actualSize = availableSize;

            this.UpdateScrollInfo();

            return availableSize;
        }

        private TimerBuffer<object> _buffer = null;
        private bool _isMeasureArrangedFirstFlag = false;
        protected override void OnArrangeModeChanged()
        {
            switch (this.ArrangeMode)
            {
                case ArrangeMode.Nomal:
                    if (this._buffer != null)
                    {
                        this._buffer.Stop();
                        this._buffer = null;
                    }
                    break;
                case ArrangeMode.Lazy:
                case ArrangeMode.LazyOneTime:
                    if (this._buffer == null)
                    {
                        _buffer = new TimerBuffer<object>(DispatcherPriority.Loaded);
                        _buffer.DueTime = 50;
                        _buffer.Action = this.MeasureArrange;
                    }
                    break;
            }
        }
        private void MeasureArrange(object obj)
        {
            this._isMeasureArrangedFirstFlag = true;
            this.MeasureCore();
            this.ArrangeCore();
        }

        private void MeasureCore()
        {
            //不在UI展示的不测量
            foreach (UIElement nonCollapsedChild in this.NonCollapsedChildren.Skip(this._startIndex).Take(this._pageCount))
            {
                nonCollapsedChild.Measure(this._eachSize);
            }
        }

        private void ArrangeCore()
        {
            Rect collapsed = new Rect(new Point(0, this._rows * _eachSize.Height), new Size(0, 0));

            int index = 0,
                    rowindex = 0,
                    columnindex = 0;

            foreach (UIElement element in this.NonCollapsedChildren)
            {
                if (index >= this._startIndex
                        && index <= this._endIndex)
                {
                    double x = columnindex * _eachSize.Width;
                    double y = rowindex * _eachSize.Height;

                    element.Arrange(new Rect(new Point(x, y), _eachSize));
                    element.SetValue(PaginationWrapPanel.IsArrangedProperty, true);

                    if (columnindex == this._columns - 1)
                    {
                        columnindex = 0;
                        rowindex++;

                        index++;
                        continue;
                    }

                    columnindex++;
                }
                else
                {
                    //已经布局过的已经出现在UI中了，需要将他移出布局
                    //尺寸按0处理
                    if ((bool)element.GetValue(PaginationWrapPanel.IsArrangedProperty))
                    {
                        element.Arrange(collapsed);
                    }
                }

                index++;
            }
        }

        public PaginationWrapPanel()
        {
            //解决线条的像素对齐
            this.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            //this.ArrangeMode = ArrangeMode.Nomal;

            this.OnArrangeModeChanged();
        }

        private int _rows, _columns;
        private Size _eachSize;
        private int _pageCount = 0, _sumCount = 0;
        private int _startIndex = 0, _endIndex = 0;
        private int _currentPageCount = 0;
        protected override Size ArrangeOverride(Size finalSize)
        {
            UIElement first = this.NonCollapsedChildren.FirstOrDefault();

            if (first != null)
            {
                switch (this.ArrangeMode)
                {
                    case ArrangeMode.Nomal:
                        this.ArrangeCore();
                        break;

                    case ArrangeMode.Lazy:
                        this._buffer.ReSet(null);
                        break;

                    case ArrangeMode.LazyOneTime:
                        if (this._isMeasureArrangedFirstFlag)
                        {
                            this.ArrangeCore();
                        }
                        else
                        {
                            this._buffer.ReSet(null);
                        }
                        break;
                }
            }

            //处理特殊项
            if (!this._isAnchoredItemInit
                    && this.AnchoredItem != null)
            {
                this._isAnchoredItemInit = true;
                this.AddVisualChild(this.AnchoredItem);
            }

            if (this.AnchoredItem != null)
            {
                this.AnchoredItem.Measure(this._eachSize);
                this.AnchoredItem.Arrange(new Rect(new Point((this._columns - 1) * this._eachSize.Width, (this._rows - 1) * this._eachSize.Height), this._eachSize));
            }

            //线条
            if (this.ShowGridLines
                    && this._controlLinesRenderer == null)
            {
                _controlLinesRenderer = new ControlLinesRenderer(this);
                this.AddVisualChild(_controlLinesRenderer);
            }
            if (this.ShowGridLines)
            {
                _controlLinesRenderer.UpdateRenderBounds(finalSize, this._rows, this._columns, this._currentPageCount);
            }

            return finalSize;
        }

        private Size _actualSize;

        private void UpdateScrollInfo()
        {
            this.ExtentWidth = _actualSize.Width * this.PageCount;
            this.ExtentHeight = _actualSize.Height * this.PageCount;

            if (this.ScrollOwner != null
                  && (this.CanHorizontallyScroll || this.CanVerticallyScroll))
            {
                this.ScrollOwner.InvalidateScrollInfo();
            }
        }

        public bool CanVerticallyScroll
        {
            get;
            set;
        }
        public bool CanHorizontallyScroll
        {
            get;
            set;
        }

        public double ExtentWidth
        {
            get;
            private set;
        }

        public double ExtentHeight
        {
            get;
            private set;
        }

        public double ViewportWidth => _actualSize.Width;

        public double ViewportHeight => _actualSize.Height;

        public double HorizontalOffset
        {
            get => this.PageIndex * this.ViewportWidth;
            private set
            {
                double count = value / this.ExtentWidth * this.PageCount;
                count = Math.Round(count, 0, MidpointRounding.AwayFromZero);
                this.PageIndex = (int)count;
            }
        }

        public double VerticalOffset
        {
            get => this.PageIndex * this.ViewportHeight;
            private set
            {
                double count = value / this.ExtentHeight * this.PageCount;
                count = Math.Round(count, 0, MidpointRounding.AwayFromZero);
                this.PageIndex = (int)count;
            }
        }

        private ScrollViewer _scrollOwner;
        public ScrollViewer ScrollOwner
        {
            get => this._scrollOwner;
            set => this._scrollOwner = value;
        }

        public void LineUp()
        {
        }

        public void LineDown()
        {
        }

        public void LineLeft()
        {
        }

        public void LineRight()
        {
        }

        public void PageUp()
        {
            this.PageIndex--;
        }

        public void PageDown()
        {
            this.PageIndex++;
        }

        public void PageLeft()
        {
            this.PageIndex--;
        }

        public void PageRight()
        {
            this.PageIndex++;
        }

        public void MouseWheelUp()
        {
            this.PageIndex--;
        }

        public void MouseWheelDown()
        {
            this.PageIndex++;
        }

        public void MouseWheelLeft()
        {
            this.PageIndex--;
        }

        public void MouseWheelRight()
        {
            this.PageIndex++;
        }

        public void SetHorizontalOffset(double offset)
        {
            this.HorizontalOffset = offset;
        }

        public void SetVerticalOffset(double offset)
        {
            this.VerticalOffset = offset;
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            return rectangle;
        }
    }
}
