using System;
using System.Threading;
using System.Windows.Forms;

namespace SGSPos
{
    public partial class Application : Pages.Page
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x2000000;
                return cp;
            }
        }

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