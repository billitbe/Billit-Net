using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billit_Net
{
    class Program
    {
        const string APIKEY_VALID = "09b74d3c-b0be-47bd-8a47-68bd8eb186ff"; // this is a sandbox token, only available on https://my.sandbox.billit.be & https://api.sandbox.billit.be
        const int PartyIDIndex = 0;

        static void Main(string[] args)
        {
            //see unit tests for examples

            // Billit highly recommends you to optimize the oData query and caching mechanisms to prevent your application of beeing throttled or blocked
            // More information about oData filters: https://www.odata.org/documentation/odata-version-2-0/uri-conventions/

            // This is the oData representation of all income invoices that have been modified since yesterday ordered by OrderID in ascending order.
            var oData = "?$filter=OrderDirection eq 'Income' and OrderType eq 'Invoice' and LastModified ge DateTime'" + GetDateFilter() + "'&$orderby=OrderID+asc";

            var service = new BillitService(APIKEY_VALID);
            var partyID = service.AccountInformation.Companies[PartyIDIndex].PartyID;
            var incomes = service.GetIncomeInvoices(partyID, oData);
            Console.WriteLine(string.Format("{0} income invoices detected",incomes.Count));
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static string GetDateFilter()
        {
            return DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
        }

    }
}
