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
    public partial class SGSRedeem : Page, IPanelProvider
    {
        public string ticketID;

        public string winnings;
        public string terminalID;
        public string date;
        public string game;
        public string topLeft;

        public string status;

        public SGSRedeem()
        {
            InitializeComponent();
        }

        public Panel PanelToSwitch
        {
            get
            {
                return panel1;
            }
        }

        public void OnPageLoad()
        {
            label13.Text = ticketID;
            label12.Text = game;
            label11.Text = winnings;
            label10.Text = date;
            label1.Text = terminalID;

            label4.Text = status;
            label3.Text = topLeft;
        }

        private void SGSTicketOrder_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Switch(new SGSHome());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Switch(new SGSHome());
        }
    }
}
