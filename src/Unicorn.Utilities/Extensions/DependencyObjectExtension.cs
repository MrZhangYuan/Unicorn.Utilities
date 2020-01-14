using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Unicorn.Utilities.Extensions
{
    static class DependencyObjectExtension
    {
        public static DependencyObject GetVisualOrLogicalParent(this DependencyObject sourceElement)
        {
            if (sourceElement == null)
            {
                return null;
            }
            if (sourceElement is Visual)
            {
                return VisualTreeHelper.GetParent(sourceElement) ?? LogicalTreeHelper.GetParent(sourceElement);
            }
            return LogicalTreeHelper.GetParent(sourceElement);
        }
    }

}
