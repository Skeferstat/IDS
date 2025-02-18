using System.Xml;
using IdsLibrary.Factories;
using IdsLibrary.Models.PackageHeaders;
using IdsSampleClient.Helpers;
using IdsSampleClient.InternalServer.Events;
using Microsoft.Extensions.Options;
using Serilog;

namespace IdsSampleClient
{
    public partial class MainForm : Form
    {
        private readonly AppSettings _appSettings;

        public MainForm(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            InitializeComponent();
            ShopUrlTextBox.Text = _appSettings.Shop.AuthUrl;
            BasketHookUriTextBox.Text = _appSettings.BasketsReceiveHookUri;
            SearchArticleHookUriTextBox.Text = _appSettings.ArticlesReceiveHookUri;

            if (IdsVersionComboBox.Items.Count - 1 >= 0)
                IdsVersionComboBox.SelectedIndex = IdsVersionComboBox.Items.Count - 1;

            TreeNodeHelper.AddContextMenu(CurrentRawBasketTreeView);
           
            InternalServer.InternalBasketServer internalBasketServer = new InternalServer.InternalBasketServer(_appSettings.InternalBasketsReceiveHookUri);
            internalBasketServer.BasketReceived += OnBasketReceived;

            InternalServer.InternalArticleServer internalArticlesServer = new InternalServer.InternalArticleServer(_appSettings.InternalArticlesReceiveHookUri);
            internalArticlesServer.ArticlesReceived += OnArticlesReceived;

            internalBasketServer.StartHttpServer();
            internalArticlesServer.StartHttpServer();
        }

        private void OnBasketReceived(object? sender, DataReceivedEventArgs eventArgs)
        {
            if (ReceivedRawBasketTreeView.InvokeRequired)
            {
                // Execute the same method on the UI thread
                ReceivedRawBasketTreeView.Invoke(new MethodInvoker(() => OnBasketReceived(sender, eventArgs)));
            }
            else
            {
                // Logic to handle the basket received event.
                BindBasketXmlToTreeView(eventArgs.Xml, ReceivedRawBasketTreeView);
            }
        }

        private void OnArticlesReceived(object? sender, DataReceivedEventArgs eventArgs)
        {
            if (ReceivedRawArticlesTreeView.InvokeRequired)
            {
                // Execute the same method on the UI thread
                ReceivedRawArticlesTreeView.Invoke(new MethodInvoker(() => OnArticlesReceived(sender, eventArgs)));
            }
            else
            {
                // Logic to handle the articles received event.
                BindArticlesXmlToTreeView(eventArgs.Xml, ReceivedRawArticlesTreeView);
            }
        }


        private void OnOpenBasketFile(object sender, EventArgs eventArgs)
        {
            DialogResult result = OpenBasketFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                BasketXmlFileTextBox.Text = OpenBasketFileDialog.FileName;
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(OpenBasketFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error loading basket file");
                    throw;
                }

                BindBasketXmlToTreeView(xmlDoc.InnerXml, CurrentRawBasketTreeView);
            }
        }

        private void OnSaveBasket(object sender, EventArgs eventArgs)
        {
            DialogResult result = SaveBasketFileDialog.ShowDialog();
            if (result == DialogResult.OK && SaveBasketFileDialog.FileName != "")
            {
                TextWriter writer = new StreamWriter(SaveBasketFileDialog.FileName);
                string xml = TreeNodeHelper.ConvertToXml(CurrentRawBasketTreeView);
                writer.Write(xml);
                writer.Close();
            }
        }

        private async void OnSendBasketToShop(object sender, EventArgs eventArgs)
        {
            string shopUrl = ShopUrlTextBox.Text;
            string hookUri = BasketHookUriTextBox.Text;
            string? idsVersion = IdsVersionComboBox.SelectedItem!.ToString();

            string xml = TreeNodeHelper.ConvertToXml(CurrentRawBasketTreeView);

            BasketSendPackageHeader? packageHeader = new BasketSendPackageHeader
            {
                CustomerNumber = _appSettings.Shop.AuthCustomerNumber,
                UserName = _appSettings.Shop.AuthUsername,
                Password = _appSettings.Shop.AuthPassword,
                Version = idsVersion,
                ShopUri = new Uri(shopUrl),
                HookUri = new Uri(hookUri)
            };

            BasketSendPackageFactory factory = new BasketSendPackageFactory();
            IIdsPackage data = await factory.CreatePackage(packageHeader, xml);

            MemoryStream memoryStream = new MemoryStream();
            data.Content.CopyToAsync(memoryStream).Wait();
            memoryStream.Position = 0;

            WebViewForm webViewForm = new WebViewForm();
            await webViewForm.SetDataAsync(data.ShopUri, data.Method, memoryStream, data.Headers);
            webViewForm.Show();
        }

        private async void OnSearchTerm(object sender, EventArgs eventArgs)
        {
            string shopUrl = ShopUrlTextBox.Text;
            string hookUri = BasketHookUriTextBox.Text;
            string? idsVersion = IdsVersionComboBox.SelectedItem!.ToString();
            string searchTerm = SearchTermTextBox.Text;

            SearchTermPackageHeader? packageHeader = new SearchTermPackageHeader
            {
                CustomerNumber = _appSettings.Shop.AuthCustomerNumber,
                UserName = _appSettings.Shop.AuthUsername,
                Password = _appSettings.Shop.AuthPassword,
                Version = idsVersion,
                ShopUri = new Uri(shopUrl)
            };

            SearchTermPackageFactory factory = new SearchTermPackageFactory();
            IIdsPackage data = await factory.CreatePackage(packageHeader, searchTerm);

            MemoryStream memoryStream = new MemoryStream();
            data.Content.CopyToAsync(memoryStream).Wait();
            memoryStream.Position = 0;

            WebViewForm webViewForm = new WebViewForm();
            await webViewForm.SetDataAsync(data.ShopUri, data.Method, memoryStream, data.Headers);
            webViewForm.Show();
        }


        private async void OnDeepLinkSearchTerm(object sender, EventArgs eventArgs)
        {
            string shopUrl = ShopUrlTextBox.Text;
            string? idsVersion = IdsVersionComboBox.SelectedItem!.ToString();
            string articleNumber = DeepLinkSearchTextBox.Text;

            DeepLinkPackageHeader packageHeader = new DeepLinkPackageHeader
            {
                CustomerNumber = _appSettings.Shop.AuthCustomerNumber,
                UserName = _appSettings.Shop.AuthUsername,
                Password = _appSettings.Shop.AuthPassword,
                Version = idsVersion,
                ShopUri = new Uri(shopUrl)
            };

            DeepLinkPackageFactory factory = new DeepLinkPackageFactory();
            IIdsPackage data = await factory.CreatePackage(packageHeader, articleNumber);

            MemoryStream memoryStream = new MemoryStream();
            data.Content.CopyToAsync(memoryStream).Wait();
            memoryStream.Position = 0;

            WebViewForm webViewForm = new WebViewForm();
            await webViewForm.SetDataAsync(data.ShopUri, data.Method, memoryStream, data.Headers);
            webViewForm.Show();
        }

        private void OnCopyReceivedBasketToCurrentBasket(object sender, EventArgs e)
        {
            if (ReceivedRawBasketTreeView.Nodes.Count < 1)
            {
                return;
            }

            string xml = TreeNodeHelper.ConvertToXml(ReceivedRawBasketTreeView);
            BindBasketXmlToTreeView(xml, CurrentRawBasketTreeView);
        }


        private void BindBasketXmlToTreeView(string xml, TreeView treeView)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            XmlDocument xmlDoc = new XmlDocument
            {
                InnerXml = xml
            };

            treeView.Nodes.Clear();
            treeView.Nodes.Add(new TreeNode(xmlDoc.DocumentElement!.Name));
            TreeNode treeNode = treeView.Nodes[0];
            TreeNodeHelper.AddNode(xmlDoc.DocumentElement, treeNode);
            treeView.ExpandAll();
            treeView.TopNode = treeView.Nodes[0];
        }

        private void BindArticlesXmlToTreeView(string xml, TreeView treeView)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentNullException(nameof(xml));
            }

            XmlDocument xmlDoc = new XmlDocument
            {
                InnerXml = xml
            };

            treeView.Nodes.Clear();
            treeView.Nodes.Add(new TreeNode(xmlDoc.DocumentElement!.Name));
            TreeNode treeNode = treeView.Nodes[0];
            TreeNodeHelper.AddNode(xmlDoc.DocumentElement, treeNode);
            treeView.ExpandAll();
            treeView.TopNode = treeView.Nodes[0];
        }


    }
}
