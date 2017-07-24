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
    public partial class SGSScanDevice : Page, IPanelProvider
    {
        public bool isRedeem = false;

        public SGSScanDevice()
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
            //throw new NotImplementedException();

            textBox1.Select();
            textBox1.KeyPress += TextBox1_KeyPress;
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                button2_Click(null, null);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (isRedeem)
            {
                Service.SGSAPI.GetTicketResponse response = await Service.SGSAPI.GetTicket(textBox1.Text);

                SGSRedeem redeem = new SGSRedeem();

                try
                {
                    redeem.ticketID = response.ticket.ticketid;
                    redeem.winnings = response.ticket.winamount.ToString("C");
                    redeem.date = response.ticket.purchaseDate;
                    redeem.terminalID = "123";
                    redeem.game = response.ticket.gameid;
                    redeem.status = response.ticket.status.ToUpper();
                    redeem.topLeft = "Ticket Found";
                }
                catch
                {
                    redeem.topLeft = "Unable to find ticket \'" + textBox1.Text + "\'";
                    redeem.ticketID = "N/A";
                    redeem.winnings = "N/A";
                    redeem.date = "N/A";
                    redeem.terminalID = "N/A";
                    redeem.game = "N/A";
                    redeem.status = "INVALID TICKET";
                }

                Switch(redeem as IPanelProvider);
            }
            else
            {
                Service.SGSAPI.GetBatchResponse response = await Service.SGSAPI.GetBatch(textBox1.Text);

                List<Service.SGSAPI.GetTicketResponse> tickets = new List<Service.SGSAPI.GetTicketResponse>();

                foreach (string ticket in response.ticketids)
                {
                    Service.SGSAPI.GetTicketResponse ticketResponse = await Service.SGSAPI.GetTicket(ticket);
                    tickets.Add(ticketResponse);
                }

                SGSTicketOrder order = new SGSTicketOrder();
                order.ticketsToUse = tickets;
                order.Batch = textBox1.Text;
                Switch(order as IPanelProvider);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Switch(new SGSHome());
        }
    }
}
