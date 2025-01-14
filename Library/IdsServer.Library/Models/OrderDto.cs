using System.Collections.Generic;

namespace IdsServer.Library.Models;

public class OrderDto
{
    public OrderInfoDto OrderInfoDto { get; set; } = new();
    public List<OrderItemDto> OrderItemDtos { get; set; } = new();
}