using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CPE.Sales.Properties;
using MSOutlookProvider;
using MSOutlookProvider.Model;

namespace CPE.Sales
{
    /// <summary>
    ///     Interaction logic for FirstRunWindow.xaml
    /// </summary>
    public partial class FirstRunWindow : Window
    {
        public FirstRunWindow()
        {
            InitializeComponent();
        }

        private void FirstRunWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var folders = MailProvider.GetFolderHierarchy();

            var flat = new List<MailFolder>();

            var queue = new Queue<MailFolder>();
            queue.Enqueue(folders.First());

            while (queue.Count > 0)
            {
                var f = queue.Dequeue();

                flat.Add(f);

                f.Children.ForEach(x => queue.Enqueue(x));
            }

            DataContext = flat.OrderBy(f => f.Name);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var folder = FoldersList.SelectedItem as MailFolder;

            Settings.Default.SalesOrderFolderId = folder.Id;

            Settings.Default.Save();

            Close();
        }
    }
}