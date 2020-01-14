using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Unicorn.Utilities.Behaviours;

namespace Unicorn.Utilities
{
    public class WindowHelper
    {
        public static bool GetSaveWindowPosition(DependencyObject obj)
        {
            return (bool)obj.GetValue(SaveWindowPositionProperty);
        }
        public static void SetSaveWindowPosition(DependencyObject obj, bool value)
        {
            obj.SetValue(SaveWindowPositionProperty, value);
        }
        public static readonly DependencyProperty SaveWindowPositionProperty = DependencyProperty.RegisterAttached("SaveWindowPosition", typeof(bool), typeof(WindowHelper), new PropertyMetadata(false, callback));

        private static void callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    StylizedBehaviors.SetBehaviors(window, new StylizedBehaviorCollection
                    {
                            new WindowsSettingBehaviour()
                    });
                }
                else
                {
                    StylizedBehaviors.SetBehaviors(window, null);
                }
            }
        }
    }
}
