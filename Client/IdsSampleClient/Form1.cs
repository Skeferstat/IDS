using System.Text;
using BasketSend;
using Microsoft.Extensions.Options;
using Microsoft.Web.WebView2.Core;
using Serilog;

namespace IdsSampleClient
{
    public partial class Form1 : Form
    {
        private readonly AppSettings _appSettings;
        public Form1(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            InitializeComponent();

            this.ShopUrlTextBox.Text = _appSettings.Shop.AuthUrl;
        }

        private void OnOpenBasketFile(object sender, EventArgs e)
        {
            this.OpenBasketFileDialog.ShowDialog();
        }

        private async void OnSendBasketToShop(object sender, EventArgs e)
        {
            await this.ShopWebView.EnsureCoreWebView2Async();
            string data = "{}";
            string url = ShopUrlTextBox.Text;

            typeWarenkorb basket = new typeWarenkorb();
            basket.WarenkorbInfo = new typeWarenkorbInfo();
            basket.WarenkorbInfo.Date =   DateTime.UtcNow;
            basket.WarenkorbInfo.Time = DateTime.UtcNow;
            basket.WarenkorbInfo.Version = typeWarenkorbInfoVersion.Item25;

            basket.Order = new typeOrder();
            basket.Order.OrderInfo = new typeOrderInfo();
            basket.Order.OrderInfo.OrderNumber = "123456";


            string additionalHeaders = "Content-Type: application/xml";
            //CoreWebView2WebResourceRequest request = ShopWebView.CoreWebView2.Environment.CreateWebResourceRequest(url, "POST", new MemoryStream(Encoding.UTF8.GetBytes(data)), additionalHeaders);
            //ShopWebView.CoreWebView2.NavigateWithWebResourceRequest(request);
        }
    }
}
