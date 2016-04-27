using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSOutlookProvider;
using MSOutlookProvider.Model;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var folders = MailProvider.GetFolderHierarchy();

            foreach (var folder in folders)
            {
                RecursivelyPrintHierarchy(folder);
            }

            Console.Read();
        }

        static void RecursivelyPrintHierarchy(MailFolder folder)
        {
            Console.WriteLine(folder.Name);
            
            foreach (var childFolder in folder.Children)
            {
                RecursivelyPrintHierarchy(childFolder);
            }
        }
    }
}
