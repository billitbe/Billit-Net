using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billit_Net.DTO
{
    public record Process
    {
        public string ProcessIdentifier { get; set; }
        public string ProcessIdentifierScheme { get; set; }
    }

    public record ServiceDetail
    {
        public string DocumentIdentifier { get; set; }
        public string DocumentIdentifierScheme { get; set; }
        public List<Process> Processes { get; set; }
    }

    public record PeppolParticipantInformation
    {
        public bool Registered { get; set; }
        public List<string> DocumentTypes { get; set; }
        public List<ServiceDetail> ServiceDetails { get; set; }
    }
}
