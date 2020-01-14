using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Unicorn.Utilities
{
    public enum LineStyle
    {
        /// <summary>
        /// 实线
        /// </summary>
        Solid,
        /// <summary>
        /// 虚线
        /// </summary>
        Dotted
    }

    public class ControlLinesRenderer : DrawingVisual
    {
        public static Brush GetLineBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(LineBrushProperty);
        }
        public static void SetLineBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(LineBrushProperty, value);
        }
        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.RegisterAttached("LineBrush", typeof(Brush), typeof(ControlLinesRenderer), new PropertyMetadata(null));



        public static double GetLineThickness(DependencyObject obj)
        {
            return (double)obj.GetValue(LineThicknessProperty);
        }
        public static void SetLineThickness(DependencyObject obj, double value)
        {
            obj.SetValue(LineThicknessProperty, value);
        }
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.RegisterAttached("LineThickness", typeof(double), typeof(ControlLinesRenderer), new PropertyMetadata(0d));


        public static LineStyle GetLineStyle(DependencyObject obj)
        {
            return (LineStyle)obj.GetValue(LineStyleProperty);
        }
        public static void SetLineStyle(DependencyObject obj, LineStyle value)
        {
            obj.SetValue(LineStyleProperty, value);
        }
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.RegisterAttached("LineStyle", typeof(LineStyle), typeof(ControlLinesRenderer), new PropertyMetadata(LineStyle.Solid));


        private readonly Pen _pen;

        private DependencyObject _target = null;
        public ControlLinesRenderer(DependencyObject target)
        {
            this._target = target;
            if (this._target != null)
            {
                Brush brush = this._target.GetValue(ControlLinesRenderer.LineBrushProperty) as Brush;
                object thincknessobj = this._target.GetValue(ControlLinesRenderer.LineThicknessProperty);
                object linestyleobj = this._target.GetValue(ControlLinesRenderer.LineStyleProperty);

                if (brush != null
                        && thincknessobj != null
                        && linestyleobj != null)
                {
                    double thinckness = (double)thincknessobj;
                    LineStyle linestyle = (LineStyle)linestyleobj;

                    _pen = new Pen(brush, thinckness);

                    switch (linestyle)
                    {
                        case LineStyle.Solid:
                            break;
                        case LineStyle.Dotted:
                            _pen.DashStyle = new DashStyle
                            {
                                Dashes = new DoubleCollection { 4, 4 }
                            };
                            break;
                    }

                    _pen.Freeze();
                }

                //_pen = new Pen(linebrush, thickness);
            }
        }

        public ControlLinesRenderer(Brush linebrush, double thickness)
        {
            _pen = new Pen(linebrush, thickness);
            _pen.DashStyle = new DashStyle
            {
                Dashes = new DoubleCollection
                                 {
                                         4,4
                                 }
            };
            _pen.Freeze();
        }

        public void UpdateRenderBounds(Size boundsSize, int rows, int columns, int currentpagecount)
        {
            using (DrawingContext drawingContext = this.RenderOpen())
            {
                if (rows < 1
                        || columns < 1)
                {
                    return;
                }

                int sumcount = rows * columns;

                int actfullrows = (int)Math.Ceiling((double)currentpagecount / columns);

                int lastrowcount = currentpagecount % columns;

                int fulllinerows = lastrowcount == 0 ? actfullrows : actfullrows - 1,
                        fulllinecolumns = lastrowcount == 0 ? columns - 1 : lastrowcount;

                //去除满页时最后一行
                fulllinerows = fulllinerows == rows ? fulllinerows - 1 : fulllinerows;

                //横向满行
                double yoffset = 0, yrange = boundsSize.Height / rows;
                for (int i = 1; i <= fulllinerows; i++)
                {
                    yoffset += yrange;
                    DrawGridLine(drawingContext, 0.0, yoffset, boundsSize.Width, yoffset);
                }

                //纵向满行
                double y = yrange * actfullrows;
                double xoffset = 0, xrange = boundsSize.Width / columns;
                for (int i = 1; i <= fulllinecolumns; i++)
                {
                    xoffset = xoffset + xrange;
                    DrawGridLine(drawingContext, xoffset, 0.0, xoffset, y);
                }

                //补最后一行X
                yoffset += yrange;
                DrawGridLine(drawingContext, 0.0, yoffset, lastrowcount * xrange, yoffset);

                //补Y
                y = lastrowcount == 0 ? y : y - yrange;
                for (int i = fulllinecolumns + 1; i < columns; i++)
                {
                    xoffset = xoffset + xrange;
                    DrawGridLine(drawingContext, xoffset, 0.0, xoffset, y);
                }
            }
            //using (DrawingContext drawingContext = this.RenderOpen())
            //{
            //        int stoprow = 1;
            //        int sum = 0;
            //        double yoffset = 0, yrange = boundsSize.Height / rows;
            //        for (int index = 1; index < rows; ++index)
            //        {
            //                sum += columns;
            //                yoffset += yrange;
            //                if (sum <= currentpagecount || (sum > currentpagecount && sum - columns < currentpagecount))
            //                {
            //                        stoprow = index + 1 == rows && sum <= currentpagecount ? index + 1 : index;
            //                        DrawGridLine(drawingContext, 0.0, yoffset, boundsSize.Width, yoffset);
            //                }
            //        }

            //        double xoffset = 0, xrange = boundsSize.Width / columns;
            //        double y = yrange * stoprow;
            //        for (int index = 1; index < columns; ++index)
            //        {
            //                xoffset = xoffset + xrange;
            //                DrawGridLine(drawingContext, xoffset, 0.0, xoffset, y);
            //        }
            //}
        }

        public void UpdateRenderBounds(Size boundsSize, int rows, int columns)
        {
            using (DrawingContext drawingContext = this.RenderOpen())
            {
                double xoffset = 0, xrange = boundsSize.Width / columns;
                for (int index = 1; index < columns; ++index)
                {
                    xoffset = xoffset + xrange;
                    DrawGridLine(drawingContext, xoffset, 0.0, xoffset, boundsSize.Height);
                }

                double yoffset = 0, yrange = boundsSize.Height / rows;
                for (int index = 1; index < rows; ++index)
                {
                    yoffset += yrange;
                    DrawGridLine(drawingContext, 0.0, yoffset, boundsSize.Width, yoffset);
                }
            }
        }

        private void DrawGridLine(DrawingContext drawingContext, double startX, double startY, double endX, double endY)
        {
            Point point0 = new Point(startX, startY);
            Point point1 = new Point(endX, endY);
            this.DrawGridLine(drawingContext, point0, point1);
        }

        private void DrawGridLine(DrawingContext drawingContext, Point start, Point end)
        {
            drawingContext.DrawLine(this._pen, start, end);
        }
    }
}
