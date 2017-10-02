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
        public Service.SGSAPI2.Ticket[] ticketsToUse;
        public List<string> ticketIdsToUse = new List<string>();
        public string Batch;
        public string price;

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
            if(String.IsNullOrWhiteSpace(price))
            {
                label2.Text = "Collect: No Data";
            }
            label2.Text = "Collect " + price;
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
                tableLayoutPanel1.RowCount = 1 + ticketsToUse.Length;
                try
                {
                    label2.Text = "Collect " + Convert.ToDecimal(ticketsToUse.Sum(x => Convert.ToDecimal(x.cost))).ToString("C");
                }
                catch
                {
                    label2.Text = "Collect $" + "error on conversion";
                }
                int rowDifference = tableLayoutPanel1.RowCount - tableLayoutPanel1.RowStyles.Count;
                if(rowDifference > 0)
                {
                    for (int i = 0; i < rowDifference; i++)
                        tableLayoutPanel1.RowStyles.Add(new RowStyle() { Height = 38 });
                }
                tableLayoutPanel1.Height = (38 * ticketsToUse.Length) + 38;
                foreach (Service.SGSAPI2.Ticket ticket in ticketsToUse)
                {
                    tableLayoutPanel1.RowStyles[index].Height = 38;
                    Partial.TicketOrderLine ticketLine = new Partial.TicketOrderLine();
                    ticketLine.TicketIDLabel.Text = ticket.id;
                    ticketLine.TicketGameLabel.Text = ticket.gamename;
                    if (String.IsNullOrWhiteSpace(ticket.getUserNumbers("-")))
                        ticketLine.TicketNumbersLabel.Text = "N/A";
                    else
                        ticketLine.TicketNumbersLabel.Text = ticket.getUserNumbers("-");

                    ticketLine.TicketIDLabel.ForeColor = Color.GhostWhite;
                    ticketLine.TicketNumbersLabel.ForeColor = Color.GhostWhite;
                    ticketLine.TicketGameLabel.ForeColor = Color.GhostWhite;
                    ticketLine.TicketPriceLabel.ForeColor = Color.GhostWhite;

                    ticketIdsToUse.Add(ticket.id);
                    try
                    {
                        ticketLine.TicketPriceLabel.Text = Convert.ToDecimal(ticket.cost).ToString("C");
                    }
                    catch
                    {
                        ticketLine.TicketPriceLabel.Text = ticket.cost.ToString();
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
            button2.Enabled = false;
            await Service.SGSAPI2.PosMarkPaid(Batch);
            Service.SGSAPI2.GetTicketBatchPrintingResponse response = await Service.SGSAPI2.GetTicketBatchPrinting(Batch);

            ticketsToUse = response.tickets;

            if(response.success == false)
            {
                MessageBox.Show(response.error.message, "Error handled!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button2.Enabled = true;
                return;
            }

            if(response.tickets == null)
            {
                MessageBox.Show("This batch has no tickets!", "Error handled!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button2.Enabled = true;
                return;
            }

            if (ticketsToUse.Length > 0)
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
