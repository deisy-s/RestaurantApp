using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RestaurantApp.Controller
{
    public class GlobalController
    {
        public string sError { get; set; }
        ConnectSupabase connectSupabase = new ConnectSupabase();

        public async Task<List<MenuData>> getMenuItems()
        {
            return await connectSupabase.GetMenuItems();
        }

        public async Task<List<MenuData>> getMenuItems(MenuData menuItem, string ogName)
        {
            return await connectSupabase.GetMenuItems(menuItem, ogName);
        }

        public async Task<bool> SaveOrder(OrderData order, List<OrderDetailsData> orderData)
        {
            return await connectSupabase.SaveOrder(order, orderData);
        }

        public async Task<bool> updateStatus(OrderData order)
        {
            return await connectSupabase.UpdateStatus(order);
        }
    }
}
