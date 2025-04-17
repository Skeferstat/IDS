using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;
using BasketSend;
using DevExpress.Charts.Native;
using DevExpress.CodeParser.VB;
using DevExpress.Map.Kml.Model;
using DevExpress.XtraCharts.Native;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSpreadsheet.TileLayout;
using IdsLibrary.Converter;
using IdsLibrary.Factories;
using IdsLibrary.Models.PackageHeaders;
using IdsSampleClient.Helpers;
using IdsSampleClient.InternalServer.Events;
using IdsSampleClient.Models;
using Microsoft.Extensions.Options;
using Serilog;

namespace IdsSampleClient
{
    public partial class MainForm : Form
    {
        private readonly AppSettings _appSettings;
        private typeWarenkorb? _currentBasketGridData;

        public MainForm(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            InitializeComponent();
            ShopUrlTextBox.Text = _appSettings.Shop.AuthUrl;
            BasketHookUriTextBox.Text = _appSettings.BasketsReceiveHookUri;
            SearchArticleHookUriTextBox.Text = _appSettings.ArticlesReceiveHookUri;

            if (IdsVersionComboBox.Items.Count - 1 >= 0)
                IdsVersionComboBox.SelectedIndex = IdsVersionComboBox.Items.Count - 1;

            InternalServer.InternalBasketServer internalBasketServer = new InternalServer.InternalBasketServer(_appSettings.InternalBasketsReceiveHookUri);
            internalBasketServer.BasketReceived += OnBasketReceived;

            InternalServer.InternalArticleServer internalArticlesServer = new InternalServer.InternalArticleServer(_appSettings.InternalArticlesReceiveHookUri);
            internalArticlesServer.ArticlesReceived += OnArticlesReceived;

            internalBasketServer.StartHttpServer();
            internalArticlesServer.StartHttpServer();
        }

        private void OnBasketReceived(object? sender, DataReceivedEventArgs eventArgs)
        {
            if (ReceivedBasketGridControl.InvokeRequired)
            {
                // Execute the same method on the UI thread
                ReceivedBasketGridControl.Invoke(new MethodInvoker(() => OnBasketReceived(sender, eventArgs)));
            }
            else
            {
                // Logic to handle the basket received event.
                BindToDataGrid(eventArgs.Xml, ReceivedBasketGridControl);
            }
        }

        private void OnArticlesReceived(object? sender, DataReceivedEventArgs eventArgs)
        {
            //if (ReceivedRawArticlesTreeView.InvokeRequired)
            //{
            //    // Execute the same method on the UI thread
            //    ReceivedRawArticlesTreeView.Invoke(new MethodInvoker(() => OnArticlesReceived(sender, eventArgs)));
            //}
            //else
            //{
            //    // Logic to handle the articles received event.
            //    BindArticlesXmlToTreeView(eventArgs.Xml, ReceivedRawArticlesTreeView);
            //}
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

                BindToDataGrid(xmlDoc.InnerXml, CurrentBasketGridControl);
            }
        }

        private void BindToDataGrid(string xmlData, GridControl gridControl)
        {
            _currentBasketGridData = IdsLibrary.Serializing.Deserializer.DeserializeBasketSend(xmlData);
            var master = new SendBasketGridData
            {
                Date = _currentBasketGridData!.WarenkorbInfo.Date,
                Time = _currentBasketGridData.WarenkorbInfo.Time.ToString("HH:mm:ss"),

                Version = Helper.GetXmlEnumValue(_currentBasketGridData.WarenkorbInfo.Version),
                OrderItems = _currentBasketGridData.Order.OrderItem.ToList()
            };

            var masterList = new List<SendBasketGridData> { master };
            gridControl.DataSource = masterList;

            GridView detailView = new GridView(gridControl);
            gridControl.LevelTree.Nodes.Add("OrderItems", detailView);

            detailView.OptionsCustomization.AllowGroup = false;
            detailView.OptionsView.ShowGroupPanel = false;
            detailView.GroupCount = 0;
            detailView.ClearGrouping();
            detailView.Columns.Clear();
            detailView.VertScrollVisibility = ScrollVisibility.Auto;

            detailView.Columns.AddVisible("ArtNo", "ArtNo");
            detailView.Columns.AddVisible("Qty", "Qty");
            detailView.Columns.AddVisible("NetPrice", "NetPrice");
            detailView.Columns["NetPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            detailView.Columns["NetPrice"].DisplayFormat.FormatString = "c2";
        }

        private void OnSaveBasket(object sender, EventArgs eventArgs)
        {
            DialogResult result = SaveBasketFileDialog.ShowDialog();

            SendBasketGridData? gridBasket = ((CurrentBasketGridControl.DataSource as List<SendBasketGridData>)!).FirstOrDefault();

            _currentBasketGridData!.WarenkorbInfo.Date = gridBasket!.Date;
            _currentBasketGridData.WarenkorbInfo.Time = DateTime.ParseExact(gridBasket.Time, "HH:mm:ss", CultureInfo.InvariantCulture);
            _currentBasketGridData.WarenkorbInfo.Version = Helper.GetEnumFromXmlValue<typeWarenkorbInfoVersion>(gridBasket.Version);

            _currentBasketGridData.Order.OrderItem = gridBasket.OrderItems.ToArray();
            var xml = IdsConverter.ConvertToXml(_currentBasketGridData!);

            xml = xml.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");

            if (result == DialogResult.OK && SaveBasketFileDialog.FileName != "")
            {
                var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
                using var writer = new StreamWriter(SaveBasketFileDialog.FileName, false, encoding);
                writer.Write(xml);
            }
        }

        private async void OnSendBasketToShop(object sender, EventArgs eventArgs)
        {
            SendBasketGridData? gridBasket = ((CurrentBasketGridControl.DataSource as List<SendBasketGridData>)!).FirstOrDefault();

            _currentBasketGridData!.WarenkorbInfo.Date = gridBasket!.Date;
            _currentBasketGridData.WarenkorbInfo.Time = DateTime.ParseExact(gridBasket.Time, "HH:mm:ss", CultureInfo.InvariantCulture);
            _currentBasketGridData.WarenkorbInfo.Version = Helper.GetEnumFromXmlValue<typeWarenkorbInfoVersion>(gridBasket.Version);

            _currentBasketGridData.Order.OrderItem = gridBasket.OrderItems.ToArray();

            string shopUrl = ShopUrlTextBox.Text;
            string hookUri = BasketHookUriTextBox.Text;
            string xml = IdsConverter.ConvertToXml(_currentBasketGridData!);

            BasketSendPackageHeader? packageHeader = new BasketSendPackageHeader
            {
                CustomerNumber = _appSettings.Shop.AuthCustomerNumber,
                UserName = _appSettings.Shop.AuthUsername,
                Password = _appSettings.Shop.AuthPassword,
                Version = gridBasket.Version,
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
            string hookUri = SearchArticleHookUriTextBox.Text;
            string? idsVersion = IdsVersionComboBox.SelectedItem!.ToString();
            string searchTerm = SearchTermTextBox.Text;

            SearchTermPackageHeader packageHeader = new SearchTermPackageHeader
            {
                CustomerNumber = _appSettings.Shop.AuthCustomerNumber,
                UserName = _appSettings.Shop.AuthUsername,
                Password = _appSettings.Shop.AuthPassword,
                Version = idsVersion,
                ShopUri = new Uri(shopUrl),
                HookUri = new Uri(hookUri)
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
            //if (ReceivedRawBasketTreeView.Nodes.Count < 1)
            //{
            //    return;
            //}

            //string xml = TreeNodeHelper.ConvertToXml(ReceivedRawBasketTreeView);
            //BindBasketXmlToTreeView(xml, CurrentRawBasketTreeView);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
