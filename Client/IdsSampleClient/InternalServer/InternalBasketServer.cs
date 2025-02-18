using System.Net;
using IdsSampleClient.InternalServer.Events;

namespace IdsSampleClient.InternalServer;
internal class InternalBasketServer
{
    private readonly string _url;

    public event EventHandler<DataReceivedEventArgs>? BasketReceived;
    public event EventHandler<Events.ErrorEventArgs>? ErrorOccurred;

    /// <summary>
    /// Initializes a new instance of the <see cref="InternalBasketServer"/> class.
    /// </summary>
    /// <param name="internalUrl">The url where you want to listen.</param>
    public InternalBasketServer(string internalUrl)
    {
        _url = internalUrl;
    }

    internal void StartHttpServer()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(_url);
        listener.Start();
        listener.BeginGetContext(OnProcessBasketReceive, listener);
    }

    private void OnProcessBasketReceive(IAsyncResult result)
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
                    string basketXml = reader.ReadToEnd();

                    BasketReceived?.Invoke(this, new DataReceivedEventArgs(basketXml));
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
                listener.BeginGetContext(OnProcessBasketReceive, listener);
            }
        }
    }
}
