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
using System.Windows.Forms;
using e3kdrv;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace SGSPos.Service
{
    public class SGSAPI2
    {
        public static string baseURI = "https://eps-api-2.shoutz.com/";

        public async static Task<T> GenericGet<T>(string api)
        {
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
                            T response = JsonConvert.DeserializeObject<T>(responseContent);
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
        public async static Task<T> GenericPost<T, R>(string api, R request)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                        var httpResponse = await httpClient.PostAsync(api, content);
                        if (httpResponse.Content != null)
                        {
                            string responseContent = await httpResponse.Content.ReadAsStringAsync();
                            T response = JsonConvert.DeserializeObject<T>(responseContent);
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

        public async static Task<GetBatchForPaymentResponse> GetBatchForPayment(string batchID)
        {
            string api = baseURI + "api/ticket/posGetTicketBatchPayment?batchid=" + batchID;

            try
            {
                return await GenericGet<GetBatchForPaymentResponse>(api);
            }
            catch
            {
                throw;
            }
        }

        public async static Task<MarkPaidResponse> PosMarkPaid(string batchid)
        {
            string api = baseURI + "api/cart/posMarkPaid?batchid=" + batchid;

            try
            {
                return await GenericGet<MarkPaidResponse>(api);
            }
            catch
            {
                throw;
            }
        }

        public async static Task<GetTicketBatchPrintingResponse> GetTicketBatchPrinting(string batchid)
        {
            string api = baseURI + "api/ticket/posGetTicketBatchPrinting?batchid=" + batchid;

            try
            {
                return await GenericGet<GetTicketBatchPrintingResponse>(api);
            }
            catch
            {
                throw;
            }
        }

        public async static Task<MarkRedeemedResponse> MarkRedeemed(string batchid, RedeemRequest request)
        {
            string api = baseURI + "api/cart/posMarkRedeemed?batchid=" + batchid;

            try
            {
                return await GenericPost<MarkRedeemedResponse, RedeemRequest>(api, request);
            }
            catch
            {
                throw;
            }
        }

        public class RedeemRequest
        {
            public string ticketid;
            public string agentid;
            public string terminalid;
        }

        public class posApiResponse
        {
            public bool success;
            public string apiEnvType;
        }

        public class MarkRedeemedResponse : posApiResponse
        {
            public bool markRedeemedSuccess;
        }

        public class MarkPaidResponse : posApiResponse
        {
            public string markPaidSuccess;
        }

        public class GetTicketBatchPrintingResponse : posApiResponse
        {
            public decimal totalPrice;
            public Meta meta;
            public int totalTickets;
            public string[] ticketids;
            public User user;
        }

        public class GetBatchForPaymentResponse : posApiResponse
        {
            public decimal totalPrice;
            public Meta meta;
            public int totalTickets;
        }

        public class Meta
        {
            public string code { get; set; }
            public string userMessage { get; set; }
            public string userId { get; set; }
        }
        public class User
        {
            public string tempid { get; set; }
        }
    }
}
