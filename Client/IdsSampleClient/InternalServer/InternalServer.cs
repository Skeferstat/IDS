using System.Net;
using AutoMapper;
using IdsSampleClient.InternalServer.Events;
using IdsSampleClient.Mapping;

namespace IdsSampleClient.InternalServer;
internal class InternalServer
{
    private readonly string _url;
    private readonly Mapper _mapper;

    public event EventHandler<BasketReceivedEventArgs>? BasketReceived;
    public event EventHandler<Events.ErrorEventArgs>? ErrorOccurred;

    public InternalServer(string internalUrl)
    {
        _url = internalUrl;
        _mapper = MapperConfig.Get();
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
            HttpListenerContext context = null;
            try
            {
                context = listener.EndGetContext(result);
                HttpListenerRequest request = context.Request;

                if (request.HttpMethod == "POST")
                {
                    using StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    string basketXml = reader.ReadToEnd();

                    BasketReceived?.Invoke(this, new BasketReceivedEventArgs(basketXml));
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
