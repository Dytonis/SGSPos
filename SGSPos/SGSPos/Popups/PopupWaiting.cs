using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using e3kdrv;
using System.IO;

namespace SGSPos.Popups
{
    public partial class PopupWaiting : Popup
    {
        public PopupWaiting(Pages.Page parent)
        {
            PanelParent = parent;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
