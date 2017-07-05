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

        private void Application_Load(object sender, EventArgs e)
        {
            Switch(new SGSPos.Pages.SGSHome());
        }
    }
}