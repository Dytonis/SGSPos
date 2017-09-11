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
        public string gameID;
        public string topLeft;

        public string status;

        public SGSRedeem()
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
            label13.Text = ticketID;
            label12.Text = game + " [id: " + gameID + "]";
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
            SGSScanDevice device = new SGSScanDevice();
            device.isRedeem = true;
            Switch(device);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await Service.SGSAPI.RedeemTicket(ticketID, "test", "123");

            Switch(new SGSHome());
        }
    }
}
