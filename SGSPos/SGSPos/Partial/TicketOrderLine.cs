using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGSPos.Partial
{
    public partial class TicketOrderLine : UserControl
    {
        public Label TicketIDLabel
        {
            get
            {
                return label1;
            }
        }

        public Label TicketGameLabel
        {
            get
            {
                return label2;
            }
        }

        public Label TicketNumbersLabel
        {
            get
            {
                return label3;
            }
        }

        public Label TicketPriceLabel
        {
            get
            {
                return label4;
            }
        }
        public TicketOrderLine()
        {
            InitializeComponent();
        }
    }
}
