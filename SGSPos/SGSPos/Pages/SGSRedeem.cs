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
        public string batch;

        public string winnings;
        public string ticketIDS;
        public string ticketCount;
        public string message;
        public string cost;
        public string topLeft;

        public string status;

        public bool success;

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
            label13.Text = batch;
            label12.Text = cost;
            label11.Text = ticketCount;
            label10.Text = ticketIDS;
            label1.Text = message;

            label4.Text = status + winnings;
            label3.Text = topLeft;

            if (!success)
                button2.Enabled = false;
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
            Service.SGSAPI2.MarkRedeemedResponse response = await Service.SGSAPI2.MarkRedeemed(batch);

            if (response.markRedeemedSuccess == true)
            {
                Pop(this, new Popups.PopupChooseForm(this, (x) =>
                {
                    switch(x)
                    {
                        default:
                            break;
                    }
                }));
                Switch(new SGSHome());
            }
            else
            {
                while (MessageBox.Show("The redeem process was not successfull for batchid " + batch + ". " + (response.error == null ? "There was no error data given." : response?.error.message), "Error handled!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) != DialogResult.Cancel)
                {
                    Service.SGSAPI2.MarkRedeemedResponse response2 = await Service.SGSAPI2.MarkRedeemed(batch);

                    if (response2.markRedeemedSuccess == true)
                        Switch(new SGSHome());
                }

                MessageBox.Show("The batch could not be redeemed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Switch(new SGSHome());
            }
        }
    }
}
