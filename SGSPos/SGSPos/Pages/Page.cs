using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SGSPos.Pages
{
    /// <summary>
    /// The purpose of this class is to provide a cloneable instance of ApplicationPageSwitch for the editor. \nYou must override Panel 'PanelToSwitch.get"
    /// </summary>
    public class Page : ApplicationPageSwitch
    {
        /// <summary>
        /// Switches the current Program.App.Panel to the panel provided in IFormProvider 'provider'
        /// </summary>
        /// <param name="provider"></param>
        public override void Switch(IPanelProvider provider)
        {
            try
            {
                ClosePage(); //close the current page
                for (int i = 0; i < 100; i++) //100 control limit
                {
                    if (provider.PanelToSwitch.Controls.Count > 0)
                    {
                        Program.App.Controls.Add(provider.PanelToSwitch.Controls[0]);
                    }
                    else break;
                }

#if DEBUG
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Page.Switch called from " + this + " to " + provider);
#endif

                provider.OnPageLoad();
            }
            catch (Exception er)
            {
                MessageBox.Show("There was an error loading this page.\n\n" + er.Message);
            }
        }

        public DialogResult Pop(IWin32Window owner, Popups.Popup provider)
        {
            DialogResult r = provider.ShowDialog(owner);
            return r;
        }

        public override void ClosePage()
        {
            try
            {
                Program.App.Controls.Clear();
            }
            catch
            {

            }
        }

        public Page()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new System.Drawing.Size(1920, 1080);
        }
    }
}