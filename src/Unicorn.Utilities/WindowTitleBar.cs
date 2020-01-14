using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Unicorn.Utilities
{
    public class WindowTitleBar : Border, INonClientArea
    {
        public bool IsSystemMenuEnabled
        {
            get
            {
                return (bool)GetValue(IsSystemMenuEnabledProperty);
            }
            set
            {
                SetValue(IsSystemMenuEnabledProperty, value);
            }
        }
        public static readonly DependencyProperty IsSystemMenuEnabledProperty = DependencyProperty.Register(
                "IsSystemMenuEnabled",
                typeof(bool),
                typeof(WindowTitleBar),
                new PropertyMetadata(true));

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }
        int INonClientArea.HitTest(System.Windows.Point point)
        {
            return 2;
        }
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            if (this.IsSystemMenuEnabled
                    && !e.Handled)
            {
                HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
                if (hwndSource != null)
                {
                    CustomChromeWindow.ShowWindowMenu(hwndSource, this, Mouse.GetPosition(this), base.RenderSize);
                }
                e.Handled = true;
            }
        }
    }

}
