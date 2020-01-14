﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Unicorn.Utilities.Behaviours
{
    public interface IAttachedObject
    {
        DependencyObject AssociatedObject
        {
            get;
        }
        void Attach(DependencyObject dependencyObject);
        void Detach();
    }
}
