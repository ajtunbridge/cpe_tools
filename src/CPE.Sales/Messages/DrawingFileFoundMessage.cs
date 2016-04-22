using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Sales.Models;

namespace CPE.Sales.Messages
{
    public sealed class DrawingFileFoundMessage
    {
        public DrawingFile FoundFile { get; private set; }

        public DrawingFileFoundMessage(DrawingFile foundFile)
        {
            FoundFile = foundFile;
        }
    }
}
