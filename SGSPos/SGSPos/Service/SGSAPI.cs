using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http;

namespace SGSPos.Service
{
    public class SGSAPI
    {
        public static string baseURI = "http://test-eps-api.shoutz.com/";

        public async static Task<GetBatchResponse> GetBatch(string batchID)
        {
            string api = baseURI + "api/v1/tickets/getbatch?batchid=" + batchID;

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
                            GetBatchResponse response = JsonConvert.DeserializeObject<GetBatchResponse>(responseContent);
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
