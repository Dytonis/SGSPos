using System;
using System.Windows.Forms;

namespace SGSPos.Popups
{
    public partial class Popup : Form
    {
        public Popup()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
        }

        public DialogResult Pop(IWin32Window owner, Popups.Popup provider)
        {
            DialogResult r = provider.ShowDialog(owner);
            return r;
        }
    }

    public interface IPopProvider
    {
        void OnPageLoad();
    }
}