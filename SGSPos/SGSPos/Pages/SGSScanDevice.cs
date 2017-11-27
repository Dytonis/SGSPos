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

        public SGSScanDevice(bool isRedeem = false)
        {
            this.isRedeem = isRedeem;
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

            textBox1.KeyPress += TextBox1_KeyPress;
            textBox1.Select();
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
                Service.SGSAPI2.GetTicketBatchPrintingResponse response = await Service.SGSAPI2.GetTicketBatchPrinting(textBox1.Text);

                SGSRedeem redeem = new SGSRedeem();

                try
                {
                    redeem.success = response.success;

                    if (response.success == true)
                    {
                        redeem.batch = textBox1.Text;
                        //if (redeem.winnings != null)
                        //{
                        //redeem.winnings = (decimal)(response.ticket.winamount).ToString("C");
                        //}

                        redeem.message = response.meta.userMessage;
                        redeem.cost = response.totalPrice.ToString("C");
                        redeem.winnings = response.totalWinAmount.ToString("C");
                        redeem.ticketCount = response.totalTickets.ToString();
                        redeem.ticketIDS = response.tickets.Select(x => x.id).Aggregate((i, j) => i + ", " + j);
                        redeem.status = "Pay ";
                        redeem.topLeft = "Batch Found";
                    }
                    else
                    {
                        //try ticket redeem instead
                        Service.SGSAPI.GetTicketResponse ticketResponse = await Service.SGSAPI.GetTicket(textBox1.Text);

                        if (ticketResponse.ticket != null)
                        {
                            redeem.topLeft = "Ticket Found";
                            redeem.message = ticketResponse.meta.userMessage + " However, this operation is currently missing data.";
                            redeem.cost = ticketResponse.ticket.betamount;
                            redeem.winnings = "";
                            redeem.ticketCount = "1";
                            redeem.ticketIDS = textBox1.Text;
                            redeem.batch = "Unavailable with current version.";
                            redeem.status = "Unavailable with current version.";
                        }
                        else
                        {
                            redeem.topLeft = "There was an error proccessing \'" + textBox1.Text + "\'";
                            redeem.ticketCount = "N/A";
                            redeem.winnings = "";
                            redeem.ticketIDS = "N/A";
                            redeem.message = response.error.message + " A ticket redeem was carried out and failed.";
                            redeem.cost = "N/A";
                            redeem.status = "There was an error.";
                        }
                    }
                }
                catch(Exception error)
                {
                    redeem.topLeft = "There was an error proccessing \'" + textBox1.Text + "\'";
                    redeem.ticketCount = "N/A";
                    redeem.winnings = "";
                    redeem.ticketIDS = "N/A";
                    redeem.message = "An unknown exception has occured.";
                    redeem.cost = "N/A";
                    redeem.status = "Unknown exception.";
                    redeem.success = false;

                    MessageBox.Show(error.Message + "\n" + error.InnerException + "\n" + error.StackTrace, "Error handled.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Switch(redeem as IPanelProvider);
            }
            else
            {
                Service.SGSAPI2.GetBatchForPaymentResponse response = await Service.SGSAPI2.GetBatchForPayment(textBox1.Text);

               /* List<Service.SGSAPI.GetTicketResponse> tickets = new List<Service.SGSAPI.GetTicketResponse>();

                foreach (string ticket in response.ticketids)
                {
                    Service.SGSAPI.GetTicketResponse ticketResponse = await Service.SGSAPI.GetTicket(ticket);
                    tickets.Add(ticketResponse);
                }*/

                SGSTicketOrder order = new SGSTicketOrder();
                //order.ticketsToUse = tickets;
                order.price = response.totalPrice.ToString("C");
                order.Batch = textBox1.Text;
                Switch(order as IPanelProvider);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Switch(new SGSHome());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text.Replace("Please Scan or Enter Purchase Code", "");
        }
    }
}
