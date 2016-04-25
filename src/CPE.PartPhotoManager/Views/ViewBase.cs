using System.ComponentModel;
using System.Windows.Controls;

namespace CPE.PartPhotoManager.Views
{
    public abstract class ViewBase : UserControl
    {
        public bool AlreadyLoaded { get; set; }

        public bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(this);
    }
}
