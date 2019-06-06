using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billit_Net.DTO
{
    public class FileInformation
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }
    }

    public class FileToProcess
    {
        public FileInformation File { get; set; }
    }
}
