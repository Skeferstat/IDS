using BasketReceive;

namespace IdsServer.Models;

public class OrderDto
{
    public OrderInfoDto OrderInfoDto { get; set; }
    public List<OrderItemDto> OrderItemDtos { get; set; }
}