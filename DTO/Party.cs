using System;
using System.Collections.Generic;

namespace Billit_Net.DTO
{
    public class Party
    {
        public int PartyID { get; set; }
        public string Nr { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string VATNumber { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime Created { get; set; }
        public string PartyType { get; set; }
        public bool VATLiable { get; set; }
        public string Language { get; set; }
        public List<object> BankAccounts { get; set; }
        public string Box { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, PartyType);
        }
    }

    public class PartyCollection
    {
        public List<Party> Items { get; set; }
        public string NextPageLink { get; set; }
    }

    
}