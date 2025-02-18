using System.Net;
using IdsSampleClient.InternalServer.Events;

namespace IdsSampleClient.InternalServer;
internal class InternalArticleServer
{
    private readonly string _url;

    public event EventHandler<DataReceivedEventArgs>? ArticlesReceived;
    public event EventHandler<Events.ErrorEventArgs>? ErrorOccurred;

    /// <summary>
    /// Initializes a new instance of the <see cref="InternalArticleServer"/> class.
    /// </summary>
    /// <param name="internalUrl">The url where you want to listen.</param>
    public InternalArticleServer(string internalUrl)
    {
        _url = internalUrl;
    }

    internal void StartHttpServer()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(_url);
        listener.Start();
        listener.BeginGetContext(OnProcessArticleReceive, listener);
    }

    private void OnProcessArticleReceive(IAsyncResult result)
    {
        if (result.AsyncState is HttpListener listener)
        {
            HttpListenerContext? context = null;
            try
            {
                context = listener.EndGetContext(result);
                HttpListenerRequest request = context.Request;

                if (request.HttpMethod == "POST")
                {
                    using StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    string articlesXml = reader.ReadToEnd();

                    ArticlesReceived?.Invoke(this, new DataReceivedEventArgs(articlesXml));
                }

                context.Response.StatusCode = 200;
            }
            catch (Exception exception)
            {
                ErrorOccurred?.Invoke(this, new Events.ErrorEventArgs(exception));
                if (context != null)
                {
                    context.Response.StatusCode = 500;
                }
            }
            finally
            {
                context?.Response.Close();
                listener.BeginGetContext(OnProcessArticleReceive, listener);
            }
        }
    }
}
