using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp
{
    public class FullOrder
    {
        public OrderData order { get; set; }
        public List<OrderDetailsData> details { get; set; }
        public bool IsNotCancelled => order.status != "Cancelado" && order.status != "Finalizado";
        public int GridHeight => (details.Count * 80)+50;
    }
}
