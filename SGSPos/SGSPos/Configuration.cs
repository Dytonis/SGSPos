using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace SGSPos
{
    public class Configuration
    {
        static public int ImagePrintWidth = 640;
        static public bool UseDemoProcedure = false;

        public static void ReadConfig()
        {
            string json = "";

            if (File.Exists(@"config.json"))
            {
                json = File.ReadAllText(@"config.json");
            }
            else
            {
                ConfigResult w = new ConfigResult()
                {
                    ImagePrintWidth = ImagePrintWidth,
                    UseDemoProcedure = UseDemoProcedure,
                };

                string write = JsonConvert.SerializeObject(w);

                File.WriteAllText(@"config.json", write);

                MessageBox.Show("There was no configuration file. It has been made at the install location.", "Info", MessageBoxButtons.OK);
            }

            try
            {
                ConfigResult r = JsonConvert.DeserializeObject<ConfigResult>(json);

                if (json != null && r != null)
                {
                    ImagePrintWidth = r.ImagePrintWidth;
                    UseDemoProcedure = r.UseDemoProcedure;
                }
            }
            catch
            {
                ConfigResult w = new ConfigResult()
                {
                    ImagePrintWidth = ImagePrintWidth,
                    UseDemoProcedure = UseDemoProcedure,
                };

                string write = JsonConvert.SerializeObject(w);

                File.WriteAllText(@"config.json", write);

                MessageBox.Show("The configuration file was not in the right format. It has been re-made at the install location.", "Info", MessageBoxButtons.OK);
            }
        }
    }

    public class ConfigResult
    {
        public int ImagePrintWidth { get; set; }
        public bool UseDemoProcedure { get; set; }
    }
}
