using IdsServer.Library.Models;

namespace IdsSampleClient.InternalServer.Events;

internal class BasketReceivedEventArgs : EventArgs
{
    public BasketDto Basket { get; }

    public BasketReceivedEventArgs(BasketDto basket)
    {
        Basket = basket;
    }
}