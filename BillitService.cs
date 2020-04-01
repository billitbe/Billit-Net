using Billit_Net.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Billit_Net
{
    public class BillitService
    {

        const string environment = "https://api.sandbox.billit.be";


        const string URLACCOUNTINFORMATION = environment+"/v1/account/accountInformation";
        const string URLPARTIES = environment + "/v1/parties";
        const string URLORDERS = environment + "/v1/orders";
        const string ACCEPTHEADER = "Accept";
        const string RESPONSEHEADER = "Content-type";
        const string ACCEPTHEADERJSON = "application/json";
        const string RESPONSECONTENTTYPE = "application/json";
        const string PARTYIDHEADER = "partyID";
        const string API_KEYHEADER = "apiKey";

        private string m_apiKey;
        private AccountInformation m_accountinfo;

        public AccountInformation AccountInformation { get { return m_accountinfo; } }

        public BillitService(string apiKey)
        {
            this.m_apiKey = apiKey;
            Connect();
        }

        public IEnumerable<Party> GetParties(string partyID, string odatafilter = "?$orderby=LastModified+asc")
        {
            if (string.IsNullOrEmpty(partyID)) partyID = AccountInformation.Companies[0].PartyID;

            List<Party> parties = new List<Party>();
            string url = URLPARTIES + odatafilter;
            using (var client = new WebClient())
            {
                do
                {
                    var headers = new WebHeaderCollection
                    {
                        { ACCEPTHEADER, ACCEPTHEADERJSON },
                        { API_KEYHEADER, this.m_apiKey },
                        { PARTYIDHEADER, partyID }
                    };
                    client.Headers = headers;
                    var json = client.DownloadString(url);
                    var serializer = new JavaScriptSerializer();
                    var result = serializer.Deserialize<PartyCollection>(json);
                    url = result.NextPageLink;
                    parties.AddRange(result.Items);
                } while (!string.IsNullOrEmpty(url));
            }
            return parties;
        }

        public IEnumerable<Party> GetCustomers(string partyID=null, string odatafilter=null)
        {
            var customers = GetParties(partyID, odatafilter);
            return customers;
        }

        public IEnumerable<Party> GetSuppliers(string partyID, string odatafilter)
        {
            var suppliers = GetParties(partyID, odatafilter);
            return suppliers;
        }

        public ICollection<Order> GetOrders(string partyID, string odatafilter)
        {
            List<Order> orders = new List<Order>();
            string url = URLORDERS + odatafilter;
            using (var client = new WebClient())
            {
                do
                {
                    var headers = new WebHeaderCollection
                    {
                        { ACCEPTHEADER, ACCEPTHEADERJSON },
                        { API_KEYHEADER, this.m_apiKey },
                        { PARTYIDHEADER, partyID }
                    };
                    client.Headers = headers;
                    var json = client.DownloadString(url);
                    var serializer = new JavaScriptSerializer();
                    var result = serializer.Deserialize<OrderCollection>(json);
                    url = result.NextPageLink;
                    orders.AddRange(result.Items);
                } while (!string.IsNullOrEmpty(url));
            }
            return orders;
        }

        public ICollection<Order> GetIncomeInvoices(string partyID, string odatafilter)
        {
            var incomes = GetOrders(partyID, odatafilter);
            return incomes;
        }

        public ICollection<Order> GetIncomeCreditNotes(string partyID, string odatafilter)
        {
            var incomes = GetOrders(partyID, "?$filter=OrderDirection eq 'Income' and OrderType eq 'CreditNote'&$orderby=OrderID+asc");
            return incomes;
        }

        public ICollection<Order> GetIncomeOffers(string partyID, string odatafilter)
        {
            var incomes = GetOrders(partyID, "?$filter=OrderDirection eq 'Income'and OrderType eq 'Offer'&$orderby=OrderID+asc");
            return incomes;
        }

        public ICollection<Order> GetExpenses(string partyID, string odatafilter)
        {
            var expenses = GetOrders(partyID, "?$filter=OrderDirection eq 'Cost'&$orderby=OrderID+asc");
            return expenses;
        }

        public PeppolParticipantInformation IsCompanyActiveOnPEPPOL(string VAT)
        {
            PeppolParticipantInformation result = null;
            var url = string.Format(environment+"/v1/peppol/participantInformation/{0}", VAT);
            using (var client = new WebClient())
            {
                var headers = new WebHeaderCollection
                    {
                        { ACCEPTHEADER, ACCEPTHEADERJSON },
                        { API_KEYHEADER, this.m_apiKey }
                    };
                client.Headers = headers;
                var json = client.DownloadString(url);
                var serializer = new JavaScriptSerializer();
                result = serializer.Deserialize<PeppolParticipantInformation>(json);
            }

            return result;
        }

        public void PushDocumentToFastInput(FileToProcess fileinfo)
        {
            var url = string.Format(environment+"/v1/toProcess");

            using (var client = new WebClient())
            {
                var headers = new WebHeaderCollection
                    {
                        { ACCEPTHEADER, ACCEPTHEADERJSON },
                        { API_KEYHEADER, this.m_apiKey },
                        { RESPONSEHEADER, RESPONSECONTENTTYPE }
                    };
                client.Headers = headers;
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(fileinfo);
                var result = client.UploadString(url, json);
            }
        }
        public string SendInvoiceViaXMLPEPPOL(string json)
        {
            var url = string.Format(environment + "/v1/peppol/sendXML");
            var responseID = string.Empty;

            using (var client = new WebClient())
            {
                var headers = new WebHeaderCollection
                    {
                        { ACCEPTHEADER, ACCEPTHEADERJSON },
                        { API_KEYHEADER, this.m_apiKey },
                        { RESPONSEHEADER, RESPONSECONTENTTYPE }
                    };
                client.Headers = headers;
                var serializer = new JavaScriptSerializer();
                responseID = client.UploadString(url, json);
            }

            return responseID;
        }

        public string SendInvoiceViaJsonPEPPOL(string json)
        {
                var responseID = string.Empty;
           
                var url = string.Format(environment + "/v1/peppol/sendOrder");

                using (var client = new WebClient())
                {
                    var headers = new WebHeaderCollection
                    {
                        { ACCEPTHEADER, ACCEPTHEADERJSON },
                        { API_KEYHEADER, this.m_apiKey },
                        { RESPONSEHEADER, RESPONSECONTENTTYPE }
                    };
                    client.Headers = headers;
                    var serializer = new JavaScriptSerializer();
                    responseID = client.UploadString(url, json);
                }
            return responseID;
        }

        private void Connect()
        {
            using (var client = new WebClient())
            {
                var headers = new WebHeaderCollection
                {
                    { ACCEPTHEADER, ACCEPTHEADERJSON },
                    { API_KEYHEADER, this.m_apiKey }
                };
                client.Headers = headers;
                var json = client.DownloadString(URLACCOUNTINFORMATION);
                var serializer = new JavaScriptSerializer();
                m_accountinfo = serializer.Deserialize<AccountInformation>(json);
            }
        }

        public void CreateCustomer(string json)
        {
            var url = string.Format(environment + "/v1/parties");
            var responseID = string.Empty;

            using (var client = new WebClient())
            {
                var headers = new WebHeaderCollection
                    {
                        { ACCEPTHEADER, ACCEPTHEADERJSON },
                        { API_KEYHEADER, this.m_apiKey },
                        { RESPONSEHEADER, RESPONSECONTENTTYPE }
                    };
                client.Headers = headers;
                var serializer = new JavaScriptSerializer();
                responseID = client.UploadString(url, json);
            }

        }

        public void UploadCODA(string CODA)
        {
            var url = string.Format(environment + "/v1/financialTransaction/importFile");
            var responseID = string.Empty;

            using (var client = new WebClient())
            {
                var headers = new WebHeaderCollection
                    {
                        { ACCEPTHEADER, ACCEPTHEADERJSON },
                        { API_KEYHEADER, this.m_apiKey },
                        { RESPONSEHEADER, RESPONSECONTENTTYPE }
                    };
                client.Headers = headers;
                var serializer = new JavaScriptSerializer();
                responseID = client.UploadString(url, CODA);
            }
        }
    }
}
