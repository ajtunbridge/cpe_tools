using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CPE.Sales.Models;
using CPE.Sales.Properties;
using Microsoft.Office.Interop.Outlook;

namespace CPE.Sales.Services
{
    public sealed class MSOutlookService
    {
        public static List<MSOutlookMailItem> GetSalesOrderMail()
        {
            var folder = Settings.Default.NewOrdersFolderName;

            return GetMailItems(folder, ".pdf");
        }

        public static MSOutlookMailItem GetMostRecentOpenOrderReport()
        {
            var folder = Settings.Default.NewOpenOrderReportsFolderName;

            return GetMailItems(folder, ".xls").OrderByDescending(m => m.ReceivedAt).FirstOrDefault();
        }

        private static List<MSOutlookMailItem> GetMailItems(string folderName, string validFileExtension)
        {
            var results = new List<MSOutlookMailItem>();

            var session = Login();

            if (session == null)
            {
                return results;
            }

            MAPIFolder orderFolder = null;

            foreach (MAPIFolder folder in session.Folders)
            {
                orderFolder = FindFolder(folder, folderName);

                if (orderFolder != null)
                {
                    break;
                }
            }

            foreach (var obj in orderFolder.Items)
            {
                if (obj is MailItem)
                {
                    var mail = (MailItem) obj;

                    var mailItem = new MSOutlookMailItem
                    {
                        Body = mail.Body,
                        MailId = mail.EntryID,
                        ReceivedAt = mail.ReceivedTime,
                        Sender = mail.SenderName,
                        Subject = mail.Subject
                    };

                    if (mail.Attachments.Count > 0)
                    {
                        for (var i = 1; i <= mail.Attachments.Count; i++)
                        {
                            var ext = Path.GetExtension(mail.Attachments[i].FileName).ToLower();
                            if (ext != validFileExtension)
                            {
                                continue;
                            }

                            var fileName = Path.Combine(Path.GetTempPath(), mail.Attachments[i].FileName);

                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                            }

                            mail.Attachments[i].SaveAsFile(fileName);

                            mailItem.Attachments.Add(fileName);
                        }
                    }

                    results.Add(mailItem);
                }
            }

            return results;
        }

        private static NameSpace Login()
        {
            var application = new Application();

            try
            {
                application.Session.Logon();
            }
            catch
            {
                // exception thrown if Outlook isn't open and user cancels out of profile selection dialog
                return null;
            }

            return application.Session;
        }

        private static MAPIFolder FindFolder(MAPIFolder rootFolder, string folderName)
        {
            if (rootFolder.Folders.Count == 0)
            {
                if (rootFolder.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase))
                {
                    return rootFolder;
                }
            }
            else
            {
                foreach (MAPIFolder subFolder in rootFolder.Folders)
                {
                    var result = FindFolder(subFolder, folderName);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
    }
}