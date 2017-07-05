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
            Service.SGSAPI.GetBatchResponse response = await Service.SGSAPI.GetBatch(textBox1.Text);

            List<Service.SGSAPI.GetTicketResponse> tickets = new List<Service.SGSAPI.GetTicketResponse>();

            foreach(string ticket in response.ticketids)
            {
                Service.SGSAPI.GetTicketResponse ticketResponse = await Service.SGSAPI.GetTicket(ticket);
                tickets.Add(ticketResponse);
            }

            SGSTicketOrder order = new SGSTicketOrder();
            order.ticketsToUse = tickets;
            order.Batch = textBox1.Text;
            Switch(order as IPanelProvider);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Switch(new SGSHome());
        }
    }
}
