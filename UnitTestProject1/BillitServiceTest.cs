using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Collections;
using System.Linq;
using Billit_Net;

namespace Billit.SDK.Test
{
    [TestClass]
    public class BillitServiceTest
    {
        const string APIKEY_VALID = "8b5f64eb-76aa-40b5-a7c3-971f6d56a559"; // You should add the API key of your development account here
        const string APIKEY_INVALID = "8b5f64eb-0000-0000-0000-971f6d56a559";
        const int PartyIDIndex = 0;

        [TestMethod]
        public void BillitService_AuthenticationTest()
        {
            var service = new BillitService(APIKEY_VALID);
            Assert.IsTrue(service.AccountInformation.Companies.Count != 0);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void BillitService_Authentication_FalseKeyTest()
        {
            var service = new BillitService(APIKEY_INVALID);
        }

        [TestMethod]
        public void BillitService_GetPartiesTest()
        {
            var service = new BillitService(APIKEY_VALID);
            var partyCollection = service.GetParties(service.AccountInformation.Companies[PartyIDIndex].PartyID);
            Assert.IsTrue(service.AccountInformation.Companies.Count != 0);
        }

        [TestMethod]
        public void BillitService_GetSuppliersTest()
        {
            var service = new BillitService(APIKEY_VALID);
            var suppliers = service.GetSuppliers(service.AccountInformation.Companies[PartyIDIndex].PartyID);

            Assert.IsTrue(suppliers.Where(s => s.PartyType == "Customer").Count() == 0, "There should not be parties with PartyType Customer in GetSuppliers");
        }

        [TestMethod]
        public void BillitService_GetCustomersTest()
        {
            var service = new BillitService(APIKEY_VALID);
            var customers = service.GetCustomers(service.AccountInformation.Companies[PartyIDIndex].PartyID);
            Assert.IsTrue(customers.Where(s => s.PartyType == "Supplier").Count() == 0, "There should not be parties with PartyType Supplier in GetCustomers");
        }

        [TestMethod]
        public void BillitService_GetCustomersNoPartyIDTest()
        {
            var service = new BillitService(APIKEY_VALID);
            var customers = service.GetCustomers();
            Assert.IsTrue(customers.Where(s => s.PartyType == "Supplier").Count() == 0, "There should not be parties with PartyType Supplier in GetCustomers");
        }

        [TestMethod]
        public void BillitService_GetOrdersTest()
        {
            var service = new BillitService(APIKEY_VALID);
            var orders = service.GetOrders(service.AccountInformation.Companies[PartyIDIndex].PartyID, string.Empty);
            Assert.IsTrue(orders.Count() != 0);
        }

        [TestMethod]
        public void BillitService_GetIncomesTest()
        {
            var service = new BillitService(APIKEY_VALID);
            var incomes = service.GetIncomeInvoices(service.AccountInformation.Companies[PartyIDIndex].PartyID, string.Empty);
            Assert.IsTrue(incomes.Count() != 0);
            Assert.IsTrue(incomes.Where(s => s.OrderDirection == "Cost").Count() == 0, "There should not be orders with OrderDirection Expense in GetIncome");
        }

        [TestMethod]
        public void BillitService_GetIncomeInvoicesTest()
        {
            var service = new BillitService(APIKEY_VALID);
            var incomes = service.GetIncomeInvoices(service.AccountInformation.Companies[PartyIDIndex].PartyID, string.Empty);
            Assert.IsTrue(incomes.Count() != 0);
            Assert.IsTrue(incomes.Where(s => s.OrderType != "Invoice").Count() == 0, "There should only be income invoices in GetIncomeInvoice");
        }

        [TestMethod]
        public void BillitService_GetIncomeOffersTest()
        {
            var service = new BillitService(APIKEY_VALID);
            var incomes = service.GetIncomeOffers(service.AccountInformation.Companies[PartyIDIndex].PartyID, string.Empty);
            Assert.IsTrue(incomes.Count() != 0);
            Assert.IsTrue(incomes.Where(s => s.OrderType != "Offer").Count() == 0, "There should only be income offers in GetIncomeOffers");
        }

        [TestMethod]
        public void BillitService_GetIncomeCreditNotesTest()
        {
            var service = new BillitService(APIKEY_VALID);
            var incomes = service.GetIncomeCreditNotes(service.AccountInformation.Companies[PartyIDIndex].PartyID, string.Empty);
            Assert.IsTrue(incomes.Count() != 0);
            Assert.IsTrue(incomes.Where(s => s.OrderType != "CreditNote").Count() == 0, "There should only be income creditnotes in GetIncomeCreditNotes");
        }

        [TestMethod]
        public void BillitService_GetExpenses()
        {
            var service = new BillitService(APIKEY_VALID);
            var expenses = service.GetExpenses(service.AccountInformation.Companies[PartyIDIndex].PartyID, string.Empty);
            Assert.IsTrue(expenses.Count() != 0);
            Assert.IsTrue(expenses.Where(s => s.OrderDirection == "Income").Count() == 0, "There should not be orders with OrderDirection Income in GetExpenses");
        }
    }
}
