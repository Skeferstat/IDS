namespace IdsSampleClient.InternalServer.Events;

internal class DataReceivedEventArgs : EventArgs
{
    public string Xml { get; }

    public DataReceivedEventArgs(string xml)
    {
        Xml = xml;
    }
}