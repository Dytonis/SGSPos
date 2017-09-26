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
        public List<string> ticketIdsToUse = new List<string>();
        public string Batch;

        public SGSTicketOrder()
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
            button3.Enabled = false;
            label1.Text = Batch;
            label2.Text = "Collect: Complete order to get total.";
        }

        public void RenderTable()
        {
            Partial.TicketOrderLine ticketOrderLine = new Partial.TicketOrderLine();
            ticketOrderLine.TicketIDLabel.Text = "Ticket ID";
            ticketOrderLine.TicketNumbersLabel.Text = "Numbers";
            ticketOrderLine.TicketGameLabel.Text = "Game";
            ticketOrderLine.TicketPriceLabel.Text = "Price";

            ticketOrderLine.TicketIDLabel.ForeColor = Color.LightCoral;
            ticketOrderLine.TicketNumbersLabel.ForeColor = Color.LightCoral;
            ticketOrderLine.TicketGameLabel.ForeColor = Color.LightCoral;
            ticketOrderLine.TicketPriceLabel.ForeColor = Color.LightCoral;

            tableLayoutPanel1.Controls.Add(ticketOrderLine.TicketIDLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(ticketOrderLine.TicketGameLabel, 1, 0);
            tableLayoutPanel1.Controls.Add(ticketOrderLine.TicketNumbersLabel, 2, 0);
            tableLayoutPanel1.Controls.Add(ticketOrderLine.TicketPriceLabel, 3, 0);

            label1.Text = Batch;

            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles[0].Height = 38;

            if (ticketsToUse != null)
            {
                int index = 1;
                tableLayoutPanel1.RowCount = 1 + ticketsToUse.Count;
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
                    tableLayoutPanel1.RowStyles[index].Height = 38;
                    Partial.TicketOrderLine ticketLine = new Partial.TicketOrderLine();
                    ticketLine.TicketIDLabel.Text = ticket.ticket.ticketid;
                    ticketLine.TicketGameLabel.Text = ticket.ticket.gamename;
                    if (String.IsNullOrWhiteSpace(ticket.ticket.numbers))
                        ticketLine.TicketNumbersLabel.Text = "N/A";
                    else
                        ticketLine.TicketNumbersLabel.Text = ticket.ticket.numbers;

                    ticketLine.TicketIDLabel.ForeColor = Color.GhostWhite;
                    ticketLine.TicketNumbersLabel.ForeColor = Color.GhostWhite;
                    ticketLine.TicketGameLabel.ForeColor = Color.GhostWhite;
                    ticketLine.TicketPriceLabel.ForeColor = Color.GhostWhite;

                    ticketIdsToUse.Add(ticket.ticket.ticketid);
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

        private async void button2_Click(object sender, EventArgs e)
        {
            await Service.SGSAPI2.PosMarkPaid(Batch);
            Service.SGSAPI2.GetTicketBatchPrintingResponse response = await Service.SGSAPI2.GetTicketBatchPrinting(Batch);

            ticketsToUse = new List<Service.SGSAPI.GetTicketResponse>();

            foreach (string ticket in response.ticketids)
            {
                Service.SGSAPI.GetTicketResponse ticketResponse = await Service.SGSAPI.GetTicket(ticket);
                ticketResponse.ticket.ticketid = ticket;
                ticketsToUse.Add(ticketResponse);
            }

            if (ticketsToUse.Count > 0)
            {
                RenderTable();
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
            //Popups.PopupChoosePrintAction popup = new Popups.PopupChoosePrintAction(this);
            //popup.ticketIds = ticketIdsToUse.ToArray();
            //popup.Pop(this, popup);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Popups.PopupChoosePrintAction popup = new Popups.PopupChoosePrintAction(this);
            popup.ticketIds = ticketIdsToUse.ToArray();
            popup.Pop(this, popup);
        }
    }
}
