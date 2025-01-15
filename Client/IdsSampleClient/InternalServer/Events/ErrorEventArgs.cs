namespace IdsSampleClient.InternalServer.Events;

internal class ErrorEventArgs : EventArgs
{
    public Exception Exception { get; }

    public ErrorEventArgs(Exception exception)
    {
        Exception = exception;
    }
}