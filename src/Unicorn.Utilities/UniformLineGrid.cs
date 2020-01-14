using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Unicorn.Utilities
{
    /// <summary>
    /// 包含分割线的UniformGrid
    /// </summary>
    public class UniformLineGrid : UniformGrid
    {
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
        public static readonly DependencyProperty ShowGridLinesProperty = DependencyProperty.Register("ShowGridLines", typeof(bool), typeof(UniformLineGrid), new PropertyMetadata(false, ShowGridLinesChanged));

        private static void ShowGridLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UniformLineGrid grid = (UniformLineGrid)d;
            grid.InvalidateVisual();
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

        public double LineStyle
        {
            get
            {
                return (double)GetValue(ControlLinesRenderer.LineStyleProperty);
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

        private ControlLinesRenderer _controlLinesRenderer = null;

        protected override Visual GetVisualChild(int index)
        {
            if (index != base.VisualChildrenCount)
            {
                return (Visual)base.GetVisualChild(index);
            }

            if (this._controlLinesRenderer == null)
                throw new ArgumentOutOfRangeException(nameof(index), (object)index, "Visual_ArgumentOutOfRange");
            return (Visual)this._controlLinesRenderer;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return base.VisualChildrenCount + (this._controlLinesRenderer != null ? 1 : 0);
            }
        }

        public UniformLineGrid()
        {
            //解决线条的像素对齐
            this.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var size = base.ArrangeOverride(arrangeSize);

            if (this.ShowGridLines
                    && this._controlLinesRenderer == null)
            {
                _controlLinesRenderer = new ControlLinesRenderer(this);
                this.AddVisualChild(_controlLinesRenderer);
            }

            if (this.ShowGridLines)
            {
                _controlLinesRenderer.UpdateRenderBounds(arrangeSize, this.Rows, this.Columns);
            }

            return size;
        }
    }

}
