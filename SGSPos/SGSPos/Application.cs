using System;
using System.Threading;

namespace SGSPos
{
    public partial class Application : Pages.Page
    {
        public Application()
        {
            InitializeComponent();
        }

        private async void Application_Load(object sender, EventArgs e)
        {
            //await Service.SGSAPI.GetTicketImage();
            Switch(new SGSPos.Pages.SGSHome());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}