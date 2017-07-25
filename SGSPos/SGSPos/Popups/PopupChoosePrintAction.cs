using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using e3kdrv;
using System.IO;

namespace SGSPos.Popups
{
    public partial class PopupChoosePrintAction : Popup
    {
        public string[] ticketIds;

        public PopupChoosePrintAction(Pages.Page parent)
        {
            PanelParent = parent;
            InitializeComponent();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (Configuration.UseDemoProcedure == false)
            {
                foreach (string s in ticketIds)
                {
                    try
                    {
                        await Service.SGSAPI.GetTicketImage(s);
                    }
                    catch(Exception error)
                    {
                        this.Close();
                        //MessageBox.Show("Something went wrong! " + error.Message, "Error", MessageBoxButtons.OK);
                    }
                }
            }
            else
            {
                if (ticketIds.Length <= 0)
                    await Demo(1);
                else
                    await Demo(ticketIds.Length);
            }

            PanelParent.Switch(new Pages.SGSHome());
            Close();
        }

        private static async Task Demo(int tickets)
        {
            int sequence = 0;
            int ticketsTotal = 1;

            if (Directory.Exists(@"C:/SGSDemo"))
            {
                if (File.Exists(@"C:/SGSDemo/sequence.txt"))
                {
                    try
                    {
                        string read = File.ReadAllText(@"C:/SGSDemo/sequence.txt");

                        string[] splits = read.Split(',');

                        sequence = Convert.ToInt32(splits[1].Trim());
                        ticketsTotal = Convert.ToInt32(splits[0].Trim());
                    }
                    catch
                    {
                        MessageBox.Show("Could not read or convert read data from C:/SGSDemo/sequence.txt. Make sure the format is as follows: \'x,y\' where x is the amount of tickets, and y is the current position (leave at 0 if unsure). Name the ticket images SGS0.png, SGS1.png, SGS2.png etc...", "ERROR", MessageBoxButtons.OK);
                        return;
                    }
                }
                else
                {
                    File.WriteAllText(@"C:/SGSDemo/sequence.txt", "0,0");
                    MessageBox.Show("There was no storage file at C:/SGSDemo/sequence.txt. It was created. Please edit the first value (\'x,y\', x in this case) to be equal to the amount of ticket images present in the SGSDemo folder. The second value can be left at 0. Name the ticket images SGS0.png, SGS1.png, SGS2.png etc...");
                    return;
                }
            }
            else
            {
                Directory.CreateDirectory(@"C:/SGSDemo");
                File.WriteAllText(@"C:/SGSDemo/sequence.txt", "0,0");
                MessageBox.Show("There was no storage file at C:/SGSDemo/sequence.txt. It was created. Please edit the first value (\'x,y\', x in this case) to be equal to the amount of ticket images present in the SGSDemo folder. The second value can be left at 0. Name the ticket images SGS0.png, SGS1.png, SGS2.png etc...");
                return;
            }

            await Service.SGSAPI.DemoGetSequentialTicket(ticketsTotal, tickets, sequence);

            File.WriteAllText(@"C:/SGSDemo/sequence.txt", "" + ticketsTotal + "," + ((sequence + tickets) % ticketsTotal) + "");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PanelParent.Switch(new Pages.SGSHome());
            Close();
        }

        private void PopupChoosePrintAction_Load(object sender, EventArgs e)
        {

        }
    }
}
