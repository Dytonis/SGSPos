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
    public partial class PopupTellCashDrawer : Popup
    {
        public string[] ticketIds;
        public Action action;

        public PopupTellCashDrawer(Pages.Page parent, Action callback)
        {
            action = callback;
            PanelParent = parent;
            InitializeComponent();
        }

        private void PopupChoosePrintAction_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            action.Invoke();
            Close();
        }
    }
}
