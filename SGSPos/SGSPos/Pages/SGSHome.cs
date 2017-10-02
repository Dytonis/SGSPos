using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGSPos.Pages
{
    public partial class SGSHome : Page, IPanelProvider
    {
        public SGSHome()
        {
            InitializeComponent();
        }

        public SGSSwitchPanel PanelToSwitch
        {
            get
            {
                return panel1;
            }
        }

        public void OnPageLoad()
        {
            //throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Switch(new SGSScanDevice());
            Switch(new SGSScanDevice());
            Switch(new SGSScanDevice());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SGSScanDevice scan = new SGSScanDevice();
            scan.isRedeem = true;

            Switch(scan as IPanelProvider);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Would you like to close this application?", "SGS Pos", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                System.Windows.Forms.Application.Exit();
        }
    }
}
