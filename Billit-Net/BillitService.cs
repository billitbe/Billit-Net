using Billit_Net.DTO;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Billit_Net
{
    public class BillitService
    {
        // Reusing the httpclient makes more sense
        private static readonly HttpClient _client = new();
        public AccountInformation AccountInformation { get; private set; }

        public BillitService(string apiKey, string baseUrl) 
        {
        //    _environment = baseUrl;
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Add("apiKey", apiKey);
        }
        public async Task<AccountInformation> ConnectAsync()
        {
            AccountInformation = await GetDataFromBillItAPI<AccountInformation>("/v1/account/accountInformation");
            return AccountInformation;
        }

        public List<Party> GetCustomers(string partyID = null, string odatafilter = null)
        {
            return GetParties(partyID, odatafilter).Result;
        }

        public List<Party> GetSuppliers(string partyID, string odatafilter)
        {
            return GetParties(partyID, odatafilter).Result;
        }

        public List<Order> GetIncomeInvoices(string partyID, string odatafilter)
        {
            return GetOrders(partyID, odatafilter).Result;
        }
        public List<Order> GetExpenses(string partyID, string odatafilter)
        {
            return GetOrders(partyID, "?$filter=OrderDirection eq 'Cost'&$orderby=OrderID+asc").Result;
        }

        public List<Order> GetIncomeCreditNotes(string partyID, string odatafilter)
        {
            return GetOrders(partyID, "?$filter=OrderDirection eq 'Income' and OrderType eq 'CreditNote'&$orderby=OrderID+asc").Result;
        }

        public List<Order> GetIncomeOffers(string partyID, string odatafilter)
        {
            return GetOrders(partyID, "?$filter=OrderDirection eq 'Income'and OrderType eq 'Offer'&$orderby=OrderID+asc").Result;
        }

        public PeppolParticipantInformation IsCompanyActiveOnPEPPOL(string VAT)
        {
            return GetDataFromBillItAPI<PeppolParticipantInformation>($"/v1/peppol/participantInformation/{VAT}").Result;
        }

        public void PushDocumentToFastInput(FileToProcess fileinfo)
        {
            var result = SendDataToBillItAPI("/v1/toProcess", JsonConvert.SerializeObject(fileinfo)).Result;
        }

        public string SendInvoiceViaXMLPEPPOL(string json)
        {
            return SendDataToBillItAPI("/v1/peppol/sendXML", json).Result;
        }

        public string SendInvoiceViaJsonPEPPOL(string json)
        {
            return SendDataToBillItAPI("/v1/peppol/sendOrder", json).Result;
        }

        public string CreateCustomer(string json)
        {
            return SendDataToBillItAPI("/v1/parties", json).Result;
        }

        public string UploadCODA(string CODA)
        {
            return SendDataToBillItAPI("/v1/financialTransaction/importFile", CODA).Result;
        }

        #region Class Helpers

        private async Task<List<Order>> GetOrders(string partyId, string odatafilter)
        {
            var orders = new List<Order>();
            var url = $"/v1/orders{odatafilter}";

            while (true)
            {
                var result = await GetDataFromBillItAPI<OrderCollection>(url, partyId);
                orders.AddRange(result.Items);
                if (String.IsNullOrEmpty(result.NextPageLink))
                    break;
                else url = result.NextPageLink;
            }

            return orders;
        }

        private async Task<List<Party>> GetParties(string partyId, string odatafilter = "?$orderby=LastModified+asc")
        {
            var parties = new List<Party>();
            var url = $"/v1/parties{odatafilter}";

            while (true)
            {
                var result = await GetDataFromBillItAPI<PartyCollection>(url, partyId);
                parties.AddRange(result.Items);
                if (String.IsNullOrEmpty(result.NextPageLink))
                    break;
                else url = result.NextPageLink;
            }

            return parties;
        }

        private async Task<string> SendDataToBillItAPI(string url, string payload)
        {
            HttpResponseMessage response = await _client.PostAsync(url, 
                new StringContent(payload, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            return resp;
        }

        private async Task<T> GetDataFromBillItAPI<T>(string url, string partyId = "")
        {
            _client.DefaultRequestHeaders.Add("partyID", partyId);
            return await _client.GetFromJsonAsync<T>(url);
        }

        #endregion

    }
}