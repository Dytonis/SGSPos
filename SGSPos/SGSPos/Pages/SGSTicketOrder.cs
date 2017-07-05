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
    public partial class SGSTicketOrder : Page, IPanelProvider
    {
        public List<Service.SGSAPI.GetTicketResponse> ticketsToUse;
        public string Batch;

        public SGSTicketOrder()
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
            Partial.TicketOrderLine ticketOrderLine = new Partial.TicketOrderLine();
            ticketOrderLine.TicketIDLabel.Text = "Ticket ID";
            ticketOrderLine.TicketNumbersLabel.Text = "Numbers";
            ticketOrderLine.TicketGameLabel.Text = "Game";
            ticketOrderLine.TicketPriceLabel.Text = "Price";

            tableLayoutPanel1.Controls.Add(ticketOrderLine.TicketIDLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(ticketOrderLine.TicketGameLabel, 1, 0);
            tableLayoutPanel1.Controls.Add(ticketOrderLine.TicketNumbersLabel, 2, 0);
            tableLayoutPanel1.Controls.Add(ticketOrderLine.TicketPriceLabel, 3, 0);

            label1.Text = "Batch ID: " + Batch;

            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles[0].Height = 38;

            if (ticketsToUse != null)
            {
                int index = 1;
                tableLayoutPanel1.RowCount = 1 + ticketsToUse.Count;
                tableLayoutPanel1.RowStyles[index].Height = 38;
                tableLayoutPanel1.Height = (38 * ticketsToUse.Count) + 38;
                try
                {
                    label2.Text = "Collect " + Convert.ToDecimal(ticketsToUse.Sum(x => Convert.ToDecimal(x.ticket.betamount))).ToString("C");
                }
                catch
                {
                    label2.Text = "Collect $" + "error on conversion";
                }
                foreach (Service.SGSAPI.GetTicketResponse ticket in ticketsToUse)
                {
                    Partial.TicketOrderLine ticketLine = new Partial.TicketOrderLine();
                    ticketLine.TicketIDLabel.Text = ticket.ticket.ticketid;
                    ticketLine.TicketGameLabel.Text = ticket.ticket.gameid;
                    ticketLine.TicketNumbersLabel.Text = ticket.ticket.numbers;
                    try
                    {
                        ticketLine.TicketPriceLabel.Text = Convert.ToDecimal(ticket.ticket.betamount).ToString("C");
                    }
                    catch
                    {
                        ticketLine.TicketPriceLabel.Text = ticket.ticket.betamount;
                    }
                    tableLayoutPanel1.Controls.Add(ticketLine.TicketIDLabel, 0, index);
                    tableLayoutPanel1.Controls.Add(ticketLine.TicketGameLabel, 1, index);
                    tableLayoutPanel1.Controls.Add(ticketLine.TicketNumbersLabel, 2, index);
                    tableLayoutPanel1.Controls.Add(ticketLine.TicketPriceLabel, 33, index);

                    index++;
                }
            }
        }

        private void SGSTicketOrder_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Switch(new SGSHome());
        }
    }
}
