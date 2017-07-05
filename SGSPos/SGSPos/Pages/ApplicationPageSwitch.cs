using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGSPos.Pages
{
    /// <summary>
    /// DO NOT EXTEND THIS CLASS. Use Page instead.
    /// </summary>
    public abstract class ApplicationPageSwitch : Form
    {
        abstract public void Switch(IPanelProvider provider);
        abstract public void ClosePage();
    }
}