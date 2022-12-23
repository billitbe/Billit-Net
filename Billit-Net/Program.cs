// See https://aka.ms/new-console-template for more information
using Billit_Net;

const string env = "https://api.sandbox.billit.be";
const string APIKEY_VALID = "d3aa6a49-e1e0-4e76-9132-c5eaf069244e";
const int PartyIDIndex = 0;
var oData = $"?$filter=OrderDirection eq 'Income' and OrderType eq 'Invoice' and LastModified ge DateTime'{DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd")}'&$orderby=OrderID+asc";

var service = new BillitService(APIKEY_VALID,env);
await service.ConnectAsync();
var partyID = service.AccountInformation.Companies[PartyIDIndex].PartyID.ToString();
//var incomes = service.GetIncomeInvoices(partyID, oData);
var expenses = service.GetExpenses(partyID, oData);
Console.WriteLine($"{expenses.Count} expenses found");

var parties = service.GetCustomers(partyID);
Console.WriteLine($"{parties.Count()} parties found");

var active = service.IsCompanyActiveOnPEPPOL("BE0563846944");
Console.WriteLine($"Company is active on PEPPOL: {active.Registered}");


//Console.WriteLine(string.Format("{0} income invoices detected", incomes.Count));
Console.WriteLine("Press any key to continue...");
Console.ReadLine();
        

