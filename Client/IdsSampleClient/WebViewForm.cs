using System.Net.Http.Headers;
using Microsoft.Web.WebView2.Core;

namespace IdsSampleClient;
public partial class WebViewForm : Form
{
    public WebViewForm()
    {
        InitializeComponent();
        InitializeWebView();
    }

    private void InitializeWebView()
    {
        WebView.CoreWebView2InitializationCompleted += OnInitializationCompleted;
        WebView.EnsureCoreWebView2Async(null);
    }


    public void NavigateWithWebResourceRequest(CoreWebView2WebResourceRequest webResourceRequest)
    {
        WebView.CoreWebView2.NavigateWithWebResourceRequest(webResourceRequest);
    }

    public async Task SetDataAsync(Uri uri, string method, MemoryStream contentStream, HttpContentHeaders headers)
    {
        if (WebView.CoreWebView2 == null)
        {
            await WebView.EnsureCoreWebView2Async(null);
        }

        CoreWebView2WebResourceRequest? request =
            WebView.CoreWebView2!.Environment.CreateWebResourceRequest(uri.ToString(), method, contentStream, headers.ToString());
        NavigateWithWebResourceRequest(request);
    }


    private void OnInitializationCompleted(object? sender, EventArgs e)
    {
        WebView.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
        WebView.CoreWebView2.WebResourceResponseReceived += OnResponseReceived;
    }


    private void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs eventArgs)
    {
        if (eventArgs.IsSuccess)
        {
            
        }
        else
        {
           
        }
    }

    private void OnResponseReceived(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs eventArgs)
    {
        CoreWebView2WebResourceResponseView? response = eventArgs.Response;
        if (response.StatusCode != 200)
        {
            
        }
        else
        {

        }
    }
}
