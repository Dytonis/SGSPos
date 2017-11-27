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
    public partial class SGSLogin : Page, IPanelProvider
    {
        public SGSLogin()
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

        private async void button1_Click(object sender, EventArgs e)
        {
            Popups.PopupWaiting waiter = new Popups.PopupWaiting(this);
            Pop(this, waiter);
            Service.User u = await Service.SGSAPI3.TryLogin(textBox2.Text, textBox1.Text);
            waiter.Close();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 4)
                button1.Enabled = true;
            else button1.Enabled = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Switch(new SGSHome());
        }
    }
}
