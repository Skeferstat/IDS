namespace IdsSampleClient.InternalServer.Events;

internal class BasketReceivedEventArgs : EventArgs
{
    public string Xml { get; }

    public BasketReceivedEventArgs(string xml)
    {
        Xml = xml;
    }
}