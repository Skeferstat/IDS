using System.Net.Http.Headers;
using System.Xml;
using IdsLibrary.Factories;
using IdsLibrary.Models.PackageHeaders;
using IdsLibrary.Serializing;
using IdsSampleClient.Helpers;
using IdsSampleClient.InternalServer;
using IdsSampleClient.InternalServer.Events;
using IdsServer.Library.Models;
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
            this.ShopUrlTextBox.Text = _appSettings.Shop.AuthUrl;
            this.BasketHookUriTextBox.Text = _appSettings.BasketReceiveHookUri;
            if (IdsVersionComboBox.Items.Count - 1 >= 0)
                this.IdsVersionComboBox.SelectedIndex = IdsVersionComboBox.Items.Count - 1;

            InternalServer.InternalServer internalServer = new InternalServer.InternalServer(_appSettings.InternalBasketReceiveHookUri);
            internalServer.BasketReceived += OnBasketReceived;
            internalServer.StartHttpServer();
        }

        private void OnBasketReceived(object? sender, BasketReceivedEventArgs eventArgs)
        {
            if (RawBasketTreeView.InvokeRequired)
            {
                // Execute the same method on the UI thread
                RawBasketTreeView.Invoke(new MethodInvoker(() => OnBasketReceived(sender, eventArgs)));
            }
            else
            {
                // Logic to handle the basket received event.
                BasketDto basket = eventArgs.Basket;
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(basket.RawXml);

                RawBasketTreeView.Nodes.Clear();
                RawBasketTreeView.Nodes.Add(new TreeNode(dom.DocumentElement!.Name));

                TreeNode treeNode = RawBasketTreeView.Nodes[0];
                TreeNodeHelper.AddNode(dom.DocumentElement, treeNode);
            }
        }


        private void OnOpenBasketFile(object sender, EventArgs eventArgs)
        {
            DialogResult result = this.OpenBasketFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.BasketXmlFileTextBox.Text = this.OpenBasketFileDialog.FileName;
            }
        }

        private async void OnSendBasketToShop(object sender, EventArgs eventArgs)
        {
            string shopUrl = ShopUrlTextBox.Text;
            string hookUri = BasketHookUriTextBox.Text;
            string? idsVersion = IdsVersionComboBox.SelectedItem!.ToString();
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
            IIdsPackage data = await factory.CreatePackage(packageHeader, xmlDoc.InnerXml);

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
            string hookUri = BasketHookUriTextBox.Text;
            string? idsVersion = IdsVersionComboBox.SelectedItem!.ToString();
            string articleNumber = this.DeepLinkSearchTextBox.Text;

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
    }
}
