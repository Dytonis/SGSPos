using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Drawing;
using System.Drawing.Imaging;
using e3kdrv;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace SGSPos.Service
{
    public class SGSAPI
    {
        public static string baseURI = "http://dev-tribal-api.shoutz.com/";

        public async static Task GetTicketImage(string ticketID) //TEMPORARY I HOPE
        {
            WebClient wc = new WebClient();
            byte[] data = wc.DownloadData(new Uri("http://dev-tribal-api.shoutz.com/getticketimg2.php?ticketid=" + ticketID + "&size=mdpi"));
            MemoryStream memstream = new MemoryStream(data);
            Image img = Image.FromStream(memstream);
            Bitmap bmp = ResizeImage(img, img.Width / 5, img.Height / 5);
            bmp = BitmapTo1Bpp(bmp);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            bmp.Save("C:\\Users\\tanne\\Desktop\\temp.bmp", ImageFormat.Bmp);
            //img = resize(img, new System.Drawing.Size(100, 100));
            Epic3000.InitializePrinter();
            Epic3000.DrawImage("C:\\Users\\tanne\\Desktop\\temp.bmp");
            Epic3000.LineFeed(15);
            Epic3000.CutPaper();
            Epic3000.SendBufferAndClear();
        }

        public async static Task GetTicketImageFromPath(string imagePath) //TEMPORARY I HOPE
        {
            //WebClient wc = new WebClient();
            //byte[] data = wc.DownloadData(new Uri("http://dev-tribal-api.shoutz.com/getticketimg2.php?ticketid=" + ticketID));
            System.IO.FileStream data = new System.IO.FileStream(imagePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Image img = Image.FromStream(data);
            Bitmap bmp = ResizeImage(img, (int)((float)img.Width / 1.05f), (int)((float)img.Height / 1.05f));
            bmp = BitmapTo1Bpp(bmp);
            MemoryStream ms = new MemoryStream();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            bmp.Save(path + "\\temp.bmp", ImageFormat.Bmp);
            //img = resize(img, new System.Drawing.Size(100, 100));
            Epic3000.InitializePrinter();
            Epic3000.SetCenterJustify();
            Epic3000.AppendBufferRaw(new byte[] { 27, 64, 27, 116, 0 });
            Epic3000.DrawImage(path + "\\temp.bmp");
            Epic3000.AppendBufferRaw(new byte[] { 13, 12 });
            Epic3000.LineFeed(5);
            Epic3000.CutPaper();
            Epic3000.SendBufferAndClear();
        }

        static Random r = new Random();
        public async static Task<byte[]> RandomBytes()
        {
            List<byte> list = new List<byte>();

            for(int i = 0; i < 12; i++)
            {
                list.Add((byte)(r.Next() % 256));
            }

            return list.ToArray();
        }

        public async static Task DemoGetSequentialTicket(int ticketsTotal, int tickets, int sequence)
        {
            List<String> filesList = new List<string>();

            for (int i = 0; i < ticketsTotal; i++)
            {
                filesList.Add(@"C:/SGSDemo/SGS" + i + ".png");
            }

            for(int i = sequence, t = 0; t < tickets; i++, t++)
            {
                if (i > ticketsTotal - 1)
                    i = 0;

                await GetTicketImageFromPath(@"C:/SGSDemo/SGS" + i + ".png");
            }
        }

        public static Bitmap BitmapTo1Bpp(Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format1bppIndexed);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            byte[] scan = new byte[(w + 7) / 8];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (x % 8 == 0) scan[x / 8] = 0;
                    Color c = img.GetPixel(x, y);
                    if (c.GetBrightness() >= 0.5) scan[x / 8] |= (byte)(0x80 >> (x % 8));
                }
                Marshal.Copy(scan, 0, (IntPtr)((long)data.Scan0 + data.Stride * y), scan.Length);
            }
            bmp.UnlockBits(data);
            return bmp;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public async static Task<GetBatchResponse> GetBatch(string batchID)
        {
            string api = baseURI + "api/v1/tickets/getbatch?batchid=" + batchID;

            //try
            //{
                using (var httpClient = new HttpClient())
                {
                    //try
                    //{
                        var httpResponse = await httpClient.GetAsync(api);
                        if (httpResponse.Content != null)
                        {
                            string responseContent = await httpResponse.Content.ReadAsStringAsync();
                            GetBatchResponse response = JsonConvert.DeserializeObject<GetBatchResponse>(responseContent);
                            return response;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    //}
                    //catch
                    //{
                    //    throw;
                    //}
                }
            //}
            //catch
            //{
            //    throw;
            //}
        }
        public async static Task<GetTicketResponse> GetTicket(string TicketID)
        {
            string api = baseURI + "api/v1/tickets/get?ticketid=" + TicketID;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var httpResponse = await httpClient.GetAsync(api);
                        if (httpResponse.Content != null)
                        {
                            string responseContent = await httpResponse.Content.ReadAsStringAsync();
                            GetTicketResponse response = JsonConvert.DeserializeObject<GetTicketResponse>(responseContent);
                            return response;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public class GetBatchResponse
        {
            public Meta meta;

            public decimal totalPrice;
            public int totalTickets;
            public string[] ticketids;          
        }

        public class GetTicketResponse
        {
            public Meta meta;
            public Ticket ticket;

            public class Ticket
            {
                public string ticketid { get; set; }
                public string gameid { get; set; }
                public string status { get; set; }
                public string betamount { get; set; }
                public string numbers { get; set; }
                public string winningnumbers { get; set; }
                public decimal winamount { get; set; }
                public string createDate { get; set; }
                public string purchaseDate { get; set; }
                public string redemptionDate { get; set; }
                public string signature { get; set; }
            }
        }

        public class Meta
        {
            public string code { get; set; }
            public string userMessage { get; set; }
        }
    }
}
