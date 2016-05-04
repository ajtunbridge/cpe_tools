using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Outlook;
using MSOutlookProvider.Model;

namespace MSOutlookProvider
{
    public static class MailProvider
    {
        private static NameSpace _session;
        
        public static List<MailFolder> GetFolderHierarchy()
        {
            Login();

            if (_session == null)
            {
                return new List<MailFolder>();
            }

            var hierarchy = new List<MailFolder>();

            foreach (MAPIFolder rootFolder in _session.Folders)
            {
                var mailFolder = new MailFolder
                {
                    Id = rootFolder.StoreID,
                    Name = rootFolder.Name
                };

                AddChildFolders(rootFolder, mailFolder);

                hierarchy.Add(mailFolder);
            }

            return hierarchy;
        }

        public static List<Mail> GetMail(string folderId)
        {
            Login();

            if (_session == null)
            {
                return new List<Mail>();
            }

            var results = new List<Mail>();
            
            MAPIFolder mailFolder = _session.GetFolderFromID(folderId);

            var tempDir = Path.GetTempPath();

            foreach (var item in mailFolder.Items)
            {
                if (!(item is MailItem))
                {
                    continue;
                }

                var mailItem = item as MailItem;
                
                var mail = new Mail
                {
                    Id = mailItem.EntryID,
                    Body = mailItem.Body,
                    ReceivedAt = mailItem.ReceivedTime,
                    Sender = mailItem.SenderName,
                    Subject = mailItem.Subject
                };

                foreach (Attachment attachment in mailItem.Attachments)
                {
                    var fileName = Path.Combine(tempDir, attachment.FileName);

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    attachment.SaveAsFile(fileName);

                    mail.ExtractedAttachments.Add(fileName);
                }

                results.Add(mail);
            }

            return results;
        }
        
        private static void AddChildFolders(MAPIFolder folder, MailFolder rootFolder)
        {
            foreach (MAPIFolder childMAPIFolder in folder.Folders)
            {
                var childMailFolder = new MailFolder
                {
                    Id = childMAPIFolder.EntryID,
                    Name = childMAPIFolder.Name
                };

                AddChildFolders(childMAPIFolder, childMailFolder);

                rootFolder.Children.Add(childMailFolder);
            }
        }
        
        private static void Login()
        {
            var application = new Application();

            try
            {
                application.Session.Logon();
            }
            catch
            {
                // exception thrown if Outlook isn't open and user cancels out of profile selection dialog
            }

            _session = application.Session;
        }
    }
}