using System.Net.Http.Headers;
using System.Xml;
using IdsLibrary.Factories;
using IdsLibrary.Models.PackageHeaders;
using IdsLibrary.Serializing;
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
            this.BasketHookUriTextBox.Text = _appSettings.BasketHookUri;
            if (IdsVersionComboBox.Items.Count - 1 >= 0)
                this.IdsVersionComboBox.SelectedIndex = IdsVersionComboBox.Items.Count - 1;
        }


        private void OnOpenBasketFile(object sender, EventArgs eventArgs)
        {
            var result = this.OpenBasketFileDialog.ShowDialog();
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

            var factory = new BasketSendPackageFactory();
            var data = await factory.CreatePackage(packageHeader, xmlDoc.InnerXml);

            var memoryStream = new MemoryStream();
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

            var factory = new SearchTermPackageFactory();
            var data = await factory.CreatePackage(packageHeader, searchTerm);

            var memoryStream = new MemoryStream();
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

            var packageHeader = new DeepLinkPackageHeader
            {
                CustomerNumber = _appSettings.Shop.AuthCustomerNumber,
                UserName = _appSettings.Shop.AuthUsername,
                Password = _appSettings.Shop.AuthPassword,
                Version = idsVersion,
                ShopUri = new Uri(shopUrl)
            };

            var factory = new DeepLinkPackageFactory();
            var data = await factory.CreatePackage(packageHeader, articleNumber);

            var memoryStream = new MemoryStream();
            data.Content.CopyToAsync(memoryStream).Wait();
            memoryStream.Position = 0;

            WebViewForm webViewForm = new WebViewForm();
            await webViewForm.SetDataAsync(data.ShopUri, data.Method, memoryStream, data.Headers);
            webViewForm.Show();
        }
    }
}
