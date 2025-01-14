using IdsLibrary.Serializing;
using System.Net;
using AutoMapper;
using IdsServer.Library.Models;
using IdsSampleClient.Mapping;

namespace IdsSampleClient.InternalServer;
internal class InternalServer
{
    private readonly string _url;
    private readonly Mapper _mapper;

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
            HttpListenerContext context = listener.EndGetContext(result);
            HttpListenerRequest request = context.Request;

            if (request.HttpMethod == "POST")
            {
                using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                string basketXml = reader.ReadToEnd();
                var basket = Deserializer.DeserializeBasketReceive(basketXml);
                BasketDto basketDto = _mapper.Map<BasketDto>(basket);
                basketDto.RawXml = basketXml;
            }
            context.Response.StatusCode = 200;
            context.Response.Close();
            listener.BeginGetContext(OnProcessBasketReceive, listener);
        }
    }
}
