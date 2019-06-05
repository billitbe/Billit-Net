using System;
using System.Linq;
using System.Net;
using Billit_Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class BillitServiceTest
    {
        string demo_apiKey = "8b5f64eb-76aa-40b5-a7c3-971f6d56a559";

        [TestMethod]
        public void BillitServiceValidKey()
        {
            BillitService service = new BillitService(demo_apiKey);
            Assert.IsTrue(service.AccountInformation.Companies.Count > 0, "There should be at least one company available for this API key");
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void BillitServiceInValidKey()
        {
            BillitService service = new BillitService(string.Empty);
        }

        [TestMethod]
        public void BillitService_GetCustomers()
        {
            BillitService service = new BillitService(demo_apiKey);
            string party = service.AccountInformation.Companies.First().PartyID; // we take the first company here for demo purposes
            var customers = service.GetCustomers(party); 
        }

        [TestMethod]
        public void BillitService_GetSuppliers()
        {
            BillitService service = new BillitService(demo_apiKey);
            string party = service.AccountInformation.Companies.First().PartyID; // we take the first company here for demo purposes
            var customers = service.GetSuppliers(party);
        }
    }
}
