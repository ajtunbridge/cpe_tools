using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CPE.Sales.Views
{
    public abstract class ViewBase : UserControl
    {
        public bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(this);
    }
}
