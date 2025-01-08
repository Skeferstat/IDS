using System.Net.Http.Headers;
using System.Xml;
using IdsLibrary.Http;
using IdsLibrary.Models;
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
            this.HookUriTextBox.Text = _appSettings.HookUri;
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
            string hookUri = HookUriTextBox.Text;
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

            var basket = Deserializer.DeserializeBasketSend(xmlDoc.InnerXml!);

            PackageHeader packageHeader = new PackageHeader
            {
                CustomerNumber = _appSettings.Shop.AuthCustomerNumber,
                UserName = _appSettings.Shop.AuthUsername,
                Password = _appSettings.Shop.AuthPassword,
                ActionCode = ActionCode.SendBasketToShop,
                HookUri = new Uri(hookUri),
                Version = idsVersion,
                Target = "TOP",
                ShopUri = new Uri(shopUrl)
            };

            PostCreator postCreator = new PostCreator(packageHeader);
            (Uri ShopUri, MemoryStream ContentStream, HttpContentHeaders Headers) data = await postCreator.GetAsync(basket!);

            WebViewForm webViewForm = new WebViewForm();
            await webViewForm.SetDataAsync(data.ShopUri, "POST", data.ContentStream, data.Headers);
            webViewForm.Show();
        }

        private async void OnSearchTerm(object sender, EventArgs eventArgs)
        {
            string shopUrl = ShopUrlTextBox.Text;
            string hookUri = HookUriTextBox.Text;
            string? idsVersion = IdsVersionComboBox.SelectedItem!.ToString();
            string searchTerm = SearchTermTextBox.Text;

            PackageHeader packageHeader = new PackageHeader
            {
                CustomerNumber = _appSettings.Shop.AuthCustomerNumber,
                UserName = _appSettings.Shop.AuthUsername,
                Password = _appSettings.Shop.AuthPassword,
                ActionCode = ActionCode.ArticleSearch,
                HookUri = new Uri(hookUri),
                Version = idsVersion,
                Target = "TOP",
                ShopUri = new Uri(shopUrl)
            };

            PostCreator postCreator = new PostCreator(packageHeader);
            (Uri ShopUri, MemoryStream ContentStream, HttpContentHeaders Headers) data = await postCreator.GetAsync(searchTerm);

            WebViewForm webViewForm = new WebViewForm();
            await webViewForm.SetDataAsync(data.ShopUri, "POST", data.ContentStream, data.Headers);
            webViewForm.Show();
        }

        private void button1_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
