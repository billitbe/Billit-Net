BILLIT-Net
========
A skinny wrapper of the BILLIT API. A skinny wrapper of the BILLIT API. Supports Company Information, Invoices, Customers & Suppliers

* [Installation](#installation)
* [What is supported?](#what-is-supported)
* [Things to note](#things-to-note)
* [Samples](#samples)
* [License](#license)

## Installation

Download the source code from github and compile yourself: **https://github.com/billitbe/billit-Net**


## What is supported?
### Core
* Account - List Companies and Login
* Customers - Create, Find and Update
* Suppliers - Add, Get and List
* Bank Transactions - Create, Find and Update
* CompanyInfo - Create, Find and Update
* Credit Notes - Create, Find and Update
* Invoices - Create, Find and Update
* PEPPOL - Find VAT, Send E-invoice, Receive E-invoice

### Documents API
* Documents - Add PDF, Word, Excel files

## Things to note
* The library tries to do as little as possible and provides a basis to be extended. Private application will work out of the box, as they do not have to deal with tokens and OAuth.
* The HTTP verbs are not used in the public part of the API. Create, Update and Find are used instead. This separates the implementation from the intent.


## Samples
There are samples for each of the API endpoints. These have been done as console application and also a collection of Unit tests. The test projects contain lots of useful examples of how to use this library to interact with the Billit API.

### How to verify if a customer is active on PEPPOL?
```csharp
String vat = "BE0563846944";
BillitService service = new BillitService(APIKEY);
var result = service.IsCompanyActiveOnPEPPOL(vat);
```

### How to send an e-invoice via PEPPOL?
```csharp
var json = File.ReadAllText(Environment.CurrentDirectory+ @"\Files\validPEPPOLInvoice.json", Encoding.UTF8);
var service = new BillitService(APIKEY_VALID);
string responseID = service.SendInvoiceViaPEPPOL(json);
//responseID can be used later to retrieve results and status, however that api is at this moment not available yet
```

### How to retrieve all invoices since a specific date?
```csharp
// Billit highly recommends you to optimize the oData query and caching mechanisms to prevent your application of beeing throttled or blocked
// More information about oData filters: https://www.odata.org/documentation/odata-version-2-0/uri-conventions/
            
// This is the oData representation of all income invoices that have been modified since yesterday ordered by OrderID in ascending order.
var oData = "?$filter=OrderDirection eq 'Income' and OrderType eq 'Invoice' and LastModified ge DateTime'" + GetDateFilter() + "'&$orderby=OrderID+asc";

var service = new BillitService(APIKEY_VALID);
var partyID = service.AccountInformation.Companies[PartyIDIndex].PartyID;
var incomes = service.GetIncomeInvoices(partyID, oData);
```

### How to send a PDF or image to fast input to be processed to UBL?
```csharp
var service = new BillitService(APIKEY_VALID);
FileToProcess file = new FileToProcess() {
	File = new File {	
		FileName = "test.pdf",
		FileContent = "JVBERi0xLjcNJeLjz9MNCjE0IDAgb..."
                }
	};
service.PushDocumentToFastInput(file);
```

## License

This software is published under the [MIT License](http://en.wikipedia.org/wiki/MIT_License).

	Copyright (c) 2019  BILLIT

	Permission is hereby granted, free of charge, to any person
	obtaining a copy of this software and associated documentation
	files (the "Software"), to deal in the Software without
	restriction, including without limitation the rights to use,
	copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the
	Software is furnished to do so, subject to the following
	conditions:

	The above copyright notice and this permission notice shall be
	included in all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
	EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
	OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
	NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
	HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
	WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
	OTHER DEALINGS IN THE SOFTWARE.
