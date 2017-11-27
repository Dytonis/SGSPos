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
    public partial class PopupChooseForm : Popup
    {
        public string[] ticketIds;
        public Action<int> action;

        public PopupChooseForm(Pages.Page parent, Action<int> callback)
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
            action.Invoke(1);
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            action.Invoke(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            action.Invoke(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            action.Invoke(2);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            action.Invoke(3);
        }
    }
}
