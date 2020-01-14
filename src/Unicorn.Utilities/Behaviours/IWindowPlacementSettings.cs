using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicorn.Utilities.Internal.Win32;

namespace Unicorn.Utilities.Behaviours
{
    internal interface IWindowPlacementSettings
    {
        WINDOWPLACEMENT Placement
        {
            get; set;
        }
        void Reload();
        void Save();
    }
}
