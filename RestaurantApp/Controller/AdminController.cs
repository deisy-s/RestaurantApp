using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp
{
    public class AdminController
    {
        ConnectAdmin connectAdmin = new ConnectAdmin();
        public async Task<bool> saveMenu(MenuData menu)
        {
            return await connectAdmin.SaveMenu(menu);
        }

        public async Task<string> saveImage(FileResult file)
        {
            return await connectAdmin.UploadImage(file);
        }

        public async Task<bool> updateMenu(MenuData menu, string ogName)
        {
            return await connectAdmin.UpdateMenu(menu, ogName);
        }

        public async Task<List<OrderData>> getOrders(DateTime dtToday)
        {
            return await connectAdmin.GetOrders(dtToday);
        }

        public async Task<List<OrderData>> getOrdersDay(DateTime dtToday)
        {
            return await connectAdmin.GetOrdersDay(dtToday);
        }

        public async Task<List<OrderData>> getOrdersWeek(DateTime dtToday)
        {
            return await connectAdmin.GetOrdersWeek(dtToday);
        }

        public async Task<List<OrderData>> getOrdersMonth(DateTime dtToday)
        {
            return await connectAdmin.GetOrdersMonth(dtToday);
        }

        public async Task<List<OrderDetailsData>> getOrderDetails(OrderData order)
        {
            return await connectAdmin.GetOrderDetails(order);
        }

        public async Task<UserData> getClientDetails(OrderData order)
        {
            return await connectAdmin.GetClientDetails(order);
        }
    }
}
